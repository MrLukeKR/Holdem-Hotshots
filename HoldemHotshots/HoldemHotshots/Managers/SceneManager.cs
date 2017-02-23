using System;
using Urho;
using Urho.Audio;
using Urho.Resources;

namespace HoldemHotshots
{
	public static class SceneManager
	{
		static public ResourceCache cache;

		//Scenes
		static public Scene menuScene { get; private set; }
		static public Scene playScene { get; private set; }
		static public Scene hostScene { get; private set; }
		static public Viewport Viewport { get; private set; }
		static public Context context { get; private set; }
		static public Renderer renderer { get; private set; }

		public static void SetReferences(ResourceCache resCache, Context currContext, Renderer currRenderer) { cache = resCache; context = currContext; renderer = currRenderer; }

		public static void CreateMenuScene()
		{
			if (menuScene == null)
			{
                menuScene = new Scene();
                menuScene.CreateComponent<Octree>();

                var cameraNode = menuScene.CreateChild();
                cameraNode.Name = "MainCamera";
                cameraNode.Position = (new Vector3(0.0f, 0.0f, -10.0f));
                cameraNode.CreateComponent<Camera>();
			}
		}

		public static void CreatePlayScene()
		{
            //Don't check for null scene here, as the scene is refereshed on each game to reset the dealt cards
            if (playScene != null) playScene.Dispose();
           
                playScene = new Scene();
   
                playScene.CreateComponent<Octree>();

				var cameraNode = playScene.CreateChild();
				cameraNode.Name = "MainCamera";
				cameraNode.Position = (new Vector3(0.0f, 0.0f, -10.0f));
				cameraNode.CreateComponent<Camera>();
            
            Node soundNode = playScene.CreateChild("SFX");
            SoundSource musicSource = soundNode.CreateComponent<SoundSource>();
            musicSource.SetSoundType(SoundType.Effect.ToString());

            var lightNode = playScene.CreateChild();
                lightNode.Name = "MainLight";
                lightNode.Position = (new Vector3(0.0f, 0.0f, -10.0f));
                var light = lightNode.CreateComponent<Light>();
                light.LightType = LightType.Directional;
		}

		public static void CreateHostScene()
		{
            if (hostScene != null) hostScene.Dispose();

                hostScene = new Scene();
				hostScene.CreateComponent<Octree>();

				var cameraNode = hostScene.CreateChild();
				cameraNode.Name = "MainCamera";
				cameraNode.Position = (new Vector3(0.0f, 0.0f, -10.0f));
				cameraNode.CreateComponent<Camera>();
            
                Node soundNode = hostScene.CreateChild("SFX");
                SoundSource musicSource = soundNode.CreateComponent<SoundSource>();
                musicSource.SetSoundType(SoundType.Effect.ToString());
            
                var lightNode = hostScene.CreateChild();
                lightNode.Name = "MainLight";
                lightNode.Position = (new Vector3(0.0f, 0.0f, -10.0f));
                var light = lightNode.CreateComponent<Light>();
                light.LightType = LightType.Directional;
		}
        
        public static void ShowScene(Scene scene)
		{
            var cameraNode = scene.GetChild("MainCamera", true);

            if(Viewport!=null) Viewport.Dispose(); 

			Viewport = new Viewport(context, scene, cameraNode.GetComponent<Camera>(), null);
            Viewport.SetClearColor(new Color(0.0f, 0.4f, 0.0f, 1.0f));

			SetupRenderer();
		}

		private static void SetupRenderer() { renderer.SetViewport(0, Viewport); }
	}
}
