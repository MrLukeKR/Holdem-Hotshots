using PokerLogic;
using System;
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

        private Scene LoadPlayerScene()
        {
            var cache = ResourceCache;
            Scene playerScene = new Scene();
           
            playerScene.LoadXmlFromCache(cache, "Scenes/Player.xml");

            CameraNode = playerScene.GetChild("MainCamera", true);  //TODO: Make the camera update when the scene is changed (EVENT)
            camera = CameraNode.GetComponent<Camera>();

            Node card1 = playerScene.GetChild("Card", true);
            Node card2 = playerScene.GetChild("Card2", true);

            //TEMP RANDOM GENERATOR TO SEE DIFFERENT CARD TEXTRUES
            string card1texture = "Textures/Cards/";

            Random rnd = new Random();
            int suitint = rnd.Next(1, 4);

            switch (suitint)
            {
                case 1: card1texture += "S"; break;
                case 2: card1texture += "D"; break;
                case 3: card1texture += "C"; break;
                case 4: card1texture += "H"; break;
            }

            card1texture += rnd.Next(1, 13);
            //

            Button card1button = new Button();
            card1button.Texture = cache.GetTexture2D(card1texture + ".png", true);
            card1button.SetSize(Graphics.Width / 3, (Graphics.Width / 30) * 14);
            card1button.SetPosition(Graphics.Width - card1button.Width , Graphics.Height - card1button.Height);



            //TEMP RANDOM GENERATOR TO SEE DIFFERENT CARD TEXTRUES
            string card2texture = "Textures/Cards/";
            
            suitint = rnd.Next(1, 4);

            switch (suitint)
            {
                case 1: card2texture += "S"; break;
                case 2: card2texture += "D"; break;
                case 3: card2texture += "C"; break;
                case 4: card2texture += "H"; break;
            }

            card2texture += rnd.Next(1, 13);
            //

            Button card2button = new Button();
            card2button.Texture = cache.GetTexture2D(card2texture + ".png", true);
            card2button.SetSize(Graphics.Width / 3, (Graphics.Width / 30) * 14);
            card2button.SetPosition(Graphics.Width - card2button.Width * 2, Graphics.Height - card1button.Height);


            UI.Root.AddChild(card1button);
            UI.Root.AddChild(card2button);

            return playerScene;
        }

        private Scene LoadTableScene()
        {        
            var cache = ResourceCache;
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

        private void LoadJoiningScene()
        {
            var cache = ResourceCache;

            for(uint i = 0; i < 4; i ++)
             UI.Root.GetChild(i).Visible = false;


            UI.Root.GetChild(5).Visible = false;
            UI.Root.GetChild(5).Enabled = false;
            UI.Root.GetChild(7).Visible = true;
            UI.Root.GetChild(8).Visible = true;
            UI.Root.GetChild(8).Enabled = true;
            UI.Root.GetChild(9).Visible = false;
            UI.Root.GetChild(9).Enabled = false;

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


            UI.Root.GetChild(5).Visible = false;
            UI.Root.GetChild(5).Enabled = false;
            UI.Root.GetChild(6).Visible = true;
            UI.Root.GetChild(8).Visible = true;
            UI.Root.GetChild(8).Enabled = true;
            UI.Root.GetChild(9).Visible = false;
            UI.Root.GetChild(9).Enabled = false;

            //Issues with movement on some devices
            //Jump to position if animation causes issues:
            /* CameraNode.Position = new Vector3(0.00544398f, 0.176587f, 0.159439f);
               CameraNode.Rotation = new Quaternion(60f, -180f, 0f);
            */

            CameraNode.RemoveAllActions();
            panToHost();
            //Load hosting UI

        }

        private void BackButton_Pressed(PressedEventArgs obj)
        {
            for (uint i = 0; i < 4; i++)
                UI.Root.GetChild(i).Visible = true;


            UI.Root.GetChild(5).Visible = true;
            UI.Root.GetChild(5).Enabled = true;
            UI.Root.GetChild(6).Visible = false;
            UI.Root.GetChild(7).Visible = false;
            UI.Root.GetChild(8).Visible = false;
            UI.Root.GetChild(8).Enabled = false;

            UI.Root.GetChild("CreateLobby", true).Visible=false;
            UI.Root.GetChild("CreateLobby", true).Enabled = false;
            UI.Root.GetChild("JoinLobby", true).Visible = false;
            UI.Root.GetChild("JoinLobby", true).Enabled = false;

            UI.Root.GetChild("playerName", true).Visible = false;
            UI.Root.GetChild("playerNameLabel", true).Visible = false;
            UI.Root.GetChild("playerNameText", true).Visible = false;
            UI.Root.GetChild("serverList", true).Visible = false;

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
            var createLobbyButton = new Button();
            var joinLobbyButton = new Button();
            var playerName = new LineEdit();
            var playerNameText = new Text();

            var playerNameLabel = new Text();

            var serverList = new ListView();
            var serverListLabel = new Text();

            
            playerName.Name = "playerName";
            playerName.SetSize((Graphics.Width / 3) * 2, Graphics.Height / 20);
            playerName.SetPosition((Graphics.Width / 2) - playerName.Width / 2  , (Graphics.Height / 10 ) * 5);
            playerName.Editable = true;
            playerName.TextSelectable = true;
            playerName.Visible = false;
            playerName.AddChild(playerNameText);
            playerName.MaxLength = 24;
            playerName.Opacity = 0.6f;
            playerName.TextChanged += PlayerName_TextChanged;

            playerNameText.Name = "playerNameText";
            playerNameText.SetColor(new Color(0.0f, 0.0f, 0.0f, 1f));
            playerNameText.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
            playerNameText.SetPosition((Graphics.Width / 2) - playerNameText.Width / 2, playerName.Position.Y + playerNameText.Height / 2);
            playerNameText.Visible = false;

            playerNameLabel.Name = "playerNameLabel";
            playerNameLabel.SetColor(new Color(1.0f, 1.0f, 1.0f, 1f));
            playerNameLabel.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
            playerNameLabel.Value = "Player Name";
            playerNameLabel.SetPosition((Graphics.Width / 2) - playerNameLabel.Width / 2, playerName.Position.Y - playerNameLabel.Height - playerNameLabel.Height / 2);
            playerNameLabel.Visible = false;

            serverList.Name = "serverList";
            serverList.SetSize((Graphics.Width / 3) * 2, (Graphics.Height / 4));
            serverList.SetPosition((Graphics.Width / 2) - serverList.Width / 2, (Graphics.Height / 7));
            serverList.Visible = false;
            serverList.Opacity = 0.6f;

            copyrightNotice.Value = "Copyright © Advantage Software Group 2016. All Rights Reserved.";
            copyrightNotice.HorizontalAlignment = HorizontalAlignment.Center;
            copyrightNotice.VerticalAlignment = VerticalAlignment.Bottom;
            copyrightNotice.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            copyrightNotice.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);

            gameTitle.Texture = cache.GetTexture2D("Textures/gameTitle.png");
            gameTitle.BlendMode = BlendMode.Replace;
            gameTitle.SetSize((Graphics.Width / 5) * 3 , (Graphics.Width/5)*3);
            gameTitle.SetPosition((Graphics.Width / 2) - (gameTitle.Width / 2), 0);

            settingsButton.Texture = cache.GetTexture2D("Textures/settingsButton.png"); // Set texture
            settingsButton.BlendMode = BlendMode.Replace;
            settingsButton.SetSize(Graphics.Width / 10, Graphics.Width / 10);
            settingsButton.SetPosition(Graphics.Width - settingsButton.Width - 20, 20);
            settingsButton.Name = "Settings";
            settingsButton.Pressed += SettingsButton_Pressed;

            infoButton.Texture = cache.GetTexture2D("Textures/infoButton.png"); // Set texture
            infoButton.BlendMode = BlendMode.Replace;
            infoButton.SetSize(50, 50);
            infoButton.SetPosition(Graphics.Width - infoButton.Width - 20, Graphics.Height - infoButton.Height - 20);
            infoButton.Name = "About";
            infoButton.Pressed += InfoButton_Pressed;

            joinButton.Texture = cache.GetTexture2D("Textures/joinGameButton.png"); // Set texture
            joinButton.BlendMode = BlendMode.Replace;
            joinButton.SetSize(Graphics.Width / 3, (Graphics.Width / 4) / 2);
            joinButton.SetPosition(((Graphics.Width - joinButton.Width) / 5), (Graphics.Height / 6) * 5);
            joinButton.Name = "JoinGame";
            joinButton.Pressed += JoinButton_Pressed;

            hostButton.Texture = cache.GetTexture2D("Textures/hostGameButton.png"); // Set texture
            hostButton.BlendMode = BlendMode.Replace;
            hostButton.SetSize(Graphics.Width / 3, (Graphics.Width / 4) / 2);
            hostButton.SetPosition(((Graphics.Width - hostButton.Width) / 5) * 4, (Graphics.Height / 6) * 5);
            hostButton.Name = "HostGame";
            hostButton.Pressed += HostButton_Pressed;

            createLobbyButton.Texture = cache.GetTexture2D("Textures/createLobbyButton.png"); // Set texture
            createLobbyButton.BlendMode = BlendMode.Replace;
            createLobbyButton.SetSize((Graphics.Width / 3) * 2, Graphics.Width / 5);
            createLobbyButton.SetPosition(Graphics.Width/2 - createLobbyButton.Width/2, (Graphics.Height / 4) * 3);
            createLobbyButton.Name = "CreateLobby";
            createLobbyButton.Pressed += CreateLobbyButton_Pressed;

            createLobbyButton.Visible = false;
            createLobbyButton.Enabled = false;

            joinLobbyButton.Texture = cache.GetTexture2D("Textures/joinLobbyButton.png"); // Set texture
            joinLobbyButton.BlendMode = BlendMode.Replace;
            joinLobbyButton.SetSize((Graphics.Width / 3) * 2, Graphics.Width / 5);
            joinLobbyButton.SetPosition(Graphics.Width / 2 - createLobbyButton.Width / 2, (Graphics.Height / 4) * 3);
            joinLobbyButton.Name = "JoinLobby";
            joinLobbyButton.Pressed += JoinLobbyButton_Pressed;

            joinLobbyButton.Visible = false;
            joinLobbyButton.Enabled = false;

            Text hostText = new Text()
            {
                Value = "HOST GAME",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };


            hostText.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            hostText.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);
            hostText.Visible = false;

            Text joinText = new Text()
            {
                Value = "JOIN GAME",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };

            joinText.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            joinText.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);
            joinText.Visible = false;

            Button backButton = new Button();
            backButton.Texture = cache.GetTexture2D("Textures/backButton.png"); // Set texture
            backButton.BlendMode = BlendMode.Add;
            backButton.SetSize(Graphics.Width / 10, Graphics.Width / 10);
            backButton.SetPosition(20, 20);
            backButton.Name = "Back";
            backButton.Pressed += BackButton_Pressed;

            backButton.Visible = false;
            backButton.Enabled = false;

            var window = new Window()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

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
                VerticalAlignment = VerticalAlignment.Bottom
            };

            scroller.SetSize(window.Width, (window.Height / 5) * 4);
            scroller.ScrollBarsAutoVisible = true;
            scroller.SetScrollBarsVisible(false, true);
            
            var logo = new BorderImage()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };


            logo.Texture = cache.GetTexture2D("Textures/advantageLogo.png");
            logo.BlendMode = BlendMode.Replace;
            logo.SetSize((window.Width / 2), (window.Height / 5));

            var aboutContent = new Text()
            {
                Value = "\n\n\n\n\n\nGAME NAME GOES HERE\nVersion 0.0.5\n\nA Mixed Reality Texas Hold 'em Game\nby\nAdvantage Software Group\n\nAuthors\nLuke Rose, Jack Nicholson, Xinyi Li, Michael Uzoka, George Thomas, Rick Jin\n\nCoordinator\nDr. Peter Blanchfield, The University of Nottingham\n\nSupervisor\nDr. Thorsten Altenkirch, The University of Nottingham",
                TextAlignment = HorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Wordwrap = true
            };

            aboutContent.SetSize(window.Width, 300);

            var about = new ListView();

            about.SetSize(window.Width, (window.Height / 5) * 4);
            about.SetStyleAuto(null);

            aboutContent.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);
            aboutContent.SetColor(Color.Black);
            aboutContent.SetSize(window.Width, (window.Height / 4) * 3);
          
            about.AddItem(logo); //TODO: Figure out how to add items to the list one after the other, with a scrollbar

            about.AddItem(aboutContent);
            
            title.SetFont(cache.GetFont("Fonts/arial.ttf"), 25);
            title.SetPosition(0, 20);
            title.SetColor(Color.Black);

           
            window.AddChild(title);
            window.AddChild(about);
            window.Opacity = 0.5f;

            window.Visible = false;

            backButton.SetStyleAuto(null);
            settingsButton.SetStyleAuto(null);
            joinButton.SetStyleAuto(null);
            hostButton.SetStyleAuto(null);
            infoButton.SetStyleAuto(null);
            createLobbyButton.SetStyleAuto(null);
            joinLobbyButton.SetStyleAuto(null);

            UI.Root.AddChild(gameTitle);        //Index = 0
            UI.Root.AddChild(copyrightNotice);  //Index = 1
            UI.Root.AddChild(joinButton);       //Index = 2
            UI.Root.AddChild(hostButton);       //Index = 3
            UI.Root.AddChild(settingsButton);   //Index = 4
            UI.Root.AddChild(infoButton);        //Index = 5

            UI.Root.AddChild(hostText);         //Index = 6
            UI.Root.AddChild(joinText);         //Index = 7

            UI.Root.AddChild(backButton);       //Index = 8

            UI.Root.AddChild(window);           //Index = 9

            UI.Root.AddChild(createLobbyButton); //Index = 10
            UI.Root.AddChild(joinLobbyButton); //Index = 11

            UI.Root.AddChild(playerName);
            UI.Root.AddChild(playerNameText);
            UI.Root.AddChild(playerNameLabel);
            UI.Root.AddChild(serverList);
        }

        private void PlayerName_TextChanged(TextChangedEventArgs obj)
        {
            var textElement = (Text)UI.Root.GetChild("playerNameText", true);
            var textNode = (LineEdit)UI.Root.GetChild("playerName", true);
            textElement.Value = textNode.Text.ToUpper();
            textElement.SetPosition((Graphics.Width / 2) - textElement.Width / 2, textNode.Position.Y + textElement.Height / 2);
        }

        private void JoinLobbyButton_Pressed(PressedEventArgs obj)
        {
            UIElement nameNode = UI.Root.GetChild("playerName", true);
            LineEdit name = (LineEdit)nameNode;

            String myName = name.Text;
            var cache = ResourceCache;
            //Load Lobby Scene
            //LoadLobbyScene();

            //Load Playing Scene
            UI.Root.RemoveAllChildren();
            Node soundnode = scene.GetChild("Music", true);
            SoundSource sound = soundnode.GetComponent<SoundSource>(true);

            sound.Stop();
            scene = LoadPlayerScene();

            SetupViewport();

            Player me = new Player(myName, null);  //JACK: Can you setup client side connections here please
        }

        private void CreateLobbyButton_Pressed(PressedEventArgs obj)
        {
            //Load Hosting Scene
            Node soundnode = scene.GetChild("Music", true);
            SoundSource sound = soundnode.GetComponent<SoundSource>(true);

            sound.Stop();

            scene = LoadTableScene();
        }

        private void InfoButton_Pressed(PressedEventArgs obj)
        {
            UI.Root.GetChild(9).Visible = !UI.Root.GetChild(9).Visible;
          
        }

        private void SetupViewport()
        {
            Renderer.SetViewport(0, new Viewport(Context, scene, camera, null));
        }

        private void HostButton_Pressed(PressedEventArgs obj)
        {
            //Do host game stuff
            //TODO: Add intermediate host connection handling and setup
            UI.Root.GetChild("CreateLobby", true).Visible = true;
            UI.Root.GetChild("CreateLobby", true).Enabled = true;
            LoadHostingScene();
        }

        private void JoinButton_Pressed(PressedEventArgs obj)
        {
            //Do join game stuff
            //TODO: Add intermediate join connection handling and setup
            UI.Root.GetChild("JoinLobby", true).Visible = true;
            UI.Root.GetChild("JoinLobby", true).Enabled = true;

            UI.Root.GetChild("playerName", true).Visible = true;
            UI.Root.GetChild("playerNameLabel", true).Visible = true;
            UI.Root.GetChild("playerNameText", true).Visible = true;
            UI.Root.GetChild("serverList", true).Visible = true;
            LoadJoiningScene();
        }

        private void SettingsButton_Pressed(PressedEventArgs obj)
        {
            //Do settings stuff
            
        }
    }
}