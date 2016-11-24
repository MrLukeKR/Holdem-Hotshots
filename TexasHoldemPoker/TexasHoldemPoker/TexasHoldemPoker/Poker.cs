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

            var music = cache.GetSound("Music/MenuBGM.wav");
            music.Looped = true;
            Node musicNode = menuScene.CreateChild("Music");
            SoundSource musicSource = musicNode.CreateComponent<SoundSource>();
            musicSource.SetSoundType(SoundType.Music.ToString());
            musicSource.Play(music);

            CameraNode = menuScene.GetChild("MainCamera", true);
            TargetNode = menuScene.GetChild("PokerTable", true);

            camera = CameraNode.GetComponent<Camera>();
        //    rotateCamera(TargetNode); //Figure out a way of playing this on devices that can handle it

            return menuScene;
        }

        private Scene LoadPlayingScene()
        {
            var cache = ResourceCache;
            Scene playingScene = new Scene();

            playingScene.LoadXmlFromCache(cache, "Scenes/Hosting.xml");

            return playingScene;
        }

        private Scene LoadHostingScene()
        {
            var cache = ResourceCache;
            Scene hostingScene = new Scene();
            hostingScene.LoadXmlFromCache(cache, "Scenes/Playing.xml");

            return hostingScene;
        }

        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);
        }

        private async void rotateCamera(Node target)
        {
            await CameraNode.RunActionsAsync(
             new RepeatForever(
                 new RotateAroundBy(60, TargetNode.Position, 0.0f, 360.0f, 0.0f, TransformSpace.World)
             )
            );
        }

        private void LoadMenuUI()
        {
            var cache = ResourceCache;
            var copyrightNotice = new Text()
            {
                Value = "Copyright © Advantage Software Group 2016. All Rights Reserved.",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom
            };

            copyrightNotice.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            copyrightNotice.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);

            var gameTitle = new BorderImage();
            gameTitle.Texture = cache.GetTexture2D("Textures/gameTitle.png");
            gameTitle.BlendMode = BlendMode.Add;
            gameTitle.SetSize((Graphics.Width/5) * 4, (Graphics.Width / 5) * 2);
            gameTitle.SetPosition((Graphics.Width / 2) - (gameTitle.Width / 2), Graphics.Height / 8);

            var settingsButton = new Button();
            settingsButton.Texture = cache.GetTexture2D("Textures/settingsButton.png"); // Set texture
            settingsButton.BlendMode = BlendMode.Add;
            settingsButton.SetSize(25, 25);
            settingsButton.SetPosition(Graphics.Width - settingsButton.Width - 20, 20);
            settingsButton.Name = "Settings";
            settingsButton.Opacity = 0.6f;
            settingsButton.CreateButton();
            settingsButton.Pressed += SettingsButton_Pressed;

            var joinButton = new Button();
            joinButton.Texture = cache.GetTexture2D("Textures/joinGameButton.png"); // Set texture
            joinButton.BlendMode = BlendMode.Add;
            joinButton.SetSize(Graphics.Width / 3, (Graphics.Width / 4) / 2);
            joinButton.SetPosition(((Graphics.Width - joinButton.Width) / 5), (Graphics.Height / 4) * 3);
            joinButton.Name = "JoinGame";
            joinButton.Pressed += JoinButton_Pressed;

            var hostButton = new Button();
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
            UI.Clear();
            //TODO: Add intermediate host connection handling and setup

            //Load Hosting Scene
            scene.Clear(true, true);
            LoadHostScene();
        }

        private void JoinButton_Pressed(PressedEventArgs obj)
        {
            //Do join game stuff
            UI.Clear();
            //TODO: Add intermediate join connection handling and setup

            //Load Playing Scene
            scene.Clear(true, true);
            LoadPlayScene();
        }

        private void SettingsButton_Pressed(PressedEventArgs obj)
        {
            //Do settings stuff
            
        }
    }
}