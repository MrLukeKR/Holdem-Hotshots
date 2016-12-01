using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Audio;
using Urho.Gui;
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

        public static void LoadJoiningScene(Node CameraNode, UI ui)
        {
            for (uint i = 0; i < 4; i++)
                ui.Root.GetChild(i).Visible = false;


            ui.Root.GetChild(5).Visible = false;
            ui.Root.GetChild(5).Enabled = false;
            ui.Root.GetChild(7).Visible = true;
            ui.Root.GetChild(8).Visible = true;
            ui.Root.GetChild(8).Enabled = true;
            ui.Root.GetChild(9).Visible = false;
            ui.Root.GetChild(9).Enabled = false;

            ui.Root.GetChild("JoinLobby", true).Visible = true;
            ui.Root.GetChild("JoinLobby", true).Enabled = true;

            //Issues with movement on some devices
            //Jump to position if animation causes issues:
            /* CameraNode.Position = new Vector3(0.00544398f, 0.176587f, 0.159439f);
               CameraNode.Rotation = new Quaternion(60f, -180f, 0f);
            */

            //DO LOADING

            CameraHandler.panToJoin(CameraNode);
            //Load hosting UI


            // return playingScene;
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

        public static void LoadHostingScene(Node CameraNode, UI ui)
        {
            for (uint i = 0; i < 4; i++)
                ui.Root.GetChild(i).Visible = false;


            ui.Root.GetChild(5).Visible = false;
            ui.Root.GetChild(5).Enabled = false;
            ui.Root.GetChild(6).Visible = true;
            ui.Root.GetChild(8).Visible = true;
            ui.Root.GetChild(8).Enabled = true;
            ui.Root.GetChild(9).Visible = false;
            ui.Root.GetChild(9).Enabled = false;


            ui.Root.GetChild("CreateLobby", true).Visible = true;
            ui.Root.GetChild("CreateLobby", true).Enabled = true;

            //Issues with movement on some devices
            //Jump to position if animation causes issues:
            /* CameraNode.Position = new Vector3(0.00544398f, 0.176587f, 0.159439f);
               CameraNode.Rotation = new Quaternion(60f, -180f, 0f);
            */

            CameraNode.RemoveAllActions();
            CameraHandler.panToHost(CameraNode);
            //Load hosting UI

            // return playingScene;
        }
    }
}
