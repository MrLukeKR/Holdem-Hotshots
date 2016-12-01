using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Audio;
using Urho.Resources;

namespace TexasHoldemPoker.Game
{
    static class SceneGenerator
    {
        public static Scene LoadMenuScene(ResourceCache cache)
        {
            Scene menuScene = new Scene();

            menuScene.LoadXmlFromCache(cache, "Scenes/Menu.xml");

            var music = cache.GetSound("Music/MenuBGM.wav");
            music.Looped = true;
            Node musicNode = menuScene.CreateChild("Music");
            SoundSource musicSource = musicNode.CreateComponent<SoundSource>();
            musicSource.SetSoundType(SoundType.Music.ToString());
            musicSource.Play(music);
            
            return menuScene;
        }

        public static Scene CreateTableScene(ResourceCache cache)
        {
            Scene tableScene = new Scene();

            tableScene.LoadXmlFromCache(cache, "Scenes/Table.xml");

            var music = cache.GetSound("Music/TableBGM.wav");
            music.Looped = true;
            Node musicNode = tableScene.CreateChild("Music");
            SoundSource musicSource = musicNode.CreateComponent<SoundSource>();
            musicSource.SetSoundType(SoundType.Music.ToString());
            musicSource.Play(music);

            return tableScene;
        }
    }
}
