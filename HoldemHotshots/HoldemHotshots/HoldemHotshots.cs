using System.Collections.Generic;
using System.Diagnostics;
using Urho;
using Urho.Audio;
using Urho.Gui;

namespace HoldemHotshots
{
	public class HoldemHotshots : Application
	{



		public Viewport Viewport { get; private set; }

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
			SceneManager.SetCache(ResourceCache);
			UIManager.SetReferences(ResourceCache, Graphics, UI);

			SceneManager.Init();
			UIManager.CreateMenuUI();

			ShowScene(SceneManager.menuScene);
			UIUtils.ShowUI(UIManager.menuUI);
		}

		//Scene Management

		private void ShowScene(Scene scene)
		{
			var cameraNode = scene.GetChild("MainCamera", true);

			Viewport = new Viewport(Context, scene, cameraNode.GetComponent<Camera>(), null);
			SetupRenderer();
		}

		private void SetupRenderer() { Renderer.SetViewport(0, Viewport); }


	}
}
