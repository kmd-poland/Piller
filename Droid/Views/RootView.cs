
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Piller.ViewModels;

namespace Piller.Droid.Views
{
    [Activity(Label = "RootView")]
    public class RootView : MvxCachingFragmentCompatActivity<RootViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MainLayout);

            var bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            bottomNavigation.NavigationItemSelected += (object sender, BottomNavigationView.NavigationItemSelectedEventArgs e) => {
                Toast.MakeText(this, "Navigating to " + e.Item.TitleFormatted, ToastLength.Short).Show();

                switch (e.Item.ItemId)
				{
                    case Resource.Id.upcoming:
                        this.ViewModel.ShowUpcomingMedicationView.Execute().Subscribe();
						break;
                    case Resource.Id.medical_card:
                        this.ViewModel.ShowMedicalCardView.Execute().Subscribe();
						break;
                    case Resource.Id.holidays:
                        this.ViewModel.ShowHolidayView.Execute().Subscribe();
						break;
					default:
						Console.WriteLine("That was not meant to happen");
						break;
				}
            };
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
    }
}
