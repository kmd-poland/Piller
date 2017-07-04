using Android.App;
using Android.OS;
using MvvmCross.Droid.Support.V7.AppCompat;
using Piller.Resources;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Piller.Droid.Views
{
    [Activity(Label = "About")]
    public class About : MvxAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.About);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = AppResources.About_Title;
        }
    }
}