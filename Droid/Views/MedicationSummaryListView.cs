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

namespace Piller.Droid.Views
{
	[Activity]
	public class MedicationSummaryListView : MvxAppCompatActivity<MedicationSummaryListViewModel>
	{


		FloatingActionButton newMedicationDosage;
		MvxListView medicationList;
		TextView emptyLabel;


		protected override void OnCreate(Bundle bundle)
		{

			base.OnCreate(bundle);
			SetContentView(Resource.Layout.MedicationSummaryListView);

			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			newMedicationDosage = FindViewById<FloatingActionButton>(Resource.Id.newMedicationDosage);
			emptyLabel = FindViewById<TextView>(Resource.Id.empty);

			medicationList = FindViewById<MvxListView>(Resource.Id.medicationList);
			medicationList.Adapter = new MedicationSummaryAdapter(this, (IMvxAndroidBindingContext)this.BindingContext);
			medicationList.ItemTemplateId = Resource.Layout.medication_summary_item;

			//Toolbar will now take on default actionbar characteristics
			SetSupportActionBar(toolbar);

			SupportActionBar.Title = AppResources.MedicationSummaryListViewModel_Title;

			SetBinding();
		}
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.main_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.action_settings)
                ViewModel.GoSettings.Execute().Subscribe();

            return base.OnOptionsItemSelected(item);
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