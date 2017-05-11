using HoldemHotshots.GameLogic;
using Urho;
using Urho.Audio;
using Urho.Gui;
using Urho.Resources;

namespace HoldemHotshots.Managers
{
    /// <summary>
    /// Contains the data for all of the scenes in the game as well as suppling some utilities
    /// </summary>
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

        /// <summary>
        /// Sets access references from the main engine
        /// </summary>
        /// <param name="resCache">Resource Cache</param>
        /// <param name="currContext">Application Context</param>
        /// <param name="currRenderer">Display Renderer</param>
        public static void SetReferences(ResourceCache resCache, Context currContext, Renderer currRenderer)
        {
            cache       = resCache;
            context     = currContext;
            renderer    = currRenderer;
        }

        /// <summary>
        /// Creates the menu scene
        /// </summary>
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

        /// <summary>
        /// Creates the play scene
        /// </summary>
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

        /// <summary>
        /// Creates the host scene
        /// </summary>
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
            pot.SetFont(cache.GetFont("Fonts/arial.ttf"), 40);
            potNode.Position = Card.CARD_TABLE_POSITIONS[2];
            potNode.Position+= new Vector3(3, 3, 0);
            potNode.Rotate(new Quaternion(0, 0, -90),TransformSpace.Local);
            pot.TextAlignment = HorizontalAlignment.Center;
            pot.HorizontalAlignment = HorizontalAlignment.Center;

            var messageNode = hostScene.CreateChild("WinnerText");
            var message = messageNode.CreateComponent<Text3D>();
            message.SetFont(cache.GetFont("Fonts/arial.ttf"), 40);
            messageNode.Position = Card.CARD_TABLE_POSITIONS[2];
                messageNode.Position+=new Vector3(3,0,0);
            messageNode.Rotate(new Quaternion(0, 0, -90), TransformSpace.Local);
            message.TextAlignment = HorizontalAlignment.Center;
            message.HorizontalAlignment = HorizontalAlignment.Center;
        }
        
        /// <summary>
        /// Initialises a new viewport and switches to it
        /// </summary>
        /// <param name="scene">Scene to load and display</param>
        public static void ShowScene(Scene scene)
		{
            var cameraNode = scene.GetChild("MainCamera", true);
            
			Viewport = new Viewport(context, scene, cameraNode.GetComponent<Camera>(), null);
            Viewport.SetClearColor(new Color(0.0f, 0.4f, 0.0f, 1.0f));

			SetupRenderer();
		}

        /// <summary>
        /// Sets the renderer to display the current viewport
        /// </summary>
		private static void SetupRenderer()
        {
            renderer.SetViewport(0, Viewport);
        }
	}
}
