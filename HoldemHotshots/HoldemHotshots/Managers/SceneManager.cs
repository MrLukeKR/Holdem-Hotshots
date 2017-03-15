using HoldemHotshots.GameLogic;
using HoldemHotshots.GameLogic.Player;
using System.Collections.Generic;
using Urho;
using Urho.Audio;
using Urho.Gui;
using Urho.Resources;

namespace HoldemHotshots.Managers
{
	public static class SceneManager
	{
        static private ResourceCache cache;
        static private Viewport Viewport;
        static private Context context;
        static private Renderer renderer;

        //Scenes
        static public Scene menuScene { get; private set; }
		static public Scene playScene { get; private set; }
        static public Scene hostScene { get; private set; }

        public static void SetReferences(ResourceCache resCache, Context currContext, Renderer currRenderer)
        {
            cache       = resCache;
            context     = currContext;
            renderer    = currRenderer;
        }

		public static void CreateMenuScene()
		{
            if (menuScene != null)
                return;
            
            menuScene = new Scene();
            menuScene.CreateComponent<Octree>();

            var cameraNode = menuScene.CreateChild();
            cameraNode.Name = "MainCamera";
            cameraNode.Position = (new Vector3(0.0f, 0.0f, -10.0f));
            cameraNode.CreateComponent<Camera>();
		}

		public static void CreatePlayScene()
		{
            playScene = new Scene();
            playScene.CreateComponent<Octree>();

            var cameraNode = playScene.CreateChild("MainCamera");
            cameraNode.Position = (new Vector3(0.0f, 0.0f, -10.0f));
            cameraNode.CreateComponent<Camera>();
            
            Node soundNode = playScene.CreateChild("SFX");
            SoundSource musicSource = soundNode.CreateComponent<SoundSource>();
            musicSource.SetSoundType(SoundType.Effect.ToString());

            var lightNode = playScene.CreateChild("MainLight");
            lightNode.Position = new Vector3(0.0f, 0.0f, -10.0f);

            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Directional;
		}

		public static void CreateHostScene()
		{
            hostScene = new Scene();
            hostScene.CreateComponent<Octree>();

            var cameraNode = hostScene.CreateChild("MainCamera");
            cameraNode.Position = (new Vector3(0.0f, 0.0f, -10.0f));
			cameraNode.CreateComponent<Camera>();
            
            Node soundNode = hostScene.CreateChild("SFX");
            SoundSource musicSource = soundNode.CreateComponent<SoundSource>();
            musicSource.SetSoundType(SoundType.Effect.ToString());

            var lightNode = hostScene.CreateChild("MainLight");
            lightNode.Position = (new Vector3(0.0f, 0.0f, -10.0f));

            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Directional;

            var potNode = hostScene.CreateChild("PotInfoText");
            var pot = potNode.CreateComponent<Text3D>();
            pot.Text = "Pot\n$0";
            pot.TextAlignment = HorizontalAlignment.Center;
            pot.SetFont(cache.GetFont("Fonts/arial.ttf"), 40);
            potNode.Position = Card.CARD_TABLE_POSITIONS[2];
            potNode.Position += new Vector3(3, 0.4f, 0);
            potNode.Rotate(new Quaternion(0, 0, -90),TransformSpace.Local);
		}
        
        public static void ShowScene(Scene scene)
		{
            var cameraNode = scene.GetChild("MainCamera", true);
            
			Viewport = new Viewport(context, scene, cameraNode.GetComponent<Camera>(), null);
            Viewport.SetClearColor(new Color(0.0f, 0.4f, 0.0f, 1.0f));

			SetupRenderer();
		}

		private static void SetupRenderer()
        {
            renderer.SetViewport(0, Viewport);
        }
	}
}
