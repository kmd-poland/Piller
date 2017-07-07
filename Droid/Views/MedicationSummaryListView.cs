using Android.App;
using Android.OS;
using Piller.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using Piller.Resources;
using MvvmCross.Binding.BindingContext;
using Android.Support.Design.Widget;
using Android.Views;
using System;

using Toolbar = Android.Support.V7.Widget.Toolbar;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Binding.Droid.BindingContext;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using Android.Runtime;
using MvvmCross.Droid.Shared.Attributes;

namespace Piller.Droid.Views
{
	[MvxFragment(typeof(RootViewModel), Resource.Id.content_frame, false)]
	[Register("piller.droid.views.MedicationSummaryListView")]
	public class MedicationSummaryListView : MvxFragment<MedicationSummaryListViewModel>
	{
		FloatingActionButton newMedicationDosage;
		MvxListView medicationList;
		TextView emptyLabel;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
		{
			var ignore = base.OnCreateView(inflater, container, bundle);

            var view = this.BindingInflate(Resource.Layout.MedicationSummaryListView, null);
			((MvxCachingFragmentCompatActivity)Activity).SupportActionBar.Title = AppResources.MedicationSummaryListViewModel_Title;


            return view;
		}

		private void SetBinding()
		{
			
		}
	}
}