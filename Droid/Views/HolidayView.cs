
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;
using Piller.ViewModels;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using MvvmCross.Binding.Droid.Views;
using Piller.Resources;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace Piller.Droid.Views
{
	[MvxFragment(typeof(RootViewModel), Resource.Id.content_frame, true)]
	[Register("piller.droid.views.HolidayView")]
	public class HolidayView : MvxFragment<HolidayViewModel> 
    {

		MvxListView medicationList;
		TextView emptyLabel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
		{
			var ignore = base.OnCreateView(inflater, container, bundle);
			var view = this.BindingInflate(Resource.Layout.HolidayView, null);

			TextView fromDate;
			TextView toDate;

			((MvxCachingFragmentCompatActivity)Activity).SupportActionBar.Title = AppResources.HolidayTitle;


			fromDate = view.FindViewById<TextView>(Resource.Id.oddKiedy);
			toDate = view.FindViewById<TextView>(Resource.Id.ddoKiedy);
			String from = fromDate.Text;
			String to = toDate.Text;

			medicationList = view.FindViewById<MvxListView>(Resource.Id.medicationSearchList);


			fromDate.Click += (o,e) =>{
				
					DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
					{
						fromDate.Text = time.ToShortDateString();
					});

					frag.minDate = DateTime.Now.Date;

				frag.Show(Activity.FragmentManager, DatePickerFragment.TAG);

			};

			toDate.Click += (o,e) => {
					DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
					{
						toDate.Text = time.ToShortDateString();
					});
				if (!string.IsNullOrEmpty(fromDate.Text))
				{
					frag.minDate = DateTime.Parse(fromDate.Text);
				}
				else
				{
					frag.minDate = DateTime.Now.Date;
				}
					frag.Show(Activity.FragmentManager, DatePickerFragment.TAG);

			};

			fromDate.AfterTextChanged += (sender, args) =>
			{
				if (!string.IsNullOrEmpty(fromDate.Text) && !string.IsNullOrEmpty(toDate.Text))
				{
					from = fromDate.Text;
					to = toDate.Text;
					medicationList.Adapter = new HolidayAdapter(this.Activity, (IMvxAndroidBindingContext)this.BindingContext, from, to);
					medicationList.ItemTemplateId = Resource.Layout.holiday_item;
                    SetBinding();
				}
			};

			toDate.AfterTextChanged += (sender, args) =>
			{
				if (!string.IsNullOrEmpty(fromDate.Text) && !string.IsNullOrEmpty(toDate.Text))
				{
					from = fromDate.Text;
					to = toDate.Text;
					medicationList.Adapter = new HolidayAdapter(this.Activity, (IMvxAndroidBindingContext)this.BindingContext, from, to);
					medicationList.ItemTemplateId = Resource.Layout.holiday_item;
                    SetBinding();
				}
			};


			return view;
		}
		private void SetBinding()
		{
			var bindingSet = this.CreateBindingSet<HolidayView, HolidayViewModel>();


		bindingSet.Bind(medicationList)
			.For(x => x.ItemsSource)
			.To(vm => vm.MedicationList);

		bindingSet.Apply();
		}

    }
}
