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
		static public Scene menuScene { get; internal set; }
		static public Scene playScene { get; internal set; }
		static public Scene hostScene { get; internal set; }

		public static void SetCache(ResourceCache resCache) { cache = resCache; }

		public static void Init()
		{
			CreateMenuScene();
		}

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
	}
}
