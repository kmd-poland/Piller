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

namespace Piller.Droid.Views
{
    [Activity]
    public class RegistrationView : MvxAppCompatActivity<RegistrationViewModel>
    {
        MvxListView nearestList;
        MvxListView overdueList;
        MvxListView laterList;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.RegistrationView);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            nearestList = FindViewById<MvxListView>(Resource.Id.nearestList);
            nearestList.Adapter = new NearestListAdapter(this, (IMvxAndroidBindingContext)this.BindingContext);
            nearestList.ItemTemplateId = Resource.Layout.nearest_item;
            var nearestAdapter = (NearestListAdapter)nearestList.Adapter;
            nearestAdapter.DeleteRequested.Subscribe(async notification => await this.ViewModel.DeleteNearest(notification));

            overdueList = FindViewById<MvxListView>(Resource.Id.overdueList);
            overdueList.Adapter = new OverdueListAdapter(this, (IMvxAndroidBindingContext)this.BindingContext);
            overdueList.ItemTemplateId = Resource.Layout.overdue_item;
            var overdueAdapter = (OverdueListAdapter)overdueList.Adapter;
            overdueAdapter.DeleteRequested.Subscribe(async medication => await this.ViewModel.DeleteOverdue(medication));

            laterList = FindViewById<MvxListView>(Resource.Id.laterList);
            laterList.Adapter = new LaterListAdapter(this, (IMvxAndroidBindingContext)this.BindingContext);
            laterList.ItemTemplateId = Resource.Layout.later_item;
            
            //Toolbar will now take on default actionbar characteristics
            SetSupportActionBar(toolbar);
            SetBinding();
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
    }
}
