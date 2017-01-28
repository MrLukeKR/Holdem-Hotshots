using System.Threading.Tasks;
using Foundation;
using UIKit;
using Urho;

namespace HoldemHotshots.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			LaunchGame();

			return true;
		}

		async void LaunchGame()
		{
			await Task.Yield();
			new HoldemHotshots(new ApplicationOptions("Data")).Run();
		}

		public override void OnResignActivation(UIApplication application) { }
		public override void DidEnterBackground(UIApplication application) { }
		public override void WillEnterForeground(UIApplication application) { }
		public override void OnActivated(UIApplication application) { }
		public override void WillTerminate(UIApplication application) { }
	}
}

