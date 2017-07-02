using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Java.Text;
using Java.Util;
using MvvmCross.Platform;
using Piller.Data;
using Piller.MixIns.DaysOfWeekMixIns;
using Piller.Services;

namespace Piller.Droid.Services
{
    public class AndroidNotificationService : INotificationService
    {
        private static readonly string everyday = "everyday";

        private readonly IPermanentStorageService storage = Mvx.Resolve<IPermanentStorageService>();

		private Context ctx;

		public AndroidNotificationService(Context context)
		{
			this.ctx = context;
		}

        public async Task ScheduleNotifications(MedicationDosage medicationDosage)
		{
			Intent notificationIntent = new Intent(this.ctx, typeof(NotificationPublisher));

            var notificationDefaults = NotificationDefaults.Lights | NotificationDefaults.Sound | NotificationDefaults.Vibrate;

            // cancel previously scheduled notifications
            // no; notifications have been cancelled already

            // NotificationOccurrence table already contains nearest occurrences, just loop over them to shcedule
            var notificationOccurrences = await this.storage.List<NotificationOccurrence>(o => o.MedicationDosageId == medicationDosage.Id.Value);

            foreach (var notificationOccurrence in notificationOccurrences)
            {
                var not = NotificationHelper.GetNotification(this.ctx, medicationDosage, notificationOccurrence, notificationIntent);
				not.Defaults |= notificationDefaults; // todo get from settings or medication itself (if set custom in medication, otherwise global value from settings)
				await this.SetAlarm(medicationDosage.Name, medicationDosage.Dosage, not, medicationDosage.Id.Value, notificationOccurrence, notificationIntent);
			}
		}

        

		private DateTime NextOccurrenceFromHour(TimeSpan hour)
		{
			var occurrenceDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour.Hours, hour.Minutes, 0);
			if (DateTime.Now.Hour > hour.Hours)
				return occurrenceDate.AddDays(1);

			return occurrenceDate;
		}

        private DateTime NextOccurrenceFromHourAndDayOfWeek(DaysOfWeek day, TimeSpan hour)
        {
			var occurrenceDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour.Hours, hour.Minutes, 0);

            if (DateTime.Now.DayOfWeek.EqualsDaysOfWeek(day)) {
                if (occurrenceDate < DateTime.Now)
                    occurrenceDate = occurrenceDate.AddDays(7);
            } else {
                occurrenceDate = occurrenceDate.AddDays(this.GetDaysToNextDayOfWeek(day, occurrenceDate.DayOfWeek.ToDaysOfWeek()));
            }

			return occurrenceDate;
        }

        private int GetDaysToNextDayOfWeek(DaysOfWeek day, DaysOfWeek currentDayOfWeek)
        {
            var daysOfWeekOrdinal = day.GetOrdinal();
            var currentDayOfWeekOrdinal = currentDayOfWeek.GetOrdinal();
            if (daysOfWeekOrdinal < currentDayOfWeekOrdinal)
                return 7 - currentDayOfWeekOrdinal - daysOfWeekOrdinal;
            else if (daysOfWeekOrdinal > currentDayOfWeekOrdinal)
                return daysOfWeekOrdinal - currentDayOfWeekOrdinal;
            else
                return 0;
        }

        public async Task CancelNotification(NotificationOccurrence notificationOccurrence)
        {
            // todo removing not matter if actual notification has been cancelled or not 
            await this.storage.DeleteAsync(notificationOccurrence);

            var alarmManager = (AlarmManager)this.ctx.GetSystemService(Context.AlarmService);
            Intent intent = new Intent(this.ctx, typeof(NotificationPublisher));
            if (PendingIntent.GetBroadcast(this.ctx, notificationOccurrence.Id.Value, intent, PendingIntentFlags.NoCreate) != null)
            {
                PendingIntent alarmIntent = PendingIntent.GetBroadcast(this.ctx, notificationOccurrence.Id.Value, intent, PendingIntentFlags.CancelCurrent);
                alarmManager.Cancel(alarmIntent);
            }
            else
            {
                System.Diagnostics.Debug.Write($"Alarm with id {notificationOccurrence.Id.Value} does not exist.");
            }
        }

        public async Task CancelNotifications(MedicationDosage medicationDosage)
        {
            var notifications = await this.storage.List<NotificationOccurrence>(o => o.MedicationDosageId == medicationDosage.Id.Value);
            foreach (var notification in notifications)
                await this.CancelNotification(notification);
        }

        public async Task CancelNotification(int medicationDosageId)
		{
            // remove scheduled alarms
			var alarmManager = (AlarmManager)this.ctx.GetSystemService(Context.AlarmService);
			Intent intent = new Intent(this.ctx, typeof(NotificationPublisher));
            var occurrences = await this.storage.List<NotificationOccurrence>(n => n.MedicationDosageId == medicationDosageId);
            foreach (var item in occurrences)
            {
                if (PendingIntent.GetBroadcast(this.ctx, item.Id.Value, intent, PendingIntentFlags.NoCreate) != null)
                {
                    PendingIntent alarmIntent = PendingIntent.GetBroadcast(this.ctx, item.Id.Value, intent, PendingIntentFlags.CancelCurrent);
                    alarmManager.Cancel(alarmIntent);
                } else {
                    System.Diagnostics.Debug.Write($"Alarm with id {item.Id.Value} does not exist.");
                }
            }

		}

        public async Task CancelAndRemove(int medicationDosageId)
        {
            var occurrences = await this.storage.List<NotificationOccurrence>(n => n.MedicationDosageId == medicationDosageId);
            await CancelNotification(medicationDosageId);
            foreach (var notificationId in occurrences.Select(o => o.Id.Value).ToList())
                await this.storage.DeleteByKeyAsync<NotificationOccurrence>(notificationId);
        }

        public async Task OverdueNotification(NotificationOccurrence notificationOccurrence, MedicationDosage medicationDosage)
        {
            var notificationIntent = new Intent(this.ctx, typeof(NotificationPublisher));
            var not = NotificationHelper.GetNotification(this.ctx, medicationDosage, notificationOccurrence, notificationIntent);
            await this.SetAlarm(medicationDosage.Name, medicationDosage.Dosage, not, medicationDosage.Id.Value, notificationOccurrence, notificationIntent);
        }

        private async Task SetAlarm(String name, String dosage, Notification notification, int id, NotificationOccurrence notificationOccurrence, Intent notificationIntent)
        {
			var firingCal = Java.Util.Calendar.Instance;

            firingCal.Set(CalendarField.Year, notificationOccurrence.OccurrenceDateTime.Year);
            firingCal.Set(CalendarField.Month, notificationOccurrence.OccurrenceDateTime.Month - 1);
            firingCal.Set(CalendarField.DayOfMonth, notificationOccurrence.OccurrenceDateTime.Day);
			firingCal.Set(CalendarField.HourOfDay, notificationOccurrence.OccurrenceDateTime.Hour);
			firingCal.Set(CalendarField.Minute, notificationOccurrence.OccurrenceDateTime.Minute);
			firingCal.Set(CalendarField.Second, notificationOccurrence.OccurrenceDateTime.Second);

			var triggerTime = firingCal.TimeInMillis;

            // for test purposes only
			var dateFormat = new SimpleDateFormat("dd:MM:yy:HH:mm:ss");
			var cal = dateFormat.Format(triggerTime);

            notificationIntent.PutExtra(NotificationPublisher.NOTIFICATION_ID, notificationOccurrence.Id.Value);
			notificationIntent.PutExtra(NotificationPublisher.MEDICATION_ID, id);
			notificationIntent.PutExtra(NotificationPublisher.NOTIFICATION, notification);
			notificationIntent.PutExtra(NotificationPublisher.NOTIFICATION_FIRE_TIME, triggerTime);

            var requestId = notificationOccurrence.Id.Value;
			PendingIntent pendingIntent = PendingIntent.GetBroadcast(this.ctx, requestId, notificationIntent, PendingIntentFlags.CancelCurrent);

			AlarmManager alarmManager = (AlarmManager)this.ctx.GetSystemService(Context.AlarmService);

			alarmManager.SetExact(AlarmType.RtcWakeup, triggerTime, pendingIntent);
		}
    }
}
