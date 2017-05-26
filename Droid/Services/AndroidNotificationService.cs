using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V7.App;
using Java.Text;
using Java.Util;
using MvvmCross.Platform;
using Piller.Data;
using Piller.Services;

namespace Piller.Droid.Services
{
    public class AndroidNotificationService : INotificationService
    {
        private readonly static string everyday = "everyday";

        private readonly IPermanentStorageService storage = Mvx.Resolve<IPermanentStorageService>();

		private Context ctx;

		public AndroidNotificationService(Context context)
		{
			this.ctx = context;
		}

        public async Task ScheduleNotification(MedicationDosage medicationDosage)
		{
			Intent notificationIntent = new Intent(this.ctx, typeof(NotificationPublisher));
			var notificationDefaults = NotificationDefaults.Lights | NotificationDefaults.Sound | NotificationDefaults.Vibrate;

            // cancel previously scheduled notifications
            await this.CancelNotification(medicationDosage.Id.Value);

			if (medicationDosage.Days.AllSelected()) {
                // schedule for every occurrence of hour for every 24 hours
                foreach (var hour in medicationDosage.HoursEncoded.Split(';'))
                {
                    var nextOccurrenceDate = this.NextOccurrenceFromHour(TimeSpan.Parse(hour));
                    var not = NotificationHelper.GetNotification(this.ctx, medicationDosage, nextOccurrenceDate, notificationIntent);
                    not.Defaults |= notificationDefaults; // todo get from settings or medication itself (if set custom in medication, otherwise global value from settings)
                    await this.SetAlarm(not, medicationDosage.Id.Value, nextOccurrenceDate, notificationIntent);
                }
            } else {
                // schedule in a weekly manner for each day of week
                foreach (var hour in medicationDosage.HoursEncoded.Split(';'))
                {
                    foreach (var day in medicationDosage.Days.GetSelected())
                    {
                        var nextOccurrenceDate = this.NextOccurrenceFromHourAndDayOfWeek(day, TimeSpan.Parse(hour));
                        var not = NotificationHelper.GetNotification(this.ctx, medicationDosage, nextOccurrenceDate, notificationIntent);
						not.Defaults |= notificationDefaults; // todo get from settings or medication itself (if set custom in medication, otherwise global value from settings)
                        await this.SetAlarm(not, medicationDosage.Id.Value, nextOccurrenceDate, notificationIntent);
					}
				}
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

            // remove notifications from storage
            foreach (var notificationId in occurrences.Select(o => o.Id.Value).ToList())
                await this.storage.DeleteByKeyAsync<NotificationOccurrence>(notificationId);
		}

        private async Task SetAlarm(Notification notification, int id, DateTime occurrenceDate, Intent notificationIntent)
        {
			var firingCal = Java.Util.Calendar.Instance;

            firingCal.Set(CalendarField.Year, occurrenceDate.Year);
            firingCal.Set(CalendarField.Month, occurrenceDate.Month - 1);
            firingCal.Set(CalendarField.DayOfMonth, occurrenceDate.Day);
			firingCal.Set(CalendarField.HourOfDay, occurrenceDate.Hour);
			firingCal.Set(CalendarField.Minute, occurrenceDate.Minute);
			firingCal.Set(CalendarField.Second, occurrenceDate.Second);

			var triggerTime = firingCal.TimeInMillis;

            // for test purposes only
			var dateFormat = new SimpleDateFormat("dd:MM:yy:HH:mm:ss");
			var cal = dateFormat.Format(triggerTime);

            var notificationOccurrence = new NotificationOccurrence(id, occurrenceDate, triggerTime);
            await this.storage.SaveAsync(notificationOccurrence);

            notificationIntent.PutExtra(NotificationPublisher.NOTIFICATION_ID, notificationOccurrence.Id.Value);
			notificationIntent.PutExtra(NotificationPublisher.MEDICATION_ID, id);
			notificationIntent.PutExtra(NotificationPublisher.NOTIFICATION, notification);
			notificationIntent.PutExtra(NotificationPublisher.NOTIFICATION_FIRE_TIME, triggerTime);

			var requestId = DateTime.Now.Millisecond;
			PendingIntent pendingIntent = PendingIntent.GetBroadcast(this.ctx, requestId, notificationIntent, PendingIntentFlags.CancelCurrent);

			AlarmManager alarmManager = (AlarmManager)this.ctx.GetSystemService(Context.AlarmService);

			alarmManager.SetExact(AlarmType.RtcWakeup, triggerTime, pendingIntent);
		}
    }

    public static class DaysOfWeekMixIn
    {
        public static bool AllSelected(this DaysOfWeek This)
        {
            return (This.HasFlag(DaysOfWeek.Monday)
                    && This.HasFlag(DaysOfWeek.Tuesday)
                    && This.HasFlag(DaysOfWeek.Wednesday)
                    && This.HasFlag(DaysOfWeek.Thursday)
                    && This.HasFlag(DaysOfWeek.Friday)
                    && This.HasFlag(DaysOfWeek.Saturday)
                    && This.HasFlag(DaysOfWeek.Sunday));
        }

        public static IEnumerable<DaysOfWeek> GetSelected(this DaysOfWeek This)
        {
            if (This.HasFlag(DaysOfWeek.Monday))
                yield return DaysOfWeek.Monday;
            if (This.HasFlag(DaysOfWeek.Tuesday))
                yield return DaysOfWeek.Tuesday;
            if (This.HasFlag(DaysOfWeek.Wednesday))
                yield return DaysOfWeek.Wednesday;
            if (This.HasFlag(DaysOfWeek.Thursday))
                yield return DaysOfWeek.Thursday;
            if (This.HasFlag(DaysOfWeek.Friday))
                yield return DaysOfWeek.Friday;
            if (This.HasFlag(DaysOfWeek.Saturday))
                yield return DaysOfWeek.Saturday;
            if (This.HasFlag(DaysOfWeek.Sunday))
                yield return DaysOfWeek.Sunday;
        }

        public static string DayOfWeekName(this DaysOfWeek This)
        {
            if (This == DaysOfWeek.Monday)
                return DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Monday);
			if (This == DaysOfWeek.Tuesday)
				return DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Tuesday);
			if (This == DaysOfWeek.Wednesday)
				return DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Wednesday);
			if (This == DaysOfWeek.Thursday)
				return DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Thursday);
			if (This == DaysOfWeek.Friday)
				return DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Friday);
			if (This == DaysOfWeek.Saturday)
				return DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Saturday);
			if (This == DaysOfWeek.Sunday)
                return DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Sunday);

            return null;
        }

        public static bool EqualsDaysOfWeek(this DayOfWeek This, DaysOfWeek day)
        {
            return Enum.GetName(typeof(DayOfWeek), This).ToLower() == Enum.GetName(typeof(DaysOfWeek), day).ToLower();
        }

        public static DaysOfWeek ToDaysOfWeek (this DayOfWeek dayOfWeek)
        {
            var dayOfWeekName = Enum.GetName(typeof(DayOfWeek), dayOfWeek);
            var result = (DaysOfWeek)Enum.Parse(typeof(DaysOfWeek), dayOfWeekName);
            return result;
        }

        public static int GetOrdinal(this DaysOfWeek This)
        {
            if (This == DaysOfWeek.Monday)
                return 0;
            if (This == DaysOfWeek.Tuesday)
				return 1;
			if (This == DaysOfWeek.Wednesday)
				return 2;
            if (This == DaysOfWeek.Thursday)
				return 3;
            if (This == DaysOfWeek.Friday)
				return 4;
            if (This == DaysOfWeek.Saturday)
				return 5;
            if (This == DaysOfWeek.Sunday)
				return 6;

            throw new ArgumentOutOfRangeException();
        }
    }

    public static class NotificationHelper
    {
		public static Notification GetNotification(Context context, MedicationDosage medication, DateTime occurrenceDate, Intent notificationIntent)
		{
			var builder = new NotificationCompat.Builder(context);
			builder.SetContentTitle(medication.Name);
			builder.SetContentText(medication.Dosage + " - " + FormatOccurrence(occurrenceDate));
			builder.SetTicker($"[PILLER] {medication.Name}");
			builder.SetSmallIcon(Resource.Drawable.pill);

			builder.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Alarm));
			builder.SetPriority((int)NotificationPriority.High);
			builder.SetVisibility((int)NotificationVisibility.Public); // visible on locked screen

			var actionTaken = GetAction(context, NotificationConsts.PillTakenAction, "Tak", builder, notificationIntent, () => { System.Diagnostics.Debug.WriteLine("ACTION!"); });
			builder.AddAction(actionTaken);
            var actionNotTaken = GetAction(context, NotificationConsts.PillNotTakenAction, "Nie", builder, notificationIntent, () => { System.Diagnostics.Debug.WriteLine("ACTION!"); });
			builder.AddAction(actionNotTaken);
			var actionPostpone = GetAction(context, NotificationConsts.PillNotTakenAction, "Za 15 minut", builder, notificationIntent, () => { System.Diagnostics.Debug.WriteLine("ACTION!"); });
            builder.AddAction(actionPostpone);

			return builder.Build();
		}

		private static string FormatOccurrence(DateTime nearestOccurrence)
		{
			return $"(Data przyjęcia: {nearestOccurrence.ToString("f")}";
		}

		private static NotificationCompat.Action GetAction(Context context, string actionId, string actionDescription, NotificationCompat.Builder builder, Intent notificationIntent, Action actualAction)
		{
            var remoteInput = new Android.Support.V4.App.RemoteInput.Builder(actionId).SetLabel(actionDescription).Build();
			var pendingIntent = PendingIntent.GetBroadcast(context, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);
			return new NotificationCompat.Action.Builder(Resource.Drawable.pill, actionDescription, pendingIntent).AddRemoteInput(remoteInput).Build();
		}
	}

    public static class NotificationConsts
    {
        public readonly static string PillTakenAction = "pill-taken";
		public readonly static string PillNotTakenAction = "pill-not-taken";
        public readonly static string Postpone = "postpone";
    }
}
