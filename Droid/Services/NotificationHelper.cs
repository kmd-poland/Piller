using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V7.App;
using Piller.Data;

namespace Piller.Droid.Services
{
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

            var actionTaken = GetAction(context, NotificationConsts.PillTakenAction, medication.Id.Value, "Tak", builder, notificationIntent, () => { System.Diagnostics.Debug.WriteLine("ACTION!"); });
            builder.AddAction(actionTaken);
            var actionNotTaken = GetAction(context, NotificationConsts.PillNotTakenAction, medication.Id.Value, "Nie", builder, notificationIntent, () => { System.Diagnostics.Debug.WriteLine("ACTION!"); });
            builder.AddAction(actionNotTaken);
            var actionPostpone = GetAction(context, NotificationConsts.PillNotTakenAction, medication.Id.Value, "Za 15 minut", builder, notificationIntent, () => { System.Diagnostics.Debug.WriteLine("ACTION!"); });
            builder.AddAction(actionPostpone);

            return builder.Build();
        }

        private static string FormatOccurrence(DateTime nearestOccurrence)
        {
            return $"(Data przyjęcia: {nearestOccurrence:f}";
        }

        private static NotificationCompat.Action GetAction(Context context, string actionId, int medicationId, string actionDescription, NotificationCompat.Builder builder, Intent notificationIntent, Action actualAction)
        {
            var remoteInput = new Android.Support.V4.App.RemoteInput.Builder(actionId).SetLabel(actionDescription).Build();
            var pendingIntent = PendingIntent.GetBroadcast(context, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);
            return new NotificationCompat.Action.Builder(Resource.Drawable.pillSmall, actionDescription, pendingIntent).AddRemoteInput(remoteInput).Build();
        }
    }
}