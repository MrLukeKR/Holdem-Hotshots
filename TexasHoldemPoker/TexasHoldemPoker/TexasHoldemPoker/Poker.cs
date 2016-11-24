using Urho;
using Urho.Actions;
using Urho.Audio;
using Urho.Gui;

namespace TexasHoldemPoker
{
    class Poker : Urho.Application
    {
        public Poker() : base(new ApplicationOptions(assetsFolder: "Data")) { }
        
        Scene scene;
        Camera camera;
        Node CameraNode;
        Node TargetNode;

        protected override void Start()
        {
            base.Start();
            scene = LoadMenuScene();
            LoadMenuUI();
            SetupViewport();
        }

        private Scene LoadMenuScene()
        {
            var cache = ResourceCache;
            Scene menuScene = new Scene();

            menuScene.LoadXmlFromCache(cache, "Scenes/Menu.xml");

            //Manually creating scene:
            /*menuScene.CreateComponent<Octree>();
            var cameraNode = scene.CreateChild();
            cameraNode.Position = new Vector3(0.0f, 0.160347f, -0.247524f); //TODO: Make these a bit neater
            cameraNode.CreateComponent<Camera>();
            Viewport = new Viewport(Context, menuScene, cameraNode.GetComponent<Camera>(), null);

            Renderer.SetViewport(0, Viewport);

            Node pokerTable;
            */

            var music = cache.GetSound("Music/MenuBGM.wav");
            music.Looped = true;
            Node musicNode = menuScene.CreateChild("Music");
            SoundSource musicSource = musicNode.CreateComponent<SoundSource>();
            musicSource.SetSoundType(SoundType.Music.ToString());
            musicSource.Play(music);

            CameraNode = menuScene.GetChild("MainCamera", true);
            TargetNode = menuScene.GetChild("PokerTable", true);
            
            camera = CameraNode.GetComponent<Camera>();


            rotateCamera(TargetNode); //Figure out a way of playing this on devices that can handle it

            return menuScene;
        }

        private void LoadPlayingScene()
        {
            var cache = ResourceCache;

            UI.Root.RemoveAllChildren();
            UI.Clear();

            CameraNode.RemoveAllActions();
            panToJoin(); //Issues with movement on some devices

            //Jump to position if animation causes issues:
            /* CameraNode.Position = new Vector3(0.00544398f, 0.176587f, 0.159439f);
               CameraNode.Rotation = new Quaternion(60f, -180f, 0f);
            */

            Text temptext = new Text()
            {
                Value = "JOIN MENU WILL GO HERE",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };


            temptext.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            temptext.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);

            Button backButton = new Button();
            backButton.Texture = cache.GetTexture2D("Textures/backButton.png"); // Set texture
            backButton.BlendMode = BlendMode.Add;
            backButton.SetSize(100, 100);
            backButton.SetPosition(0, 0);
            backButton.Name = "Back";
            backButton.Opacity = 0.8f;
            backButton.CreateButton();
            backButton.Pressed += BackButton_Pressed;

            temptext.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            temptext.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);


            UI.Root.AddChild(temptext);
            UI.Root.AddChild(backButton);
            //Load hosting UI

            // return playingScene;
        }

        private void LoadHostingScene()
        {
            var cache = ResourceCache;

            UI.Root.RemoveAllChildren();
            UI.Clear();

            CameraNode.RemoveAllActions();
            panToHost(); //Issues with movement on some devices

            //Jump to position if animation causes issues:
            /* CameraNode.Position = new Vector3(0.00544398f, 0.176587f, 0.159439f);
               CameraNode.Rotation = new Quaternion(60f, -180f, 0f);
            */

            Text temptext = new Text()
            {
                Value = "HOST MENU WILL GO HERE",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };


            Button backButton = new Button();
            backButton.Texture = cache.GetTexture2D("Textures/backButton.png"); // Set texture
            backButton.BlendMode = BlendMode.Add;
            backButton.SetSize(100, 100);
            backButton.SetPosition(0,0);
            backButton.Name = "Back";
            backButton.Opacity = 0.8f;
            backButton.CreateButton();
            backButton.Pressed += BackButton_Pressed;

            temptext.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            temptext.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);


            UI.Root.AddChild(temptext);
            UI.Root.AddChild(backButton);

            //Scene hostingScene = new Scene();
            //hostingScene.LoadXmlFromCache(cache, "Scenes/Playing.xml");

          //  return hostingScene;
        }

        private void BackButton_Pressed(PressedEventArgs obj)
        {

            UI.Root.RemoveAllChildren();
            UI.Clear();
       
            panToOriginalPosition();
            rotateCamera(TargetNode);
            LoadMenuUI();
        }

        private async void panToOriginalPosition()
        {
            await CameraNode.RunActionsAsync(
                 new Sequence(
                     new MoveTo(1, new Vector3(0.00821081f, 0.160347f, -0.247524f)), new RotateTo(1, 20f, 0f, 0f)
                 )
             );
        }

        private async void panToHost()
        {
            await CameraNode.RunActionsAsync(
                 new Sequence(
                     new MoveTo(1, new Vector3(0.00544398f, 0.176587f, 0.159439f)), new RotateTo(1, 60f, -180f, 0f)
                 )
             );
        }

        private async void panToJoin()
        {
            await CameraNode.RunActionsAsync(
               new Sequence(
                   new MoveTo(1, new Vector3(0f, 0.106208f, -0.139909f)), new RotateTo(1, 20f, 0f, 0f)
               )
           );   
        }

        private async void rotateCamera(Node target)
        {
            await CameraNode.RunActionsAsync(
             new RepeatForever(
                 new RotateAroundBy(60, TargetNode.Position, 0.0f, 360.0f, 0.0f, TransformSpace.World)
             )
            );
        }

        private void  LoadMenuUI()
        {
            var cache = ResourceCache;
            var copyrightNotice = new Text();
            var gameTitle = new BorderImage();
            var settingsButton = new Button();
            var joinButton = new Button();
            var hostButton = new Button();


            copyrightNotice.Value = "Copyright © Advantage Software Group 2016. All Rights Reserved.";
            copyrightNotice.HorizontalAlignment = HorizontalAlignment.Center;
            copyrightNotice.VerticalAlignment = VerticalAlignment.Bottom;
            copyrightNotice.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            copyrightNotice.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);

            gameTitle.Texture = cache.GetTexture2D("Textures/gameTitle.png");
            gameTitle.BlendMode = BlendMode.Add;
            gameTitle.SetSize((Graphics.Width/5) * 4, (Graphics.Width / 5) * 2);
            gameTitle.SetPosition((Graphics.Width / 2) - (gameTitle.Width / 2), Graphics.Height / 8);

            settingsButton.Texture = cache.GetTexture2D("Textures/settingsButton.png"); // Set texture
            settingsButton.BlendMode = BlendMode.Add;
            settingsButton.SetSize(25, 25);
            settingsButton.SetPosition(Graphics.Width - settingsButton.Width - 20, 20);
            settingsButton.Name = "Settings";
            settingsButton.Opacity = 0.6f;
            settingsButton.Pressed += SettingsButton_Pressed;

            joinButton.Texture = cache.GetTexture2D("Textures/joinGameButton.png"); // Set texture
            joinButton.BlendMode = BlendMode.Add;
            joinButton.SetSize(Graphics.Width / 3, (Graphics.Width / 4) / 2);
            joinButton.SetPosition(((Graphics.Width - joinButton.Width) / 5), (Graphics.Height / 4) * 3);
            joinButton.Name = "JoinGame";
            joinButton.Pressed += JoinButton_Pressed;

            hostButton.Texture = cache.GetTexture2D("Textures/hostGameButton.png"); // Set texture
            hostButton.BlendMode = BlendMode.Add;
            hostButton.SetSize(Graphics.Width / 3, (Graphics.Width / 4) / 2);
            hostButton.SetPosition(((Graphics.Width - hostButton.Width) / 5) * 4, (Graphics.Height / 4) * 3);
            hostButton.Name = "HostGame";
            hostButton.Pressed += HostButton_Pressed;

            settingsButton.SetStyleAuto(null);
            joinButton.SetStyleAuto(null);
            hostButton.SetStyleAuto(null);

            UI.Root.AddChild(gameTitle);
            UI.Root.AddChild(settingsButton);
            UI.Root.AddChild(joinButton);
            UI.Root.AddChild(hostButton);
            UI.Root.AddChild(copyrightNotice);
        }

        private void SetupViewport()
        {
            Renderer.SetViewport(0, new Viewport(Context, scene, camera, null));
        }

        private void HostButton_Pressed(PressedEventArgs obj)
        {
            //Do host game stuff
            //TODO: Add intermediate host connection handling and setup

            //Load Hosting Scene
            LoadHostingScene();
        }

        private void JoinButton_Pressed(PressedEventArgs obj)
        {
            //Do join game stuff
            //TODO: Add intermediate join connection handling and setup

            //Load Playing Scene
            LoadPlayingScene();
        }

        private void SettingsButton_Pressed(PressedEventArgs obj)
        {
            //Do settings stuff
            
        }
    }
}