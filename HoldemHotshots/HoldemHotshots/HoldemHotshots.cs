using System.Collections.Generic;
using System.Diagnostics;
using Urho;
using Urho.Audio;
using Urho.Gui;

namespace HoldemHotshots
{
	public class HoldemHotshots : Application
	{
		[Preserve]
		public HoldemHotshots() : base(new ApplicationOptions(assetsFolder: "Data") { Height = 1024, Width = 576, Orientation = ApplicationOptions.OrientationType.Portrait }) { }

		[Preserve]
		public HoldemHotshots(ApplicationOptions opts) : base(opts) { }

		static HoldemHotshots()
		{
			UnhandledException += (s, e) =>
			{
				if (Debugger.IsAttached)
					Debugger.Break();
				e.Handled = true;
			};
		}

		protected override void Start()
		{
			base.Start();
			InitControllers();

			SceneManager.CreateMenuScene();
			UIManager.CreateMenuUI();

			SceneManager.ShowScene(SceneManager.menuScene);
			UIUtils.ShowUI(UIManager.menuUI);
		}

		private void InitControllers()
		{
			SceneManager.SetReferences(ResourceCache, Context, Renderer);
			UIManager.SetReferences(ResourceCache, Graphics, UI);
			PositionUtils.SetReferences(Graphics, UI);
		}
	}
}
