using Android.App;
using Android.OS;
using Android.Widget;
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

			var version = this.FindViewById<TextView>(Resource.Id.version);
			version.Text = $"Wersja oprogramowania: {this.ApplicationContext.PackageManager.GetPackageInfo(this.ApplicationContext.PackageName, 0).VersionName}";
        }
    }
}