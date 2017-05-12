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
using Android.Media;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;

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

                var firingCal = Calendar.Instance;
                var currentCal = Calendar.Instance;
                var hour = coreNotification.Pattern.Hour;
                var days = coreNotification.Pattern.DayOfWeek;
                int id = coreNotification.Pattern.AlarmId;
                PendingIntent pendingIntent= PendingIntent.GetBroadcast (this.ctx, id, notificationIntent, PendingIntentFlags.UpdateCurrent);
                  
                    // todo set time according to occurrence
                    //firingCal.Set(CalendarField.DayOfWeek, );
                    firingCal.Set (CalendarField.HourOfDay, hour.Hours); // At the hour you wanna fire
                    firingCal.Set (CalendarField.Minute, hour.Minutes); // Particular minute 
                    if (firingCal.CompareTo (currentCal) < 0) {
                        firingCal.Add (CalendarField.DayOfMonth, 1);
                    }
                   // firingCal.Add(CalendarField.Second, 5);
                    var triggerTime = firingCal.TimeInMillis; // DateTime.Now.FromLocalToUnixTime();

                    AlarmManager alarmManager = (AlarmManager)this.ctx.GetSystemService (Context.AlarmService);
                    alarmManager.SetRepeating (AlarmType.RtcWakeup, triggerTime, AlarmManager.IntervalDay*7 /* or explicit value of millis, for example 10000*/, pendingIntent);
                    
                    // alarmManager.SetInexactRepeating(AlarmType.RtcWakeup, triggerTime, 1000*10, pendingIntent);
                   
                
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
            PendingIntent alarmIntent = PendingIntent.GetBroadcast (this.ctx, id, intent, 0);

            alarmManager.Cancel (alarmIntent);
        }
        /*
        Intent GetRouteViewIntent()
        {
            var request = new MvxViewModelRequest();
            request.ParameterValues = new Dictionary<string, string>();
            request.ParameterValues.Add("idRoute", 0.ToString());
            request.ViewModelType = typeof(MedicationSummaryListView);
            var requestedTranslator = Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>();
            return requestedTranslator.GetIntentFor(request);
        }

    */
        /// <summary>
        /// Creates a single notification, an instance of <see cref="Android.App.Notification"/> class.
        /// </summary>
        /// <returns>Android notification created from <b>notification</b> parameter.</returns>
        /// <param name="notification"><b>CoreNotification</b></param>
        private Notification GetNotification (CoreNotification notification, DateTime occurrence)
        {
            // Intent resutlIntent = new Intent(this.ctx, typeof(MedicationSummaryListView));
           // var intent = GetRouteViewIntent();
           
           // PendingIntent pendingIntent= PendingIntent.GetActivity(this.ctx,0,intent, PendingIntentFlags.UpdateCurrent);
            var builder = new Notification.Builder (this.ctx);
        	builder.SetContentTitle (notification.Title);
        	builder.SetContentText (notification.Message);
        	builder.SetSmallIcon (Resource.Drawable.Icon);
            builder.SetDefaults(NotificationDefaults.Sound);
           // builder.SetContentIntent(pendingIntent);
            builder.SetVisibility(NotificationVisibility.Public);
        	return builder.Build ();
        }

        private string FormatOccurrence(DateTime occurrence)
        {
            return $"(Data przyjęcia: {occurrence.ToString("g")})";
        }
    }
}
