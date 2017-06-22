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
	[MvxFragment(typeof(RootViewModel), Resource.Id.content_frame, true)]
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

			var toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
			newMedicationDosage = view.FindViewById<FloatingActionButton>(Resource.Id.newMedicationDosage);
			emptyLabel = view.FindViewById<TextView>(Resource.Id.empty);

			medicationList = view.FindViewById<MvxListView>(Resource.Id.medicationList);
            medicationList.Adapter = new MedicationSummaryAdapter(this.Activity, (IMvxAndroidBindingContext)this.BindingContext);
			medicationList.ItemTemplateId = Resource.Layout.medication_summary_item;

            //Toolbar will now take on default actionbar characteristics
            ((MvxCachingFragmentCompatActivity)Activity).SetSupportActionBar(toolbar);
			((MvxCachingFragmentCompatActivity)Activity).SupportActionBar.Title = AppResources.MedicationSummaryListViewModel_Title;

			SetBinding();
            return view;
		}

        private void SetBinding()
		{
			var bindingSet = this.CreateBindingSet<MedicationSummaryListView, MedicationSummaryListViewModel>();

			bindingSet.Bind(newMedicationDosage)
					  .For(nameof(View.Click))
					  .To(x => x.AddNew);

			bindingSet.Bind(medicationList)
				.For(x => x.ItemsSource)
				.To(vm => vm.MedicationList);

			bindingSet.Bind(medicationList)
				.For(x => x.ItemClick)
				.To(vm => vm.Edit);

			bindingSet.Bind(emptyLabel)
				.For(v => v.Visibility)
				.To(vm => vm.IsEmpty)
				.WithConversion(new InlineValueConverter<bool, ViewStates>(isEmpty => isEmpty ? ViewStates.Visible : ViewStates.Gone));



			bindingSet.Apply();
		}
	}
}