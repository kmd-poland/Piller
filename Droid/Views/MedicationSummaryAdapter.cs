using System;
using Android.Runtime;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.Views;
using Piller.Data;
using MvvmCross.Binding.Droid.BindingContext;
using Android.Content;
using Piller.Droid.BindingConverters;
using Android.Views;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Plugins.File;
using MvvmCross.Platform;
using Android.Graphics;
using Services;
using MvvmCross.Plugins.PictureChooser.Droid;
using Android.Support.V4.Graphics.Drawable;

namespace Piller.Droid.Views
{
	public class MedicationSummaryAdapter : MvxAdapter
	{
        private readonly IMvxFileStore fileStore = Mvx.Resolve<IMvxFileStore>();
		private readonly ImageLoaderService imageLoader = Mvx.Resolve<ImageLoaderService>();

		public MedicationSummaryAdapter(Context context) : base(context)
        {
		}

		public MedicationSummaryAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
		}

		public MedicationSummaryAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
		}

        protected override IMvxListItemView CreateBindableView(object dataContext, int templateId)
        {
            var view = base.CreateBindableView(dataContext, templateId) as MvxListItemView;

            var name = view.FindViewById<TextView>(Resource.Id.label_medication_name);
            var time = view.FindViewById<TextView>(Resource.Id.label_medication_time);
            var daysOfWeek = view.FindViewById<TextView>(Resource.Id.label_medication_days_of_week);
            //var picture = view.FindViewById<ImageView>(Resource.Id.list_thumbnail);

            var bset = view.CreateBindingSet<MvxListItemView, MedicationDosage>();

           // picture.SetImageBitmap(BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.pill));
            bset.Bind(name)
                .To(x => x.Name);

            // Konwertery to specyficzny dla MvvmCross'a sposób translacji danych z view modelu do danych z których potrafi skorzystać widok.
            // Zazwyczaj nie są one potrzebne, np. kiedy pokazujemy tekst, ale jeśli zachodzi potrzeba pokazania np. listy w jednej linii musimy użyć konwertera.

            // TextView jest domyślnie bindowane do property Text, więc nie trzeba jej wprost wskazywać 
            bset.Bind(time)
                .To(x => x.DosageHours)
                .WithConversion(new DosageHoursConverter());

            // jeśli bind ma być do innej property to wskazuje się tak jak poniżej (metoda .For)
            bset.Bind(time)
                .To(x => x.DosageHours)
                .For(v => v.Visibility)
                .WithConversion(new InlineValueConverter<IEnumerable<TimeSpan>, ViewStates>(dosageHours => dosageHours.Any() ? ViewStates.Visible : ViewStates.Gone));

            bset.Bind(daysOfWeek)
				.To(x => x.Days)
                .WithConversion(new DaysOfWeekConverter());

			bset.Bind(daysOfWeek)
				.To(x => x.Days)
                .For(v => v.Visibility)
                .WithConversion(new InlineValueConverter<DaysOfWeek, ViewStates>(dosageHours => dosageHours == DaysOfWeek.None ? ViewStates.Gone : ViewStates.Visible));
            /*
            bset.Bind(picture)
                .To(vm => vm.Bytes)
                .For("Bitmap")
                .WithConversion(new MvxInMemoryImageValueConverter());
                */
            bset.Apply();

            
            var medication = dataContext as MedicationDosage;
            if (medication?.ThumbnailName != null)
            {
				var thumbnail = view.FindViewById<ImageView>(Resource.Id.list_thumbnail);
				byte[] array = imageLoader.LoadImage(medication.ThumbnailName);
                var res = thumbnail.Resources;
                var src = BitmapFactory.DecodeByteArray(array, 0, array.Length);
                RoundedBitmapDrawable dr =RoundedBitmapDrawableFactory.Create(res, src);
                dr.CornerRadius= Math.Max(src.Width, src.Height) / 2.0f;
                dr.Circular = true;
               // thumbnail.SetImageDrawable(dr);

                thumbnail.SetImageBitmap(BitmapFactory.DecodeByteArray(array, 0 ,array.Length));    
            } else {
                var thumbnail = view.FindViewById<ImageView>(Resource.Id.list_thumbnail);
                var src = BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.pill);
                var res = thumbnail.Resources;
                RoundedBitmapDrawable dr = RoundedBitmapDrawableFactory.Create(res, src);
                dr.CornerRadius = Math.Max(src.Width, src.Height) / 2.0f;
               // thumbnail.SetImageDrawable(dr);
                 thumbnail.SetImageBitmap( BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.pill));

            }

            return view;
		}
	}
}
