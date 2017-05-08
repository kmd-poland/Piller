using System;
using Android.App;
using Piller.Core.Domain;
using Piller.Core.Services;
using Android.Content;
using Piller.Droid.Views;
using MvvmCross.Platform.Droid.Platform;
using Java.Util;
using System.Threading.Tasks;
using System.Collections.Generic;
using Piller.ViewModels;

namespace Piller.Droid.Services
{
    public class AndroidNotificationService : INotificationService
    {
        private Context ctx;

        public AndroidNotificationService (Context context)
        {
            this.ctx = context;
        }

        public Task ScheduleNotification (CoreNotification coreNotification)
        {
            var task = new TaskFactory ().StartNew (() => {
                // todo return an identifier(s) for a notification(s) to store in db to be able to cancel them later
                // todo create notifications for all occrrences using AlarmManager.SetRepeating method
                var notification = this.GetNotification (coreNotification, DateTime.Now.Date);
                
                Intent notificationIntent = new Intent (this.ctx, typeof (NotificationPublisher));
                notificationIntent.PutExtra (NotificationPublisher.NOTIFICATION_ID, coreNotification.Id);
                notificationIntent.PutExtra (NotificationPublisher.NOTIFICATION, notification);
                PendingIntent pendingIntent = PendingIntent.GetBroadcast (this.ctx, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);

                var firingCal = Calendar.Instance;
                var currentCal = Calendar.Instance;
                List<TimeSpan> hours = coreNotification.Pattern.Hours;
                foreach(var hour in hours)
                {
                    // todo set time according to occurrence
                      //firingCal.Set(CalendarField.DayOfWeek, EnumSet.Of(coreNotification.Pattern.DayOfWeek));
                    firingCal.Set (CalendarField.HourOfDay, hour.Hours); // At the hour you wanna fire
                    firingCal.Set (CalendarField.Minute, hour.Minutes); // Particular minute
                    firingCal.Set (CalendarField.Second, 00); // particular second
                    if (firingCal.CompareTo (currentCal) < 0) {
                        firingCal.Add (CalendarField.DayOfMonth, 1);
                    }
                   // firingCal.Add(CalendarField.Second, 5);
                    var triggerTime = firingCal.TimeInMillis; // DateTime.Now.FromLocalToUnixTime();

                    AlarmManager alarmManager = (AlarmManager)this.ctx.GetSystemService (Context.AlarmService);
                    alarmManager.SetRepeating (AlarmType.RtcWakeup, triggerTime, 1000*10 /* or explicit value of millis, for example 10000*/, pendingIntent);
                   // alarmManager.SetInexactRepeating(AlarmType.RtcWakeup, triggerTime, 1000*10, pendingIntent);
                }
                // or
                //alarmManager.SetExact();
                // or others
            });

            return task;
        }

        public void CancelNotification (int id)
        {
            var alarmManager = (AlarmManager)this.ctx.GetSystemService (Context.AlarmService);
            Intent intent = new Intent (this.ctx, typeof (NotificationPublisher));
            PendingIntent alarmIntent = PendingIntent.GetBroadcast (this.ctx, 0, intent, 0);

            alarmManager.Cancel (alarmIntent);
        }


        /// <summary>
        /// Creates a single notification, an instance of <see cref="Android.App.Notification"/> class.
        /// </summary>
        /// <returns>Android notification created from <b>notification</b> parameter.</returns>
        /// <param name="notification"><b>CoreNotification</b></param>
        private Notification GetNotification (CoreNotification notification, DateTime occurrence)
        {
           // Intent resutlIntent = new Intent(this.ctx, typeof(MedicationSummaryListView));
           // PendingIntent pendingIntent= PendingIntent.GetActivity(this.ctx,0,resutlIntent, PendingIntentFlags.OneShot);
        	var builder = new Notification.Builder (this.ctx);
        	builder.SetContentTitle (notification.Title);
        	builder.SetContentText (notification.Message);
        	builder.SetSmallIcon (Resource.Drawable.Icon);
          //  builder.SetContentIntent(pendingIntent);
            builder.SetVisibility(NotificationVisibility.Public);
        	return builder.Build ();
        }

        private string FormatOccurrence(DateTime occurrence)
        {
            return $"(Data przyjęcia: {occurrence.ToString("g")})";
        }
    }
}
