using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace TexasHoldemPoker.Droid
{
	[Activity (Label = "Mixed Reality Poker", Theme ="@style/splashscreen", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
            base.Window.RequestFeature(WindowFeatures.ActionBar);
            base.SetTheme(global::Android.Resource.Style.ThemeHolo);
            base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication(new App());
        //    this.Window.AddFlags(WindowManagerFlags.Fullscreen);
        }
    }
}

