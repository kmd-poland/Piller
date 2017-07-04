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
        public static Notification GetNotification(Context context, MedicationDosage medication, DateTime occurrenceDate, Intent notificationIntent)
        {
            var builder = new NotificationCompat.Builder(context);
            builder.SetContentTitle(medication.Name);
            builder.SetTicker($"[PILLER] {medication.Name}");
			builder.SetSmallIcon(Resource.Drawable.pill64x64);

			Android.Net.Uri ring = Android.Net.Uri.Parse(medication.RingUri);
            builder.SetSound(ring);

            builder.SetPriority((int)NotificationPriority.High);
            builder.SetVisibility((int)NotificationVisibility.Public); // visible on locked screen

            RemoteViews contentView = new RemoteViews(context.PackageName, Resource.Layout.customNotification);
            contentView.SetTextViewText(Resource.Id.titleTextView, medication.Name);
            contentView.SetTextViewText(Resource.Id.descTextView, medication.Dosage + " - " + FormatOccurrence(occurrenceDate));

            if (medication?.ThumbnailName == null)
				contentView.SetImageViewBitmap(Resource.Id.imageView, BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.pill64x64));
            else
            {
                ImageLoaderService imageLoader = Mvx.Resolve<ImageLoaderService>();
                byte[] array = imageLoader.LoadImage(medication.ThumbnailName);
                contentView.SetImageViewBitmap(Resource.Id.imageView, BitmapFactory.DecodeByteArray(array, 0, array.Length));
            }

            RemoteViews contentBigView = new RemoteViews(context.PackageName, Resource.Layout.customBigNotification);
            contentBigView.SetTextViewText(Resource.Id.titleTextView, medication.Name);
            contentBigView.SetTextViewText(Resource.Id.descTextView, medication.Dosage + " - " + FormatOccurrence(occurrenceDate));

            PendingIntent intent = PendingIntent.GetActivity(context, 0, notificationIntent, 0);
            contentBigView.SetOnClickPendingIntent(Resource.Id.okButton, intent);

            intent = PendingIntent.GetActivity(context, 0, notificationIntent, 0);
            contentBigView.SetOnClickPendingIntent(Resource.Id.noButton, intent);

            intent = PendingIntent.GetActivity(context, 0, notificationIntent, 0);
            contentBigView.SetOnClickPendingIntent(Resource.Id.laterButton, intent);

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
            return $"(Data przyjęcia: {nearestOccurrence:f}";
        }
    }
}