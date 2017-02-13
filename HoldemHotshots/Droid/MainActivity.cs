using Android.App;
using Android.Widget;
using Android.OS;
using Urho.Droid;
using ZXing.Mobile;

namespace HoldemHotshots.Droid
{
	[Activity(Label = "Hold'em Hotshots", MainLauncher = true, Icon = "@drawable/icon", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.Window.RequestFeature(Android.Views.WindowFeatures.ActionBar);
			base.SetTheme(global::Android.Resource.Style.ThemeNoTitleBarFullScreen);
			base.OnCreate(savedInstanceState);

			MobileBarcodeScanner.Initialize(Application);

			//TODO: Find a better way to deal with this
#pragma warning disable CS0618 // Type or member is obsolete
			var mLayout = new AbsoluteLayout(this);
#pragma warning restore CS0618 // Type or member is obsolete

			var surface = UrhoSurface.CreateSurface<HoldemHotshots>(this, new Urho.ApplicationOptions("Data"));
			mLayout.AddView(surface);
			SetContentView(mLayout);
		}

        protected override void OnResume()
        {
            UrhoSurface.OnResume();
            base.OnResume();
        }

        protected override void OnPause()
        {
            UrhoSurface.OnPause();
            base.OnPause();
        }

        public override void OnLowMemory()
        {
            UrhoSurface.OnLowMemory();
            base.OnLowMemory();
        }

        protected override void OnDestroy()
        {
            UrhoSurface.OnDestroy();
            base.OnDestroy();
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            UrhoSurface.OnWindowFocusChanged(hasFocus);
            base.OnWindowFocusChanged(hasFocus);
        }

        public void SetLandscape()
        {
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
        }

        public void SetPortrait()
        {
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
        }
    }
}