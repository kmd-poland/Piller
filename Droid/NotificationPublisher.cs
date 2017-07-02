using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.Support.V7.App;
using Java.Text;
using Java.Util;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using Newtonsoft.Json;
using Piller.Data;
using Piller.Droid.Services;
using Piller.Services;
using Piller.ViewModels;
using Piller.MixIns.DaysOfWeekMixIns;
using MvvmCross.Plugins.Messenger;

namespace Piller.Droid
{
	[BroadcastReceiver]
	public class NotificationPublisher : BroadcastReceiver
	{
        private IPermanentStorageService storage = Mvx.Resolve<IPermanentStorageService>();
        private INotificationService notificationService = Mvx.Resolve<INotificationService>();

        public static string MEDICATION_ID = "medication-id";
        public static string MEDICATION_NAME = "medication-name";
        public static string NOTIFICATION_ID = "notification-id";
		public static string NOTIFICATION = "notification";
		public static string NOTIFICATION_FIRE_TIME = "notification-fire-time";

		public override void OnReceive(Context context, Intent intent)
		{
			NotificationManager notificationManager =
				(NotificationManager)context.GetSystemService(Context.NotificationService);

			var notification = intent.GetParcelableExtra(NOTIFICATION) as Notification;
            var medicationId = intent.GetIntExtra(MEDICATION_ID, 0);
            
            if (notification != null)
			{
                var notificationId = intent.GetIntExtra(NOTIFICATION_ID, 0);
                
                var fireTime = intent.GetLongExtra(NOTIFICATION_FIRE_TIME, 0);
                notificationManager.Notify(notificationId, notification);

                var dateFormat = new SimpleDateFormat("dd:MM:yy:HH:mm:ss");
                var cal = dateFormat.Format(fireTime);

                Task.Run(async () =>
                {
                    var notifications = await this.storage.List<NotificationOccurrence>();
                    var currentNotification = notifications.FirstOrDefault(n => n.Id == notificationId);
                    var medications = await this.storage.List<MedicationDosage>();
                    var medicationDosage = medications.FirstOrDefault(n => n.Id == medicationId);
                    //check if current notification is an overdue notification (if it should be sheduled again)
                    TimeSpan currentTimeSpan = currentNotification.OccurrenceDateTime.TimeOfDay;
                    DateTime newOccurrenceDateTime;

                    if (medicationDosage.DosageHours.Contains(currentTimeSpan))
                    {
                        if (medicationDosage.Days.AllSelected())
                            newOccurrenceDateTime = currentNotification.OccurrenceDateTime.AddDays(1);
                        else
                            newOccurrenceDateTime = currentNotification.OccurrenceDateTime.AddDays(7);

                        var occurrence = new NotificationOccurrence()
                        {
                            Name = currentNotification.Name,
                            Dosage = currentNotification.Dosage,
                            MedicationDosageId = currentNotification.MedicationDosageId,
                            OccurrenceDateTime = newOccurrenceDateTime
                        };

                        if (currentNotification != null)
                            await this.storage.DeleteAsync(currentNotification);
                        else
                            System.Diagnostics.Debug.Write($"Notification with id {notificationId} could not be found in database.");

                        await this.notificationService.CancelNotification(medicationId);
                        await this.storage.SaveAsync(occurrence);
                        await this.notificationService.ScheduleNotifications(medicationDosage);
                        Mvx.Resolve<IMvxMessenger>().Publish(new NotificationsChangedMessage(this));
                    }
                    else
                    {
                        if (currentNotification != null)
                            await this.storage.DeleteAsync(currentNotification);
                        else
                            System.Diagnostics.Debug.Write($"Notification with id {notificationId} could not be found in database.");
                    }

                    // find overdue notifications
                    var nowCalendar = Calendar.Instance;
                    var now = nowCalendar.TimeInMillis;
                    var overdueNotificationsOccurrences = notifications.Where(n => n.OccurrenceDateMillis < now);
                    foreach (var overdueNotificationOccurrence in overdueNotificationsOccurrences)
                    {
                        var medication = await this.storage.GetAsync<MedicationDosage>(overdueNotificationOccurrence.MedicationDosageId);

                        Intent notificationIntent = new Intent(context, typeof(NotificationPublisher));
                        
                        notificationIntent.PutExtra(NotificationPublisher.MEDICATION_ID, medicationId);
                        
                        var overdueNotification = NotificationHelper.GetNotification(context, medication, overdueNotificationOccurrence, notificationIntent);
                        notificationManager.Notify(overdueNotificationOccurrence.Id.Value, overdueNotification);
                    }
                });
			}
			else
			{
				System.Diagnostics.Debug.WriteLine(intent.Action);
                System.Diagnostics.Debug.WriteLine(medicationId);

                var notificationId = intent.GetIntExtra(NOTIFICATION_ID, 0);

                if (intent.Action == "OK")
                {
                    notificationManager.Cancel(notificationId);
                }

                /*if (intent.Action == "NO")
                {
                    var fireTime = intent.GetLongExtra(NOTIFICATION_FIRE_TIME, 0);
                    var name = intent.GetStringExtra(MEDICATION_NAME);

                    Task.Run(async () =>
                    {
                        var overdueNotification = new OverdueNotification(medicationId, name, fireTime);
                        await this.storage.SaveAsync(overdueNotification);
                    });

                    notificationManager.Cancel(notificationId);
                }*/

                if (intent.Action == "LATER")
                {
                    Task.Run(async () =>
                    {
                        var name = intent.GetStringExtra(MEDICATION_NAME);
                        var fireTime = intent.GetLongExtra(NOTIFICATION_FIRE_TIME, 0);
                        DateTime now = DateTime.Now;
                        DateTime occurrenceDate = now.AddSeconds(15);

                        var medications = await this.storage.List<MedicationDosage>();
                        var medicationDosage = medications.FirstOrDefault(n => n.Id == medicationId);
						var newNotification = new NotificationOccurrence(medicationDosage.Name, medicationDosage.Dosage, medicationDosage.Id.Value, occurrenceDate, fireTime + 90000);

                        await this.storage.SaveAsync<NotificationOccurrence>(newNotification);
                        Mvx.Resolve<IMvxMessenger>().Publish(new NotificationsChangedMessage(this));
                        await notificationService.OverdueNotification(newNotification, medicationDosage);
                    });

                    notificationManager.Cancel(notificationId);

                }
            }
		}

		public void CancelAlarm(Context context)
		{
			var alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
			Intent intent = new Intent(context, typeof(NotificationPublisher));
			PendingIntent alarmIntent = PendingIntent.GetBroadcast(context, 0, intent, 0);

			alarmManager.Cancel(alarmIntent);

			// Disable <see cref="NotificationPublisher"/> so that it doesn't automatically restart when the device is rebooted.
			// TODO: you may need to reference the context by ApplicationActivity.class
			ComponentName receiver = new ComponentName(context, Java.Lang.Class.FromType(typeof(NotificationPublisher)));
			PackageManager pm = context.PackageManager;
			pm.SetComponentEnabledSetting(receiver, ComponentEnabledState.Disabled, ComponentEnableOption.DontKillApp);
		}

        
	}
}
