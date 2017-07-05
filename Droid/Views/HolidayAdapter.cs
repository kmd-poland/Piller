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
using Piller.MixIns.DaysOfWeekMixIns;

namespace Piller.Droid.Views
{
	public class HolidayAdapter : MvxAdapter
	{
        private readonly IMvxFileStore fileStore = Mvx.Resolve<IMvxFileStore>();
		private readonly ImageLoaderService imageLoader = Mvx.Resolve<ImageLoaderService>();
		private String from = "";
		private String to = "";
		public HolidayAdapter(Context context) : base(context)
		        {
				}

		public HolidayAdapter(Context context, IMvxAndroidBindingContext bindingContext, String from, String to) : base(context, bindingContext)
		        {
					this.from = from;
           			this.to = to;
				}

		public HolidayAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		        {
				}

        protected override IMvxListItemView CreateBindableView(object dataContext, int templateId)
        {
            var view = base.CreateBindableView(dataContext, templateId) as MvxListItemView;

			var name = view.FindViewById<TextView>(Resource.Id.name);

			var dosage = view.FindViewById<TextView>(Resource.Id.dosage);

			var bset = view.CreateBindingSet<MvxListItemView, MedicationDosage>();

            bset.Bind(name)
			    .To(x => x.NameLabel);

			// Konwertery to specyficzny dla MvvmCross'a sposób translacji danych z view modelu do danych z których potrafi skorzystać widok.
			// Zazwyczaj nie są one potrzebne, np. kiedy pokazujemy tekst, ale jeśli zachodzi potrzeba pokazania np. listy w jednej linii musimy użyć konwertera.

			// TextView jest domyślnie bindowane do property Text, więc nie trzeba jej wprost wskazywać 
            
			bset.Apply();


            var medication = dataContext as MedicationDosage;
            if (medication?.ThumbnailName != null)
            {
				var thumbnail = view.FindViewById<ImageView>(Resource.Id.list_thumbnail);
				byte[] array = imageLoader.LoadImage(medication.ThumbnailName);
				thumbnail.SetImageBitmap(BitmapFactory.DecodeByteArray(array, 0 ,array.Length));
            } else {
                var thumbnail = view.FindViewById<ImageView>(Resource.Id.list_thumbnail);
				thumbnail.SetImageBitmap(BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.pill64x64));
			}

			int amount = 0;


			foreach (var hour in medication.HoursEncoded.Split(';'))
			{
				amount++;
			}

			int days = 0;

			DateTime date1;
			DateTime date2;

			if (!string.IsNullOrEmpty(medication.From) && DateTime.Parse(medication.From).Date > DateTime.Parse(from).Date)
			{
				date1 = DateTime.Parse(medication.From).Date;
			}
			else
				date1 = DateTime.Parse(from).Date;

			if (!string.IsNullOrEmpty(medication.To) && DateTime.Parse(medication.To).Date < DateTime.Parse(to).Date)
			{
				date2 = DateTime.Parse(medication.To).Date;
			}
			else
				date2 = DateTime.Parse(to).Date;



			for(DateTime date = date1; date.Date <= date2; date = date.AddDays(1))
			{
					foreach (var day in medication.Days.GetSelected())
						{
							if (date.DayOfWeek.EqualsDaysOfWeek(day)) {
								days++;
							} 
				       	}
			}

			dosage.Text = (amount * days).ToString();

			if (amount * days == 0)
			{       //sth what hide item
				
			}

				return view;


		}
	}
}
