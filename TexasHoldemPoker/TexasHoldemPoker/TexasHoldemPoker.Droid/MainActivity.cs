using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TexasHoldemPoker.Droid
{
	[Activity (Label = "Mixed Reality Poker", Theme = "@style/splashscreen", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
            base.Window.RequestFeature(WindowFeatures.ActionBar);
            base.SetTheme(global::Android.Resource.Style.ThemeHoloLight);


			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (new App());
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
        }
	}
}

