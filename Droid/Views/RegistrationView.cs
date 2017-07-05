using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Piller.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using System.Reactive.Linq;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Shared.Attributes;
using Piller.Resources;
using System.Threading.Tasks;

namespace Piller.Droid.Views
{

	[MvxFragment(typeof(RootViewModel), Resource.Id.content_frame, true)]
    [Register("piller.droid.views.RegistrationView")]
    public class RegistrationView : MvxFragment<RegistrationViewModel>
    {
        MvxLinearLayout nearestList;
        MvxLinearLayout overdueList;
        MvxLinearLayout laterList;


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
			var baseView = base.OnCreateView(inflater, container, bundle);
			var view = this.BindingInflate(Resource.Layout.RegistrationView, null);

			((MvxCachingFragmentCompatActivity)Activity).SupportActionBar.Title = AppResources.MedicationSummaryListViewModel_Title;

            var toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            nearestList = view.FindViewById<MvxLinearLayout>(Resource.Id.nearestList);
            nearestList.ItemTemplateId = Resource.Layout.nearest_item;
            var nearestAdapter = (NearestListAdapter)nearestList.Adapter;
            nearestAdapter.DeleteRequested.Select(async notification => await this.ViewModel.DeleteNearest(notification)).Subscribe();

            overdueList = view.FindViewById<MvxLinearLayout>(Resource.Id.overdueList);
            overdueList.ItemTemplateId = Resource.Layout.overdue_item;
            var overdueAdapter = (OverdueListAdapter)overdueList.Adapter;
            overdueAdapter.DeleteRequested.Select(async medication => await this.ViewModel.DeleteOverdue(medication)).Subscribe();

            laterList = view.FindViewById<MvxLinearLayout>(Resource.Id.laterList);
            laterList.ItemTemplateId = Resource.Layout.later_item;
            
            //Toolbar will now take on default actionbar characteristics
           
            SetBinding();
            return view;
        }

        private void SetBinding()
        {
            var bindingSet = this.CreateBindingSet<RegistrationView,RegistrationViewModel>();

            bindingSet.Bind(overdueList)
                .For(x => x.ItemsSource)
                .To(vm => vm.OverdueList);

            bindingSet.Bind(nearestList)
                .For(x => x.ItemsSource)
                .To(vm => vm.NearestList);

            bindingSet.Bind(laterList)
                .For(x => x.ItemsSource)
                .To(vm => vm.LaterList);

            bindingSet.Apply();
        }

        public override void OnResume()
        {
            base.OnResume();
            Task.Run(()=>
                this.ViewModel.Init());
        }
    }
}
