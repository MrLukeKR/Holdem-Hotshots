using Urho;
using Urho.Audio;
using Urho.Gui;
using Urho.IO;
using Urho.Resources;
using Urho.Urho2D;

namespace TexasHoldemPoker
{
    class Poker : Urho.Application
    {
        public Poker() : base(new ApplicationOptions(assetsFolder: "Data")) { }

        Scene scene;
        Camera camera;

        protected override void Start()
        {
            base.Start();
            scene = CreateScene();
            CreateUI();
            SetupViewport();
        }

        private Scene CreateScene()
        {
            
            var cache = ResourceCache;
            Scene menuScene = new Scene();
            menuScene.LoadXmlFromCache(cache, "Scenes/Menu.xml");

            return menuScene;
        }

        private void CreateUI()
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
            gameTitle.SetSize(400, 200);
            gameTitle.SetPosition((Graphics.Width / 2) - (gameTitle.Width / 2), gameTitle.Height / 2);

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
            joinButton.SetSize(150, 75);
            joinButton.SetPosition(((Graphics.Width - joinButton.Width) / 5), Graphics.Height - 150);
            joinButton.Name = "JoinGame";
            joinButton.Pressed += JoinButton_Pressed;

            var hostButton = new Button();
            hostButton.Texture = cache.GetTexture2D("Textures/hostGameButton.png"); // Set texture
            hostButton.BlendMode = BlendMode.Add;
            hostButton.SetSize(150, 75);
            hostButton.SetPosition(((Graphics.Width - hostButton.Width) / 5) * 4, Graphics.Height - 150);
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
            var renderer = Renderer;
            renderer.SetViewport(0, new Viewport(Context, scene, camera, null));
        }

        private void HostButton_Pressed(PressedEventArgs obj)
        {
            //Do host game stuff


            //TEMPORARY - DEMONSTRATION PURPOSES ONLY!!!
            var cache = ResourceCache;
            var info = new Text()
            {
                Value = "Pressed Host",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
            };

            info.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            info.SetFont(cache.GetFont("Fonts/arial.ttf"), 15);


            this.UI.Root.AddChild(info);
            //^^^^^ TEMPORARY - DELETE THIS IF YOU WISH
        }

        private void JoinButton_Pressed(PressedEventArgs obj)
        {
            //Do join game stuff


            //TEMPORARY - DEMONSTRATION PURPOSES ONLY!!!
            var cache = ResourceCache;
            var info = new Text()
            {
                Value = "Pressed Join",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            info.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            info.SetFont(cache.GetFont("Fonts/arial.ttf"), 15);


            this.UI.Root.AddChild(info); ;
            //^^^^^ TEMPORARY - DELETE THIS IF YOU WISH
        }

        private void SettingsButton_Pressed(PressedEventArgs obj)
        {
            //Do settings stuff

            //TEMPORARY - DEMONSTRATION PURPOSES ONLY!!!
            var cache = ResourceCache;
            var info = new Text()
            {
                Value = "Pressed Settings",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
            };

            info.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            info.SetFont(cache.GetFont("Fonts/arial.ttf"), 15);


            this.UI.Root.AddChild(info);
            //^^^^^ TEMPORARY - DELETE THIS IF YOU WISH
        }
    }
}