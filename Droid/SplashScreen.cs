using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;

namespace Piller.Droid
{
    [Activity(
        Label = "Piller.Droid"
        , MainLauncher = true
        , Icon = "@drawable/pill"
        , Theme = "@style/Theme.Splash"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }
    }
}
