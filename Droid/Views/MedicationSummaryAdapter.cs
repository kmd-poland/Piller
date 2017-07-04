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
using ReactiveUI;

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
            var dosage = view.FindViewById<TextView>(Resource.Id.label_medication_dosage);

            var ordination = view.FindViewById<TextView>(Resource.Id.label_medication_ordination);

            var medication = dataContext as MedicationDosage;

            name.Text = medication.Name;
            dosage.Text = medication.Dosage;
            ordination.Text = $"{new DaysOfWeekConverter().Convert(medication.Days,typeof(string),null,System.Globalization.CultureInfo.CurrentCulture).ToString()}: {medication.Hours}" ;
           
            if (medication?.ThumbnailName != null)
            {
				var thumbnail = view.FindViewById<ImageView>(Resource.Id.list_thumbnail);
				byte[] array = imageLoader.LoadImage(medication.ThumbnailName);
				thumbnail.SetImageBitmap(BitmapFactory.DecodeByteArray(array, 0 ,array.Length));    
            } else {
                var thumbnail = view.FindViewById<ImageView>(Resource.Id.list_thumbnail);
				thumbnail.SetImageBitmap(BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.pill64x64));
			}

			return view;
		}
	}
}
