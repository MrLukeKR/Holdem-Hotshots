using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Actions;
using Urho.Audio;
using Urho.Gui;
using Urho.Resources;

namespace TexasHoldemPoker.Game.Utils
{
    static class SceneManager
    {
        static private Graphics graphics;
        static private UI currentUi;
        static private Camera camera;
        static private Scene currentScene;
        static private ResourceCache cache;
        static private Renderer renderer;
        static private Context context;
        static private Node cameraNode;
        static private Node TargetNode;
        static private Vector3 initialCameraPos;

        static private Scene mainMenu;
        static private Scene playerScene;
        static private Scene tableScene;

        static private UI mainMenuUI;
        static private UI playerUI;
        static private UI tableUI;

        public static void init(Graphics gGraphics, UI ui, ResourceCache rCache, Renderer gRenderer, Context gContext)
        {
            currentUi = ui;
            graphics = gGraphics;
            cache = rCache;
            renderer = gRenderer;
            context = gContext;
        }

        private static void generateMenuScene()
        {
            mainMenu = new Scene();
            mainMenu.LoadXmlFromCache(cache, "Scenes/Menu.xml");
            var music = cache.GetSound("Music/MenuBGM.wav");
            music.Looped = true;
            Node musicNode = mainMenu.CreateChild("Music");
            SoundSource musicSource = musicNode.CreateComponent<SoundSource>();
            musicSource.SetSoundType(SoundType.Music.ToString());
            musicSource.Play(music);
            cameraNode = mainMenu.GetChild("MainCamera", true);
            TargetNode = mainMenu.GetChild("PokerTable", true);
            camera = cameraNode.GetComponent<Camera>();
            initialCameraPos = cameraNode.Position;
            //Figure out a way of playing this on devices that can handle it
            rotateCamera(TargetNode);
        }

        public static void generateAll()
        {
            generateMenuScene();
            generateMenuUI();
        }

        private static void generateMenuUI()
        {
            var copyrightNotice = new Text();
            var gameTitle = new BorderImage();
            var settingsButton = new Button();
            var joinButton = new Button();
            var hostButton = new Button();
            var infoButton = new Button();
            var createLobbyButton = new Button();
            var joinLobbyButton = new Button();

            var playerAvatar = new Button();

            var playerNameBox = new LineEdit();
            var playerNameText = new Text();

            var serverList = new ListView();

            var serverListLabel = new Text();

            var lobbyNameBox = new LineEdit();
            var lobbyNameText = new Text();

            lobbyNameBox.Name = "LobbyNameBox";
            lobbyNameBox.SetSize((graphics.Width / 3) * 2, graphics.Height / 20);
            lobbyNameBox.SetPosition((graphics.Width / 2) - lobbyNameBox.Width / 2, (graphics.Height / 7));
            lobbyNameBox.Editable = true;
            lobbyNameBox.TextSelectable = true;
            lobbyNameBox.Visible = false;
            lobbyNameBox.AddChild(lobbyNameText);
            lobbyNameBox.MaxLength = 24;
            lobbyNameBox.Opacity = 0.6f;
            lobbyNameBox.TextChanged += LobbyNameBox_TextChanged; ;

            lobbyNameText.Name = "LobbyNameText";
            lobbyNameText.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.5f));
            lobbyNameText.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
            lobbyNameText.Value = "Enter Lobby Name";
            lobbyNameText.SetPosition((graphics.Width / 2) - lobbyNameText.Width / 2, lobbyNameBox.Position.Y + lobbyNameText.Height / 2); //Position is dependant on playerNameBox - DO NOT EDIT THIS
            lobbyNameText.Visible = false;
            lobbyNameText.UseDerivedOpacity = false;

            playerAvatar.Name = "PlayerAvatar";
            playerAvatar.SetSize(graphics.Width / 3, graphics.Width / 3);
            playerAvatar.SetPosition((graphics.Width / 2) - (playerAvatar.Width / 2), (graphics.Height / 8));
            playerAvatar.Visible = false;
            playerAvatar.Pressed += PlayerAvatar_Pressed;
            playerAvatar.Opacity = 0.6f;

            playerNameBox.Name = "PlayerNameBox";
            playerNameBox.SetSize((graphics.Width / 3) * 2, graphics.Height / 20);
            playerNameBox.SetPosition((graphics.Width / 2) - playerNameBox.Width / 2, (graphics.Height / 5) * 2);
            playerNameBox.Editable = true;
            playerNameBox.TextSelectable = true;
            playerNameBox.Visible = false;
            playerNameBox.AddChild(playerNameText);
            playerNameBox.MaxLength = 24;
            playerNameBox.Opacity = 0.6f;
            playerNameBox.TextChanged += PlayerNameBox_TextChanged;

            playerNameText.Name = "PlayerNameText";
            playerNameText.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.5f));
            playerNameText.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
            playerNameText.Value = "Enter Player Name";
            playerNameText.SetPosition((graphics.Width / 2) - playerNameText.Width / 2, playerNameBox.Position.Y + playerNameText.Height / 2); //Position is dependant on playerNameBox - DO NOT EDIT THIS
            playerNameText.Visible = false;
            playerNameText.UseDerivedOpacity = false;

            serverList.Name = "ServerList";
            serverList.SetSize((graphics.Width / 3) * 2, (graphics.Height / 4));
            serverList.SetPosition((graphics.Width / 2) - serverList.Width / 2, (graphics.Height / 15) * 8);
            serverList.Visible = false;
            serverList.Opacity = 0.6f;

            copyrightNotice.Name = "CopyrightNotice";
            copyrightNotice.Value = "Copyright © Advantage Software Group 2016 - 2017. All Rights Reserved.";
            copyrightNotice.HorizontalAlignment = HorizontalAlignment.Center;
            copyrightNotice.VerticalAlignment = VerticalAlignment.Bottom;
            copyrightNotice.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            copyrightNotice.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);

            gameTitle.Name = "GameLogo";
            gameTitle.Texture = cache.GetTexture2D("Textures/gameTitle.png");
            gameTitle.BlendMode = BlendMode.Replace;
            gameTitle.SetSize((graphics.Width / 5) * 3, (graphics.Width / 5) * 3);
            gameTitle.SetPosition((graphics.Width / 2) - (gameTitle.Width / 2), 0);

            settingsButton.Texture = cache.GetTexture2D("Textures/settingsButton.png"); // Set texture
            settingsButton.BlendMode = BlendMode.Replace;
            settingsButton.SetSize(graphics.Width / 10, graphics.Width / 10);
            settingsButton.SetPosition(graphics.Width - settingsButton.Width - 20, 20);
            settingsButton.Name = "SettingsButton";
            //settingsButton.Pressed += SettingsButton_Pressed;

            infoButton.Texture = cache.GetTexture2D("Textures/infoButton.png"); // Set texture
            infoButton.BlendMode = BlendMode.Replace;
            infoButton.SetSize(50, 50);
            infoButton.SetPosition(graphics.Width - infoButton.Width - 20, graphics.Height - infoButton.Height - 20);
            infoButton.Name = "InfoButton";
            //infoButton.Pressed += InfoButton_Pressed;

            joinButton.Texture = cache.GetTexture2D("Textures/joinGameButton.png"); // Set texture
            joinButton.BlendMode = BlendMode.Replace;
            joinButton.SetSize(graphics.Width / 3, (graphics.Width / 4) / 2);
            joinButton.SetPosition(((graphics.Width - joinButton.Width) / 5), (graphics.Height / 6) * 5);
            joinButton.Name = "JoinGameButton";
            joinButton.Pressed += JoinButton_Pressed;

            hostButton.Texture = cache.GetTexture2D("Textures/hostGameButton.png"); // Set texture
            hostButton.BlendMode = BlendMode.Replace;
            hostButton.SetSize(graphics.Width / 3, (graphics.Width / 4) / 2);
            hostButton.SetPosition(((graphics.Width - hostButton.Width) / 5) * 4, (graphics.Height / 6) * 5);
            hostButton.Name = "HostGameButton";
            //hostButton.Pressed += HostButton_Pressed;

            createLobbyButton.Texture = cache.GetTexture2D("Textures/createLobbyButton.png"); // Set texture
            createLobbyButton.BlendMode = BlendMode.Replace;
            createLobbyButton.SetSize((graphics.Width / 3) * 2, graphics.Width / 5);
            createLobbyButton.SetPosition(graphics.Width / 2 - createLobbyButton.Width / 2, (graphics.Height / 4) * 3);
            createLobbyButton.Name = "CreateLobbyButton";
            //createLobbyButton.Pressed += CreateLobbyButton_Pressed;

            createLobbyButton.Visible = false;

            joinLobbyButton.Texture = cache.GetTexture2D("Textures/joinLobbyButton.png"); // Set texture
            joinLobbyButton.BlendMode = BlendMode.Replace;
            joinLobbyButton.SetSize((graphics.Width / 3) * 2, graphics.Width / 5);
            joinLobbyButton.SetPosition(graphics.Width / 2 - createLobbyButton.Width / 2, (graphics.Height / 6) * 5);
            joinLobbyButton.Name = "JoinLobbyButton";
            joinLobbyButton.Pressed += JoinLobbyButton_Pressed;

            joinLobbyButton.Visible = false;

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
            backButton.SetSize(graphics.Width / 10, graphics.Width / 10);
            backButton.SetPosition(20, 20);
            backButton.Name = "BackButton";
            backButton.Pressed += BackButton_Pressed;

            backButton.Visible = false;

            var window = new Window()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            window.SetSize((graphics.Width / 3) * 2, graphics.Height / 3);

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
                Value = "\n\n\n\n\n\nHold'em Hotshots\nVersion 0.0.7\n\nA Mixed Reality Texas Hold 'em Game\nby\nAdvantage Software Group\n\nAuthors\nLuke Rose, Jack Nicholson, Xinyi Li, Michael Uzoka, George Thomas, Rick Jin\n\nCoordinator\nDr. Peter Blanchfield, The University of Nottingham\n\nSupervisor\nDr. Thorsten Altenkirch, The University of Nottingham",
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

            currentUi.Root.AddChild(gameTitle);
            currentUi.Root.AddChild(copyrightNotice);
            currentUi.Root.AddChild(joinButton);
            currentUi.Root.AddChild(hostButton);
            currentUi.Root.AddChild(settingsButton);
            currentUi.Root.AddChild(infoButton);

            currentUi.Root.AddChild(backButton);

            currentUi.Root.AddChild(window);

            currentUi.Root.AddChild(createLobbyButton);
            currentUi.Root.AddChild(joinLobbyButton);

            currentUi.Root.AddChild(playerAvatar);
            currentUi.Root.AddChild(playerNameBox);
            currentUi.Root.AddChild(playerNameText);
            currentUi.Root.AddChild(serverList);

            currentUi.Root.AddChild(lobbyNameBox);
            currentUi.Root.AddChild(lobbyNameText);
        }

        static public void showScene(String sceneName)
        {
            switch (sceneName)
            {
                case "MainMenu": currentScene = mainMenu; break;
            }
            SetupViewport(new Viewport(context, currentScene, cameraNode.GetComponent<Camera>(), null));
        }

        static private void panToOriginalPosition()
        {
            cameraNode.RunActions(new Parallel(new MoveTo(1, initialCameraPos),
                                               new RotateTo(1, 20f, 0f, 0f)));
            cameraNode.LookAt(TargetNode.Position, Vector3.Up, TransformSpace.World);
        }

        static private void panToHost()
        {
            cameraNode.RunActions(new Parallel(new MoveTo(1, new Vector3(0.00544398f, 0.176587f, 0.159439f)),
                                               new RotateTo(1, 60f, -180f, 0f)));
        }

        static private void panToJoin()
        {
            cameraNode.RunActions(new Parallel(new MoveTo(1, new Vector3(0f, 0.106208f, -0.139909f)),
                                               new RotateTo(1, 20f, 0f, 0f)));
        }

        static private async void rotateCamera(Node target)
        {
            await cameraNode.RunActionsAsync(new RepeatForever(new RotateAroundBy(60, TargetNode.Position, 0.0f, 360.0f, 0.0f,
                                                               TransformSpace.World)));
        }

        static private void SetupViewport(Viewport vp) { renderer.SetViewport(0, vp); }






       static private void LoadHostingScene()
        {
            mainMenuUI.Root.GetChild("CreateLobbyButton", true).Visible = true;
            //Disabled until all server options have been set
            //UI.Root.GetChild("CreateLobbyButton", true).Enabled = false;
            //Enabled for debugging
            mainMenuUI.Root.GetChild("CreateLobbyButton", true).Enabled = true;
            mainMenuUI.Root.GetChild("LobbyNameBox", true).Visible = true;
            mainMenuUI.Root.GetChild("LobbyNameText", true).Visible = true;
            mainMenuUI.Root.GetChild("BackButton", true).Visible = true;
            //Issues with movement on some devices
            //Jump to position if animation causes issues:
            /* CameraNode.Position = new Vector3(0.00544398f, 0.176587f, 0.159439f);
               CameraNode.Rotation = new Quaternion(60f, -180f, 0f);
            */
            cameraNode.RemoveAllActions();
            panToHost();
            //Load hosting UI
        }

       static  private void BackButton_Pressed(PressedEventArgs obj)
        {
            currentUi.Root.GetChild("BackButton", true).Visible = false;
            currentUi.Root.GetChild("CreateLobbyButton", true).Visible = false;
            currentUi.Root.GetChild("JoinLobbyButton", true).Visible = false;
            currentUi.Root.GetChild("PlayerAvatar", true).Visible = false;
            currentUi.Root.GetChild("PlayerNameBox", true).Visible = false;
            currentUi.Root.GetChild("PlayerNameText", true).Visible = false;
            currentUi.Root.GetChild("ServerList", true).Visible = false;
            currentUi.Root.GetChild("LobbyNameBox", true).Visible = false;
            currentUi.Root.GetChild("LobbyNameText", true).Visible = false;
            panToOriginalPosition();
            rotateCamera(TargetNode);
        }




       static private void PlayerAvatar_Pressed(PressedEventArgs obj) { }



       static private void LobbyNameBox_TextChanged(TextChangedEventArgs obj)
        {
            var textElement = (Text)mainMenuUI.Root.GetChild("LobbyNameText", true);
            var textNode = (LineEdit)mainMenuUI.Root.GetChild("LobbyNameBox", true);
            if (textNode.Text.Length > 0)
            {
                textElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 1f));
                textElement.Value = textNode.Text;
            }
            else
            {
                textElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.5f));
                textElement.Value = "Enter Lobby Name";
            }
            textElement.SetPosition((graphics.Width / 2) - textElement.Width / 2,
                                    textNode.Position.Y + textElement.Height / 2);
        }

        static private void PlayerNameBox_TextChanged(TextChangedEventArgs obj)
        {
            var textElement = (Text)mainMenuUI.Root.GetChild("PlayerNameText", true);
            var textNode = (LineEdit)mainMenuUI.Root.GetChild("PlayerNameBox", true);
            if (textNode.Text.Length > 0)
            {
                textElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 1f));
                textElement.Value = textNode.Text;
            }
            else
            {
                textElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.5f));
                textElement.Value = "Enter Player Name";
            }
            textElement.SetPosition((graphics.Width / 2) - textElement.Width / 2,
                                    textNode.Position.Y + textElement.Height / 2);
        }

        static private void JoinLobbyButton_Pressed(PressedEventArgs obj)
        {
            UIElement nameNode = mainMenuUI.Root.GetChild("PlayerNameBox", true);
            LineEdit name = (LineEdit)nameNode;

            String myName = name.Text;

            mainMenuUI.Root.RemoveAllChildren();
            // UI.Clear();
            Node soundnode = currentScene.GetChild("Music", true);
            SoundSource sound = soundnode.GetComponent<SoundSource>(true);
            sound.Stop();
            currentScene.UpdateEnabled = false;
            currentScene.Clear(true, true);

            //JACK: Setup client/server interaction and information here (100 needs
            //to be replaced with server "Buy In" amount and null, the socket)
            Player me = new Player(myName, 100, null);

            //var playerScene = me.initPlayerScene(cache, currentUi, Current.Input);
            
            //Load Lobby Scene
            //LoadLobbyScene();

            //Load Playing Scene
            
        }
/*
       static  private void CreateLobbyButton_Pressed(PressedEventArgs obj)
        {
            //Load Hosting Scene
            UI.Root.RemoveAllChildren();
            Node soundnode = scene.GetChild("Music", true);
            SoundSource sound = soundnode.GetComponent<SoundSource>(true);

            sound.Stop();
            scene.Clear(true, true);
            scene = LoadTableScene();

            PlayerViewport = new Viewport(Context, scene, camera, null);

            SetupViewport(PlayerViewport);

            System.Console.WriteLine("Got here");

            initTableCardPositions();

            System.Console.WriteLine("Got here");

            initTableUI();


            Room room = new Room();

            //TODO: Wait until players join to start game
            room.addPlayer(new Player("Luke", 1000, null));
            room.addPlayer(new Player("Jack", 1000, null));
            room.addPlayer(new Player("George", 1000, null));
            room.addPlayer(new Player("Xinyi", 1000, null));
            room.addPlayer(new Player("Rick", 1000, null));
            room.addPlayer(new Player("Mike", 1000, null));
            ///////

            var game = new PokerGame(room, scene, UI, 1000);

            game.start();
        }


        private Scene LoadTableScene()
        {
            var cache = ResourceCache;
            Scene tableScene = new Scene();
            tableScene.LoadXmlFromCache(cache, "Scenes/Table.xml");
            //TODO: Make the camera update when the scene is changed (EVENT)
            CameraNode = tableScene.GetChild("MainCamera", true);
            camera = CameraNode.GetComponent<Camera>();
            var music = cache.GetSound("Music/TableBGM.wav");
            music.Looped = true;
            Node musicNode = tableScene.CreateChild("Music");
            SoundSource musicSource = musicNode.CreateComponent<SoundSource>();
            musicSource.SetSoundType(SoundType.Music.ToString());
            musicSource.Play(music);
            return tableScene;
        }
        */
        static private void LoadJoiningScene()
        {
            //toggleMainMenuUI();
            currentUi.Root.GetChild("PlayerAvatar", true).Visible = true;
            currentUi.Root.GetChild("JoinLobbyButton", true).Visible = true;
            //Disabled until a valid server has been selected
            //UI.Root.GetChild("JoinLobbyButton", true).Enabled = false;
            currentUi.Root.GetChild("JoinLobbyButton", true).Enabled = true; //Enabled for debugging purposes
            currentUi.Root.GetChild("BackButton", true).Visible = true;
            currentUi.Root.GetChild("PlayerNameBox", true).Visible = true;
            currentUi.Root.GetChild("PlayerNameText", true).Visible = true;
            currentUi.Root.GetChild("ServerList", true).Visible = true;
            //Issues with movement on some devices
            //Jump to position if animation causes issues:
            // CameraNode.Position = new Vector3(0.00544398f, 0.176587f, 0.159439f);
            //   CameraNode.Rotation = new Quaternion(60f, -180f, 0f);
            

            //DO LOADING
            cameraNode.RemoveAllActions();
            panToJoin();
            //Load hosting UI
            // return playingScene;
        }

            /*
        private void toggleMainMenuUI()
        {
            UI.Root.GetChild("JoinGameButton", true).Visible = !UI.Root.GetChild("JoinGameButton", true).Visible;
            UI.Root.GetChild("HostGameButton", true).Visible = !UI.Root.GetChild("HostGameButton", true).Visible;
            UI.Root.GetChild("InfoButton", true).Visible = !UI.Root.GetChild("InfoButton", true).Visible;
            UI.Root.GetChild("GameLogo", true).Visible = !UI.Root.GetChild("GameLogo", true).Visible;
            UI.Root.GetChild("CopyrightNotice", true).Visible = !UI.Root.GetChild("CopyrightNotice", true).Visible;
        }
        */
        /*
        private void initTableUI()
        {
            var cache = ResourceCache;

            Text tableMessage = new Text();
            tableMessage.Name = "TableMessage";
            tableMessage.SetFont(cache.GetFont("Fonts/arial.ttf"), 30);
            tableMessage.SetColor(new Color(1f, 1f, 1f, 1f));
            tableMessage.HorizontalAlignment = HorizontalAlignment.Center;
            tableMessage.VerticalAlignment = VerticalAlignment.Top;
            tableMessage.UseDerivedOpacity = false;

            //TODO: Find a way to rotate this for landscape

            UI.Root.AddChild(tableMessage);
        }
        */
        static private void InfoButton_Pressed(PressedEventArgs obj) { }
        static private void HostButton_Pressed(PressedEventArgs obj)
        {
            //Do host game stuff
            //TODO: Add intermediate host connection handling and setup
            LoadHostingScene();
        }


        static private void JoinButton_Pressed(PressedEventArgs obj)
        {
            //Do join game stuff
            //TODO: Add intermediate join connection handling and setup
            LoadJoiningScene();
        }

        //Do settings stuff
        static private void SettingsButton_Pressed(PressedEventArgs obj) { }
    }
    
}

