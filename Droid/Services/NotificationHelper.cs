using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V7.App;
using Piller.Data;
using Services;
using Android.Graphics;
using MvvmCross.Platform;
using Android.Widget;

namespace Piller.Droid.Services
{
    public static class NotificationHelper
    {
        public static Notification GetNotification(Context context, MedicationDosage medication, NotificationOccurrence notificationOccurrence, Intent notificationIntent)
        {
            var builder = new NotificationCompat.Builder(context);
            builder.SetContentTitle(medication.Name);
            builder.SetTicker($"[PILLER] {medication.Name}");
			builder.SetSmallIcon(Resource.Drawable.pill64x64);

            builder.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Alarm));
            builder.SetPriority((int)NotificationPriority.High);
            builder.SetVisibility((int)NotificationVisibility.Public); // visible on locked screen

            RemoteViews contentView = new RemoteViews(context.PackageName, Resource.Layout.customNotification);
            contentView.SetTextViewText(Resource.Id.titleTextView, medication.Name + " " + medication.Dosage);
            contentView.SetTextViewText(Resource.Id.descTextView, FormatOccurrence(notificationOccurrence.OccurrenceDateTime));
            contentView.SetImageViewBitmap(Resource.Id.iconView, BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.pill64x64));

            if (medication?.ThumbnailName == null)
				contentView.SetImageViewBitmap(Resource.Id.imageView, BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.pill64x64));
            else
            {
                ImageLoaderService imageLoader = Mvx.Resolve<ImageLoaderService>();
                byte[] array = imageLoader.LoadImage(medication.ThumbnailName);
                contentView.SetImageViewBitmap(Resource.Id.imageView, BitmapFactory.DecodeByteArray(array, 0, array.Length));
            }

            RemoteViews contentBigView = new RemoteViews(context.PackageName, Resource.Layout.customBigNotification);
            contentBigView.SetTextViewText(Resource.Id.titleTextView, medication.Name + " " + medication.Dosage);
            contentBigView.SetTextViewText(Resource.Id.descTextView, FormatOccurrence(notificationOccurrence.OccurrenceDateTime));
            contentBigView.SetImageViewBitmap(Resource.Id.iconView, BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.pill64x64));
            
            var medicationId = medication.Id.Value;
            var notificationId = notificationOccurrence.Id.Value;
            System.Diagnostics.Debug.Write(notificationId);

            Intent okIntent = new Intent(notificationIntent);
            Intent noIntent = new Intent(notificationIntent);
            Intent laterIntent = new Intent(notificationIntent);

            notificationIntent.PutExtra(NotificationPublisher.MEDICATION_ID, medicationId);
            okIntent.PutExtra(NotificationPublisher.MEDICATION_ID, medicationId);
            noIntent.PutExtra(NotificationPublisher.MEDICATION_ID, medicationId);
            laterIntent.PutExtra(NotificationPublisher.MEDICATION_ID, medicationId);
            notificationIntent.PutExtra(NotificationPublisher.NOTIFICATION_ID, notificationId);
            okIntent.PutExtra(NotificationPublisher.NOTIFICATION_ID, notificationId);
            noIntent.PutExtra(NotificationPublisher.NOTIFICATION_ID, notificationId);
            laterIntent.PutExtra(NotificationPublisher.NOTIFICATION_ID, notificationId);

            okIntent.SetAction("OK");
            noIntent.SetAction("LATER");
            //laterIntent.SetAction("LATER");

            PendingIntent ok_intent = PendingIntent.GetBroadcast(context, Environment.TickCount, okIntent, 0);
            contentBigView.SetOnClickPendingIntent(Resource.Id.okButton, ok_intent);

            PendingIntent no_intent = PendingIntent.GetBroadcast(context, Environment.TickCount, noIntent, 0);
            contentBigView.SetOnClickPendingIntent(Resource.Id.noButton, no_intent);

            //PendingIntent later_intent = PendingIntent.GetBroadcast(context, Environment.TickCount, laterIntent, 0);
            //contentBigView.SetOnClickPendingIntent(Resource.Id.laterButton, later_intent);
            //contentBigView.SetImageViewBitmap(Resource.Id.imageView, BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.pill64x64));

            if (medication?.ThumbnailName == null)
				contentBigView.SetImageViewBitmap(Resource.Id.imageView, BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.pill64x64));
            else
            {
                ImageLoaderService imageLoader = Mvx.Resolve<ImageLoaderService>();
                byte[] array = imageLoader.LoadImage(medication.ThumbnailName);
                contentBigView.SetImageViewBitmap(Resource.Id.imageView, BitmapFactory.DecodeByteArray(array, 0, array.Length));
            }

            builder.SetCustomContentView(contentView);
            builder.SetCustomBigContentView(contentBigView);
           
            return builder.Build();
        }

        private static string FormatOccurrence(DateTime nearestOccurrence)
        {
            return $"{nearestOccurrence:f}";
        }
    }
}