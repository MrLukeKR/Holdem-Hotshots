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
        Vector3 initialCameraPos;

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
            initialCameraPos = CameraNode.Position;

            rotateCamera(TargetNode); //Figure out a way of playing this on devices that can handle it

            return menuScene;
        }

        private void LoadPlayingScene()
        {
            var cache = ResourceCache;

            for(uint i = 0; i < 4; i ++)
             UI.Root.GetChild(i).Visible = false;

            UI.Root.GetChild(7).Visible = true;
            UI.Root.GetChild(8).Visible = true;
            UI.Root.GetChild(8).Enabled = true;
                
            //Issues with movement on some devices
            //Jump to position if animation causes issues:
            /* CameraNode.Position = new Vector3(0.00544398f, 0.176587f, 0.159439f);
               CameraNode.Rotation = new Quaternion(60f, -180f, 0f);
            */

            //DO LOADING

            CameraNode.RemoveAllActions();
            panToJoin();
            //Load hosting UI

            // return playingScene;
        }

        private void LoadHostingScene()
        {
            var cache = ResourceCache;
            for (uint i = 0; i < 4; i++)
                UI.Root.GetChild(i).Visible = false;


            UI.Root.GetChild(6).Visible = true;
            UI.Root.GetChild(8).Visible = true;
            UI.Root.GetChild(8).Enabled = true;

            //Issues with movement on some devices
            //Jump to position if animation causes issues:
            /* CameraNode.Position = new Vector3(0.00544398f, 0.176587f, 0.159439f);
               CameraNode.Rotation = new Quaternion(60f, -180f, 0f);
            */
           
            CameraNode.RemoveAllActions();
            panToHost();
            //Load hosting UI

            // return playingScene;
        }

        private void BackButton_Pressed(PressedEventArgs obj)
        {
            for (uint i = 0; i < 4; i++)
                UI.Root.GetChild(i).Visible = true;


            UI.Root.GetChild(6).Visible = false;
            UI.Root.GetChild(7).Visible = false;
            UI.Root.GetChild(8).Visible = false;
            UI.Root.GetChild(8).Enabled = false;

            panToOriginalPosition();
            rotateCamera(TargetNode);
        }

        private void panToOriginalPosition()
        {
            CameraNode.RunActions(
                 new Parallel(
                     new MoveTo(1,initialCameraPos), new RotateTo(1, 20f,0f,0f)
                 )
             );
            CameraNode.LookAt(TargetNode.Position, Vector3.Up, TransformSpace.World);
        }

        private void panToHost()
        {
            CameraNode.RunActions(
                 new Parallel(
                     new MoveTo(1, new Vector3(0.00544398f, 0.176587f, 0.159439f)), new RotateTo(1, 60f, -180f, 0f)
                 )
             );
        }
        
        private void panToJoin()
        {
            CameraNode.RunActions(
               new Parallel(
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
            var infoButton = new Button();

            copyrightNotice.Value = "Copyright © Advantage Software Group 2016. All Rights Reserved.";
            copyrightNotice.HorizontalAlignment = HorizontalAlignment.Center;
            copyrightNotice.VerticalAlignment = VerticalAlignment.Bottom;
            copyrightNotice.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            copyrightNotice.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);

            gameTitle.Texture = cache.GetTexture2D("Textures/gameTitle.png");
            gameTitle.BlendMode = BlendMode.Replace;
            gameTitle.SetSize((Graphics.Width/5) * 4, (Graphics.Width / 5) * 2);
            gameTitle.SetPosition((Graphics.Width / 2) - (gameTitle.Width / 2), Graphics.Height / 8);

            settingsButton.Texture = cache.GetTexture2D("Textures/settingsButton.png"); // Set texture
            settingsButton.BlendMode = BlendMode.Replace;
            settingsButton.SetSize(50, 50);
            settingsButton.SetPosition(Graphics.Width - settingsButton.Width - 20, 20);
            settingsButton.Name = "Settings";
            settingsButton.Pressed += SettingsButton_Pressed;

            infoButton.Texture = cache.GetTexture2D("Textures/infoButton.png"); // Set texture
            infoButton.BlendMode = BlendMode.Replace;
            infoButton.SetSize(50, 50);
            infoButton.SetPosition(Graphics.Width - infoButton.Width - 20, Graphics.Height - infoButton.Height - 20);
            infoButton.Name = "About";
            infoButton.Pressed += InfoButton_Pressed; ;

            joinButton.Texture = cache.GetTexture2D("Textures/joinGameButton.png"); // Set texture
            joinButton.BlendMode = BlendMode.Replace;
            joinButton.SetSize(Graphics.Width / 3, (Graphics.Width / 4) / 2);
            joinButton.SetPosition(((Graphics.Width - joinButton.Width) / 5), (Graphics.Height / 4) * 3);
            joinButton.Name = "JoinGame";
            joinButton.Pressed += JoinButton_Pressed;

            hostButton.Texture = cache.GetTexture2D("Textures/hostGameButton.png"); // Set texture
            hostButton.BlendMode = BlendMode.Replace;
            hostButton.SetSize(Graphics.Width / 3, (Graphics.Width / 4) / 2);
            hostButton.SetPosition(((Graphics.Width - hostButton.Width) / 5) * 4, (Graphics.Height / 4) * 3);
            hostButton.Name = "HostGame";
            hostButton.Pressed += HostButton_Pressed;


            Text hostText = new Text()
            {
                Value = "HOST MENU WILL GO HERE",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };


            hostText.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            hostText.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);
            hostText.Visible = false;

            Text joinText = new Text()
            {
                Value = "JOIN MENU WILL GO HERE",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            joinText.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            joinText.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);
            joinText.Visible = false;

            Button backButton = new Button();
            backButton.Texture = cache.GetTexture2D("Textures/backButton.png"); // Set texture
            backButton.BlendMode = BlendMode.Add;
            backButton.SetSize(50, 50);
            backButton.SetPosition(20, 20);
            backButton.Name = "Back";
            backButton.Pressed += BackButton_Pressed;

            backButton.Visible = false;
            backButton.Enabled = false;

            backButton.SetStyleAuto(null);
            settingsButton.SetStyleAuto(null);
            joinButton.SetStyleAuto(null);
            hostButton.SetStyleAuto(null);

            UI.Root.AddChild(gameTitle);        //Index = 0
            UI.Root.AddChild(copyrightNotice);  //Index = 1
            UI.Root.AddChild(joinButton);       //Index = 2
            UI.Root.AddChild(hostButton);       //Index = 3
            UI.Root.AddChild(settingsButton);   //Index = 4
            UI.Root.AddChild(infoButton);        //Index = 5

            UI.Root.AddChild(hostText);         //Index = 6
            UI.Root.AddChild(joinText);         //Index = 7

            UI.Root.AddChild(backButton);       //Index = 8
        }

        private void InfoButton_Pressed(PressedEventArgs obj)
        {
            var cache = ResourceCache;

            var window = new Window()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var close = new Button();
            close.CreateButton("Close");

            window.SetSize((Graphics.Width / 3) * 2, Graphics.Height / 3);

            var title = new Text()
            {
                Value = "About",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top     
            };

            var scroller = new ScrollView()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            scroller.SetSize(window.Width, window.Height / 2);
            
            var about = new Text()
            {
                Value = "GAME NAME GOES HERE\nVersion 0.0.5\n\nA Mixed Reality Texas Hold 'em Game\nby\nAdvantage Software Group\n\nAuthors\nLuke Rose, Jack Nicholson, Xinyi Li, Michael Uzoka, George Thomas, Rick Jin\n",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                TextAlignment = HorizontalAlignment.Center
            };

            about.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);
            about.SetColor(Color.Black);
            about.SetSize(window.Width, (window.Height / 4) * 3);
            
            title.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
            title.SetPosition(0, 20);
            title.SetColor(Color.Black);

            scroller.AddChild(about);
            scroller.UseDerivedOpacity = true;
            
            window.AddChild(title);
            window.AddChild(scroller);
            window.AddChild(close);
            window.Opacity = 0.5f;

            
            UI.Root.AddChild(window);
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