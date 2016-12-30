using Urho;
using Urho.Gui;
using Urho.Actions;
using Urho.Audio;
using System;

namespace TexasHoldemPoker
{
    public class Poker : Application
    {
        public Viewport MenuViewport { get; private set; }
        public Viewport PlayerViewport { get; private set; }

        public Vector3 card1DealingPos;
        public Vector3 card1HoldingPos;
        public Vector3 card1ViewingPos;

        public Vector3 card2DealingPos;
        public Vector3 card2HoldingPos;
        public Vector3 card2ViewingPos;

        public static Vector3 cardTableDealingPos;
        public static Vector3 card1TablePos;
        public static Vector3 card2TablePos;
        public static Vector3 card3TablePos;
        public static Vector3 card4TablePos;
        public static Vector3 card5TablePos;
        
        public Poker() : base(new ApplicationOptions(assetsFolder: "Data")) { }
        
        public Poker(ApplicationOptions opts) : base(opts) { }

        Scene scene;
        Camera camera;
        Node CameraNode;
        Node TargetNode;
        Vector3 initialCameraPos;
        
        private void initPlayerCardPositions()
        {
            card1DealingPos = new Vector3(-8.25f, 10f, 15f);
            card1HoldingPos = new Vector3(-8.25f, 6f, 15f);
            card1ViewingPos = GetScreenToWorldPoint((Graphics.Width / 2) + (Graphics.Width / 11), (Graphics.Height / 3), 17f);

            card2DealingPos = new Vector3(-8.75f, 10f, 15.05f);
            card2HoldingPos = new Vector3(-8.75f, 5.75f, 15.05f);
            card2ViewingPos = GetScreenToWorldPoint((Graphics.Width / 2) - (Graphics.Width / 11), (Graphics.Height / 3), 17f);
        }

        public void initTableCardPositions()
        {
            cardTableDealingPos = GetScreenToWorldPoint((Graphics.Width / 2), Graphics.Height, 0.065f);

            card1TablePos = GetScreenToWorldPoint((Graphics.Width / 2), (Graphics.Height / 2) -2, 0.065f);
            card2TablePos = GetScreenToWorldPoint((Graphics.Width / 2), (Graphics.Height / 2) -1, 0.065f);
            card3TablePos = GetScreenToWorldPoint((Graphics.Width / 2), (Graphics.Height / 2)   , 0.065f);
            card4TablePos = GetScreenToWorldPoint((Graphics.Width / 2), (Graphics.Height / 2) +1, 0.065f);
            card5TablePos = GetScreenToWorldPoint((Graphics.Width / 2), (Graphics.Height / 2) +2, 0.065f);
        }

        protected override void Start()
        {
            base.Start();

            
            scene = LoadMenuScene();

            MenuViewport = new Viewport(Context, scene, CameraNode.GetComponent<Camera>(), null);

            LoadMenuUI();
            SetupViewport(MenuViewport);
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

        private Scene LoadPlayerScene(Player player)
        {
            var cache = ResourceCache;
            Scene playerScene = new Scene();
           
            playerScene.LoadXmlFromCache(cache, "Scenes/Player.xml");

            CameraNode = playerScene.GetChild("MainCamera", true);  //TODO: Make the camera update when the scene is changed (EVENT)
            camera = CameraNode.GetComponent<Camera>();

            Card card1 = new Card(Card.Suit.CLUBS,Card.Rank.ACE);
            Card card2 = new Card(Card.Suit.CLUBS, Card.Rank.KING);

            card1.getNode().Position = card1DealingPos; //TODO: Make this dependent on the device's height/width
            card1.getNode().Name = "Card1";

            card2.getNode().Position = card2DealingPos;
            card2.getNode().Name = "Card2";

            playerScene.AddChild(card1.getNode());
            playerScene.AddChild(card2.getNode());
            
            initPlayerCardPositions();

            card1.getNode().RunActions(new MoveTo(.5f, card1HoldingPos)); //TODO: Only play this animation when dealt a card
            card2.getNode().RunActions( new MoveTo(.5f, card2HoldingPos));
            
            Text coords = new Text();
            coords.Name = "coords";
            coords.SetColor(new Color(1.0f, 1.0f, 1.0f, 1f));
            coords.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
            coords.Value = "X: 0, Y: 0";
            coords.VerticalAlignment = VerticalAlignment.Center;
            coords.HorizontalAlignment = HorizontalAlignment.Center;
            coords.Visible = true;

            var statusInformationText = new Text();

            statusInformationText.Name = "StatusInformationLabel";
            statusInformationText.SetColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));
            statusInformationText.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
            statusInformationText.HorizontalAlignment = HorizontalAlignment.Center;
            statusInformationText.VerticalAlignment = VerticalAlignment.Top;
            statusInformationText.SetPosition(0, statusInformationText.Height / 2);
            statusInformationText.Visible = true;
            statusInformationText.Value = player.getName() + " - $" + player.getChips();

            UI.Root.AddChild(coords);
            UI.Root.AddChild(statusInformationText);

            var input = Current.Input;

            input.TouchBegin += Input_TouchBegin;
            input.TouchMove += Input_TouchMove;
            input.TouchEnd += Input_TouchEnd;
            
            return playerScene;
        }

        private void Input_TouchEnd(TouchEndEventArgs obj)
        {
            HoldCards();
        }

        private void Input_TouchMove(TouchMoveEventArgs obj)
        {
            updateCoords();
        }

        private void Input_TouchBegin(TouchBeginEventArgs obj)
        {
            Node tempNode = GetNodeAt(Current.Input.GetTouch(0).Position);

            if (tempNode != null)
                if (tempNode.Name.Contains("Card"))
                    ViewCards();
                else if (tempNode.Name.Contains("Chip"))
                    ToggleActionMenu();

        }
        
        private void ViewCards()
        {
            scene.GetChild("Card1", true).RunActions(new MoveTo(.1f,card1ViewingPos));
            scene.GetChild("Card2", true).RunActions(new MoveTo(.1f, card2ViewingPos));
        }

        private void HoldCards()
        {
            var card1 = scene.GetChild("Card1", true);
            var card2 = scene.GetChild("Card2", true);

            if (card1.Position != card1HoldingPos)
                card1.RunActions(new MoveTo(.1f, card1HoldingPos));

            if (card2.Position != card2HoldingPos)
               card2.RunActions(new MoveTo(.1f, card2HoldingPos));
        }

        private void ToggleActionMenu()
        {
            UI.Root.GetChild("CheckButton", true).Visible = !UI.Root.GetChild("CheckButton", true).Visible;
            UI.Root.GetChild("FoldButton", true).Visible = !UI.Root.GetChild("FoldButton", true).Visible;
            UI.Root.GetChild("RaiseButton", true).Visible = !UI.Root.GetChild("RaiseButton", true).Visible;
            UI.Root.GetChild("CallButton", true).Visible = !UI.Root.GetChild("CallButton", true).Visible;
            UI.Root.GetChild("AllInButton", true).Visible = !UI.Root.GetChild("AllInButton", true).Visible;
        }

        public Node GetNodeAt(IntVector2 touchPosition)
        {
            var pos = Vector3.Zero;
            
            if (UI.GetElementAt(touchPosition, true) == null)
            {
                Ray cameraRay = camera.GetScreenRay((float)touchPosition.X / Graphics.Width, (float)touchPosition.Y / Graphics.Height);
                var result = scene.GetComponent<Octree>().RaycastSingle(cameraRay, RayQueryLevel.Triangle, 10, DrawableFlags.Geometry, uint.MaxValue);
                if (result != null)
                    return result.Value.Node;
            }
            return null;
        }
        
        private void updateCoords() {

            var input = Current.Input;

            TouchState state = input.GetTouch(0);
            var pos = state.Position;
            
            var coordsNode = UI.Root.GetChild("coords", true);
            var coords = (Text)coordsNode;

            Vector3 a = GetScreenToWorldPoint(pos, 15f);

            coords.Value = "X:" + pos.X + " Y: " + pos.Y + "\nWS: " + a;
        }


        private Vector3 GetScreenToWorldPoint(int x, int y, float z)
        {
            Vector3 a = camera.ScreenToWorldPoint(new Vector3(x - (Graphics.Width / 2), y - (Graphics.Height / 2), 0));
            a.Z = z;

            return a;
        }

        private Vector3 GetScreenToWorldPoint(IntVector2 ScreenPos, float z)
        {

            Vector3 a = camera.ScreenToWorldPoint(new Vector3(ScreenPos.X - (Graphics.Width / 2), ScreenPos.Y - (Graphics.Height / 2), 0));
            a.Z = z;

            return a;
        }

        private Scene LoadTableScene()
        {        
            var cache = ResourceCache;
            Scene tableScene = new Scene();
            
            tableScene.LoadXmlFromCache(cache, "Scenes/Table.xml");
            
            CameraNode = tableScene.GetChild("MainCamera", true);  //TODO: Make the camera update when the scene is changed (EVENT)
            camera = CameraNode.GetComponent<Camera>();

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
            toggleMainMenuUI();

            UI.Root.GetChild("PlayerAvatar", true).Visible = true;
            UI.Root.GetChild("JoinLobbyButton", true).Visible = true;

          //UI.Root.GetChild("JoinLobbyButton", true).Enabled = false; //Disabled until a valid server has been selected
            UI.Root.GetChild("JoinLobbyButton", true).Enabled = true; //Enabled for debugging purposes

            UI.Root.GetChild("BackButton", true).Visible = true;
            UI.Root.GetChild("PlayerNameBox", true).Visible = true;
            UI.Root.GetChild("PlayerNameText", true).Visible = true;
            UI.Root.GetChild("ServerList", true).Visible = true;

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

        private void toggleMainMenuUI()
        {
            UI.Root.GetChild("JoinGameButton", true).Visible = !UI.Root.GetChild("JoinGameButton", true).Visible;
            UI.Root.GetChild("HostGameButton", true).Visible = !UI.Root.GetChild("HostGameButton", true).Visible;
            UI.Root.GetChild("InfoButton", true).Visible = !UI.Root.GetChild("InfoButton", true).Visible;
            UI.Root.GetChild("GameLogo", true).Visible = !UI.Root.GetChild("GameLogo", true).Visible;
            UI.Root.GetChild("CopyrightNotice", true).Visible = !UI.Root.GetChild("CopyrightNotice", true).Visible;
        }

        private void LoadHostingScene()
        {
            toggleMainMenuUI();

            UI.Root.GetChild("CreateLobbyButton", true).Visible = true;

            //UI.Root.GetChild("CreateLobbyButton", true).Enabled = false; //Disabled until all server options have been set
            UI.Root.GetChild("CreateLobbyButton", true).Enabled = true; //Enabled for debugging

            UI.Root.GetChild("LobbyNameBox", true).Visible = true;
            UI.Root.GetChild("LobbyNameText", true).Visible = true;

            UI.Root.GetChild("BackButton", true).Visible = true;

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
            toggleMainMenuUI();

            UI.Root.GetChild("BackButton", true).Visible = false;
            UI.Root.GetChild("CreateLobbyButton", true).Visible=false;
            UI.Root.GetChild("JoinLobbyButton", true).Visible = false;
            UI.Root.GetChild("PlayerAvatar", true).Visible = false;
            UI.Root.GetChild("PlayerNameBox", true).Visible = false;
            UI.Root.GetChild("PlayerNameText", true).Visible = false;
            UI.Root.GetChild("ServerList", true).Visible = false;
            UI.Root.GetChild("LobbyNameBox", true).Visible = false;
            UI.Root.GetChild("LobbyNameText", true).Visible = false;

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

        private async void gameStartTableAnimation()
        {
            await CameraNode.RunActionsAsync(new MoveTo(1, new Vector3(0, 0, -0.01f)));
            
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

            var playerAvatar = new Button();

            var playerNameBox = new LineEdit();
            var playerNameText = new Text();

            var serverList = new ListView();

            var serverListLabel = new Text();

            var lobbyNameBox = new LineEdit();
            var lobbyNameText = new Text();
            
            lobbyNameBox.Name = "LobbyNameBox";
            lobbyNameBox.SetSize((Graphics.Width / 3) * 2, Graphics.Height / 20);
            lobbyNameBox.SetPosition((Graphics.Width / 2) - lobbyNameBox.Width / 2, (Graphics.Height / 7));
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
            lobbyNameText.SetPosition((Graphics.Width / 2) - lobbyNameText.Width / 2, lobbyNameBox.Position.Y + lobbyNameText.Height / 2); //Position is dependant on playerNameBox - DO NOT EDIT THIS
            lobbyNameText.Visible = false;
            lobbyNameText.UseDerivedOpacity = false;

            playerAvatar.Name = "PlayerAvatar";
            playerAvatar.SetSize(Graphics.Width / 3, Graphics.Width / 3);
            playerAvatar.SetPosition((Graphics.Width / 2) - (playerAvatar.Width/2), (Graphics.Height / 8));
            playerAvatar.Visible = false;
            playerAvatar.Pressed += PlayerAvatar_Pressed;
            playerAvatar.Opacity = 0.6f;
                
            playerNameBox.Name = "PlayerNameBox";
            playerNameBox.SetSize((Graphics.Width / 3) * 2, Graphics.Height / 20);
            playerNameBox.SetPosition((Graphics.Width / 2) - playerNameBox.Width / 2  , (Graphics.Height / 5 ) * 2);
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
            playerNameText.SetPosition((Graphics.Width / 2) - playerNameText.Width / 2, playerNameBox.Position.Y + playerNameText.Height / 2); //Position is dependant on playerNameBox - DO NOT EDIT THIS
            playerNameText.Visible = false;
            playerNameText.UseDerivedOpacity = false;

            serverList.Name = "ServerList";
            serverList.SetSize((Graphics.Width / 3) * 2, (Graphics.Height / 4));
            serverList.SetPosition((Graphics.Width / 2) - serverList.Width / 2, (Graphics.Height / 15) * 8);
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
            gameTitle.SetSize((Graphics.Width / 5) * 3 , (Graphics.Width/5)*3);
            gameTitle.SetPosition((Graphics.Width / 2) - (gameTitle.Width / 2), 0);

            settingsButton.Texture = cache.GetTexture2D("Textures/settingsButton.png"); // Set texture
            settingsButton.BlendMode = BlendMode.Replace;
            settingsButton.SetSize(Graphics.Width / 10, Graphics.Width / 10);
            settingsButton.SetPosition(Graphics.Width - settingsButton.Width - 20, 20);
            settingsButton.Name = "SettingsButton";
            settingsButton.Pressed += SettingsButton_Pressed;

            infoButton.Texture = cache.GetTexture2D("Textures/infoButton.png"); // Set texture
            infoButton.BlendMode = BlendMode.Replace;
            infoButton.SetSize(50, 50);
            infoButton.SetPosition(Graphics.Width - infoButton.Width - 20, Graphics.Height - infoButton.Height - 20);
            infoButton.Name = "InfoButton";
            infoButton.Pressed += InfoButton_Pressed;

            joinButton.Texture = cache.GetTexture2D("Textures/joinGameButton.png"); // Set texture
            joinButton.BlendMode = BlendMode.Replace;
            joinButton.SetSize(Graphics.Width / 3, (Graphics.Width / 4) / 2);
            joinButton.SetPosition(((Graphics.Width - joinButton.Width) / 5), (Graphics.Height / 6) * 5);
            joinButton.Name = "JoinGameButton";
            joinButton.Pressed += JoinButton_Pressed;

            hostButton.Texture = cache.GetTexture2D("Textures/hostGameButton.png"); // Set texture
            hostButton.BlendMode = BlendMode.Replace;
            hostButton.SetSize(Graphics.Width / 3, (Graphics.Width / 4) / 2);
            hostButton.SetPosition(((Graphics.Width - hostButton.Width) / 5) * 4, (Graphics.Height / 6) * 5);
            hostButton.Name = "HostGameButton";
            hostButton.Pressed += HostButton_Pressed;

            createLobbyButton.Texture = cache.GetTexture2D("Textures/createLobbyButton.png"); // Set texture
            createLobbyButton.BlendMode = BlendMode.Replace;
            createLobbyButton.SetSize((Graphics.Width / 3) * 2, Graphics.Width / 5);
            createLobbyButton.SetPosition(Graphics.Width/2 - createLobbyButton.Width/2, (Graphics.Height / 4) * 3);
            createLobbyButton.Name = "CreateLobbyButton";
            createLobbyButton.Pressed += CreateLobbyButton_Pressed;

            createLobbyButton.Visible = false;

            joinLobbyButton.Texture = cache.GetTexture2D("Textures/joinLobbyButton.png"); // Set texture
            joinLobbyButton.BlendMode = BlendMode.Replace;
            joinLobbyButton.SetSize((Graphics.Width / 3) * 2, Graphics.Width / 5);
            joinLobbyButton.SetPosition(Graphics.Width / 2 - createLobbyButton.Width / 2, (Graphics.Height / 6) * 5);
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
            backButton.SetSize(Graphics.Width / 10, Graphics.Width / 10);
            backButton.SetPosition(20, 20);
            backButton.Name = "BackButton";
            backButton.Pressed += BackButton_Pressed;

            backButton.Visible = false;

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
            
            UI.Root.AddChild(gameTitle);        
            UI.Root.AddChild(copyrightNotice);  
            UI.Root.AddChild(joinButton);       
            UI.Root.AddChild(hostButton);       
            UI.Root.AddChild(settingsButton);   
            UI.Root.AddChild(infoButton);       

            UI.Root.AddChild(backButton);       

            UI.Root.AddChild(window);           

            UI.Root.AddChild(createLobbyButton); 
            UI.Root.AddChild(joinLobbyButton);

            UI.Root.AddChild(playerAvatar);
            UI.Root.AddChild(playerNameBox);
            UI.Root.AddChild(playerNameText);
            UI.Root.AddChild(serverList);

            UI.Root.AddChild(lobbyNameBox);
            UI.Root.AddChild(lobbyNameText);

        }

        private void LobbyNameBox_TextChanged(TextChangedEventArgs obj)
        {
            var textElement = (Text)UI.Root.GetChild("LobbyNameText", true);
            var textNode = (LineEdit)UI.Root.GetChild("LobbyNameBox", true);
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

            textElement.SetPosition((Graphics.Width / 2) - textElement.Width / 2, textNode.Position.Y + textElement.Height / 2);
        }

        private void PlayerAvatar_Pressed(PressedEventArgs obj)
        {
            //TODO: Allow avatar selection
        }

        private void PlayerNameBox_TextChanged(TextChangedEventArgs obj)
        {
            var textElement = (Text)UI.Root.GetChild("PlayerNameText", true);
            var textNode = (LineEdit)UI.Root.GetChild("PlayerNameBox", true);
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

            textElement.SetPosition((Graphics.Width / 2) - textElement.Width / 2, textNode.Position.Y + textElement.Height / 2);
        }

        private void JoinLobbyButton_Pressed(PressedEventArgs obj)
        {
            UIElement nameNode = UI.Root.GetChild("PlayerNameBox", true);
            LineEdit name = (LineEdit)nameNode;
            
            String myName = name.Text;
            var cache = ResourceCache;
            
            Player me = new Player(myName,100, null);  //JACK: Setup client/server interaction and information here (100 needs to be replaced with server "Buy In" amount and null, the socket)

            //Load Lobby Scene
            //LoadLobbyScene();

            //Load Playing Scene
            UI.Root.RemoveAllChildren();
            Node soundnode = scene.GetChild("Music", true);
            SoundSource sound = soundnode.GetComponent<SoundSource>(true);

            sound.Stop();
            scene.Clear(true, true);
            scene = LoadPlayerScene(me);

            PlayerViewport = new Viewport(Context, scene, camera, null);

            SetupViewport(PlayerViewport);

        }

        private void CreateLobbyButton_Pressed(PressedEventArgs obj)
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
            
            initTableCardPositions();

            gameStartTableAnimation();

            Room room = new Room();

            //TODO: Wait until players join to start game
            room.addPlayer(new Player("Luke", 1000, null));
            room.addPlayer(new Player("Jack", 1000, null));
            room.addPlayer(new Player("George", 1000, null));
            room.addPlayer(new Player("Xinyi", 1000, null));
            room.addPlayer(new Player("Rick", 1000, null));
            room.addPlayer(new Player("Mike", 1000, null));
            ///////

            var game = new PokerGame(room,scene,1000);

            game.run();
        }

        private void InfoButton_Pressed(PressedEventArgs obj)
        {
            //TODO: Add About box information
        }

        private void SetupViewport(Viewport vp)
        {
            Renderer.SetViewport(0, vp);
        }

        private void HostButton_Pressed(PressedEventArgs obj)
        {
            //Do host game stuff
            //TODO: Add intermediate host connection handling and setup
            LoadHostingScene();
        }

        private void JoinButton_Pressed(PressedEventArgs obj)
        {
            //Do join game stuff
            //TODO: Add intermediate join connection handling and setup
            LoadJoiningScene();
        }

        private void SettingsButton_Pressed(PressedEventArgs obj)
        {
            //Do settings stuff
            
        }
    }
}