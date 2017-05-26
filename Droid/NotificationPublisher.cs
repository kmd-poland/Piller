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

namespace Piller.Droid
{
	[BroadcastReceiver]
	public class NotificationPublisher : BroadcastReceiver
	{
        private IPermanentStorageService storage = Mvx.Resolve<IPermanentStorageService>();

        public static string MEDICATION_ID = "medication-id";
		public static string NOTIFICATION_ID = "notification-id";
		public static string NOTIFICATION = "notification";
		public static string NOTIFICATION_FIRE_TIME = "notification-fire-time";

		public override void OnReceive(Context context, Intent intent)
		{
			NotificationManager notificationManager =
				(NotificationManager)context.GetSystemService(Context.NotificationService);

			var notification = intent.GetParcelableExtra(NOTIFICATION) as Notification;

			if (notification != null)
			{
                var notificationId = intent.GetIntExtra(NOTIFICATION_ID, 0);
                var medicationId = intent.GetIntExtra(MEDICATION_ID, 0);
                var fireTime = intent.GetLongExtra(NOTIFICATION_FIRE_TIME, 0);

				notificationManager.Notify(notificationId, notification);

                var dateFormat = new SimpleDateFormat("dd:MM:yy:HH:mm:ss");
                var cal = dateFormat.Format(fireTime);

                Task.Run(async () =>
                {
                    var notifications = await this.storage.List<NotificationOccurrence>();
                    var currentNotification = notifications.FirstOrDefault(n => n.Id == notificationId);
                    if (currentNotification != null)
                        await this.storage.DeleteAsync(currentNotification);
                    else
                        System.Diagnostics.Debug.Write($"Notification with id {notificationId} could not be found in database.");

                    // find overdue notifications
                    var nowCalendar = Calendar.Instance;
                    var now = nowCalendar.TimeInMillis;
                    var overdueNotificationsOccurrences = notifications.Where(n => n.OccurrenceDateMillis < now);
                    foreach (var overdueNotificationOccurrence in overdueNotificationsOccurrences)
                    {
                        var medication = await this.storage.GetAsync<MedicationDosage>(overdueNotificationOccurrence.MedicationDosageId);
						Intent notificationIntent = new Intent(context, typeof(NotificationPublisher));
                        var overdueNotification = NotificationHelper.GetNotification(context, medication, overdueNotificationOccurrence.OccurrenceDateTime, notificationIntent);
                        notificationManager.Notify(overdueNotificationOccurrence.Id.Value, overdueNotification);
                    }
                });
			}
			else
			{
				System.Diagnostics.Debug.WriteLine(intent.Action);

				var medicationId = intent.GetIntExtra(MEDICATION_ID, 0);
				//if (intent.Action == "Akcja 1") {
				var request = new MvxViewModelRequest();
				request.ParameterValues = new System.Collections.Generic.Dictionary<string, string>();
                request.ParameterValues.Add("nav", JsonConvert.SerializeObject(new MedicationDosageNavigation() { MedicationDosageId = 123 }));
				request.ViewModelType = typeof(MedicationDosageViewModel);
				var requestTranslator = Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>();
				var newActivity = requestTranslator.GetIntentFor(request);
				newActivity.SetFlags(ActivityFlags.NewTask);
				context.StartActivity(newActivity);
				//}
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
