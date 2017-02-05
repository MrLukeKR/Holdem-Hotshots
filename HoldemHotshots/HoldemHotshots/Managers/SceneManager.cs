﻿using System;
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

                var music = cache.GetSound("Music/MenuBGM.wav");
				music.Looped = true;

				Node musicNode = menuScene.CreateChild("Music");
				SoundSource musicSource = musicNode.CreateComponent<SoundSource>();
				musicSource.SetSoundType(SoundType.Music.ToString());
				musicSource.Play(music);
			}
		}

		public static void CreatePlayScene()
		{
			if (playScene == null)
			{
				playScene = new Scene();

               
                playScene.CreateComponent<Octree>();

				var cameraNode = playScene.CreateChild();
				cameraNode.Name = "MainCamera";
				cameraNode.Position = (new Vector3(0.0f, 0.0f, -10.0f));
				var camera = cameraNode.CreateComponent<Camera>();

                var lightNode = playScene.CreateChild();
                lightNode.Name = "MainLight";
                lightNode.Position = (new Vector3(0.0f, 0.0f, -10.0f));
                var light = lightNode.CreateComponent<Light>();
                light.LightType = LightType.Directional;
                
            }
		}

		public static void CreateHostScene()
		{
			if (hostScene == null)
			{
				hostScene = new Scene();
				hostScene.CreateComponent<Octree>();

				var cameraNode = hostScene.CreateChild();
				cameraNode.Name = "MainCamera";
				cameraNode.Position = (new Vector3(0.0f, 0.0f, -10.0f));
				cameraNode.CreateComponent<Camera>();

                Node musicNode = hostScene.CreateChild("SFX");
                SoundSource musicSource = musicNode.CreateComponent<SoundSource>();
                musicSource.SetSoundType(SoundType.Effect.ToString());

                var camera = cameraNode.CreateComponent<Camera>();

                PositionUtils.InitTableCardPositions(camera);
            }
		}

        public static void StopMusic(Scene scene)
        {
            var musicNode = scene.GetChild("Music", true);
            
            if (musicNode != null)
            {
                var soundSource = musicNode.GetComponent<SoundSource>();
                soundSource.Stop();
            }
        }

        public static void ShowScene(Scene scene)
		{
            var cameraNode = scene.GetChild("MainCamera", true);

            if(Viewport!=null) Viewport.Dispose(); 

			Viewport = new Viewport(context, scene, cameraNode.GetComponent<Camera>(), null);

			SetupRenderer();
		}

		private static void SetupRenderer() { renderer.SetViewport(0, Viewport); }
	}
}
