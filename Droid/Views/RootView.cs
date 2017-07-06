
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
using Android.Content.PM;
using Android.Support.Design.Internal;

namespace Piller.Droid.Views
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.KeyboardHidden)]
    public class RootView : MvxCachingFragmentCompatActivity<RootViewModel>
    {
        private BottomNavigationView bottomNavigation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MainLayout);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            this.bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            this.bottomNavigation.NavigationItemSelected += (object sender, BottomNavigationView.NavigationItemSelectedEventArgs e) => {

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

			this.ViewModel.ChangeTab.RegisterHandler(context =>
			{
                var menuId = -1; 
                switch (context.Input)
				{
					case 0:
                        menuId = Resource.Id.upcoming;
						break;
					case 1:
						menuId = Resource.Id.medical_card;
						break;
					case 2:
						menuId = Resource.Id.holidays;
						break;
					default:
						break;
				}

                if (menuId != -1)
                {
                    this.RunOnUiThread(() =>
                    {
                        this.bottomNavigation.FindViewById(menuId)?.PerformClick();
                        this.bottomNavigation.Invalidate();
                    });
                }
			});

            this.ViewModel.ShowInitialView.Execute().Subscribe();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
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

            if (item.ItemId == Resource.Id.action_About)
            {
                var intent = new Intent(this, typeof(About));
				StartActivity(intent);

                return true;
            }


            return base.OnOptionsItemSelected(item);
        }

        public override void OnBackPressed()
        {
            // do nothing
        }
    }
}
