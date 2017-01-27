using Urho;
using Urho.Gui;
using Urho.Actions;
using Urho.Audio;
using System;
using TexasHoldemPoker.Game.Utils;
using Urho.Resources;

namespace TexasHoldemPoker{
    public class Poker : Application {
        public Viewport MenuViewport { get; private set; }
        public Viewport PlayerViewport { get; private set; }
        
        public Poker() : base(new ApplicationOptions(assetsFolder: "Data") {Height=1024, Width=576, Orientation = ApplicationOptions.OrientationType.Portrait} ) { }
        public Poker(ApplicationOptions opts) : base(opts) { }
    //TODO: Consider explicit declaration of scope?
        Scene scene;
        Camera camera;
        Node CameraNode;
        Node TargetNode;
        Vector3 initialCameraPos;

        private void init()
        {
            WorldNavigationUtils.graphics = Graphics;
            WorldNavigationUtils.ui = UI;
        }

    private void initPlayerCardPositions(){
      Card.card1DealingPos = new Vector3(-8.25f, 10f, 15f); //TODO: Make this relative to screen size
      Card.card1HoldingPos = new Vector3(-8.25f, 6f, 15f);
      Card.card1ViewingPos = WorldNavigationUtils.GetScreenToWorldPoint(
          (Graphics.Width / 2) + (Graphics.Width / 11),
          (Graphics.Height / 3), 
          17f, camera);
      Card.card2DealingPos = new Vector3(-8.75f, 10f, 15.05f);
      Card.card2HoldingPos = new Vector3(-8.75f, 5.75f, 15.05f);
      Card.card2ViewingPos = WorldNavigationUtils.GetScreenToWorldPoint((Graphics.Width / 2) -
        (Graphics.Width / 11), (Graphics.Height / 3), 17f, camera);
    }

    public void initTableCardPositions(){
            Card.cardTableDealingPos = WorldNavigationUtils.GetScreenToWorldPoint(0, Graphics.Height/2, 0.065f, camera);
            Card.card1TablePos = WorldNavigationUtils.GetScreenToWorldPoint((Graphics.Width / 2), (Graphics.Height / 2) -2, 0.065f, camera);
            Card.card1TablePos.Y += (1.4f * 0.009f) * 1.5f;
            Card.card1TablePos.X += 0.009f * 1.5f;
            Card.card2TablePos = WorldNavigationUtils.GetScreenToWorldPoint((Graphics.Width / 2), (Graphics.Height / 2) -1, 0.065f, camera);
            Card.card2TablePos.Y += (1.4f * 0.009f) * 1.5f;
            Card.card2TablePos.X += 0.009f * 1.5f;
            Card.card3TablePos = WorldNavigationUtils.GetScreenToWorldPoint((Graphics.Width / 2), (Graphics.Height / 2)   , 0.065f, camera);
            Card.card3TablePos.Y += (1.4f * 0.009f) * 1.5f;
            Card.card3TablePos.X += 0.009f * 1.5f;
            Card.card4TablePos = WorldNavigationUtils.GetScreenToWorldPoint((Graphics.Width / 2), (Graphics.Height / 2) +1, 0.065f, camera);
            Card.card4TablePos.Y += (1.4f * 0.009f) * 1.5f;
            Card.card4TablePos.X += 0.009f * 1.5f;
            Card.card5TablePos = WorldNavigationUtils.GetScreenToWorldPoint((Graphics.Width / 2), (Graphics.Height / 2) +2, 0.065f, camera);
            Card.card5TablePos.Y += (1.4f * 0.009f) * 1.5f;
            Card.card5TablePos.X += 0.009f * 1.5f;
        }

    protected override void Start(){
      base.Start();
      scene = LoadMenuScene();
      MenuViewport = new Viewport(Context, scene, CameraNode.GetComponent<Camera>(), null);
      LoadMenuUI();
      SetupViewport(MenuViewport);
      init();
    }

        private void LoadMenuUI()
        {
            //TODO: Function has a mix of declaration styles
            //
            var cache = ResourceCache;

            var copyrightNotice = new Text();
            var gameTitle = new BorderImage();
            var settingsButton = new Button();
            var joinButton = new Button();
            var hostButton = new Button();
            var createLobbyButton = new Button();
            var joinLobbyButton = new Button();

            var exitButton = new Button();

            var playerAvatar = new Button();
            var serverList = new ListView();
            var serverListLabel = new Text();

            var lobbyNameBox = new LineEdit();
            var playerNameBox = new LineEdit();

            Text coords = new Text();
            var statusInfoText = new Text();

            Text tableMessage = new Text();

            lobbyNameBox.Name = "LobbyNameBox";
            lobbyNameBox.SetSize((Graphics.Width / 3) * 2, Graphics.Height / 20);
            lobbyNameBox.SetPosition((Graphics.Width / 2) - lobbyNameBox.Width / 2, (Graphics.Height / 7));
            lobbyNameBox.Editable = true;
            lobbyNameBox.TextElement.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
            lobbyNameBox.TextElement.Value = "Enter Lobby Name";
            lobbyNameBox.TextElement.SetColor(new Color(0, 0, 0, 0.6f));
            lobbyNameBox.TextSelectable = true;
            lobbyNameBox.Visible = false;
            lobbyNameBox.MaxLength = 24;
            lobbyNameBox.Opacity = 0.6f;
            lobbyNameBox.TextElement.HorizontalAlignment = HorizontalAlignment.Center;
            lobbyNameBox.TextElement.VerticalAlignment = VerticalAlignment.Center;
            lobbyNameBox.TextChanged += LobbyNameBox_TextChanged;

            playerAvatar.Name = "PlayerAvatar";
            playerAvatar.SetSize(Graphics.Width / 3, Graphics.Width / 3);
            playerAvatar.SetPosition((Graphics.Width / 2) - (playerAvatar.Width / 2), (Graphics.Height / 8));
            playerAvatar.Visible = false;
            playerAvatar.Pressed += PlayerAvatar_Pressed;
            playerAvatar.Opacity = 0.6f;

            playerNameBox.Name = "PlayerNameBox";
            playerNameBox.SetSize((Graphics.Width / 3) * 2, Graphics.Height / 20);
            playerNameBox.SetPosition((Graphics.Width / 2) - lobbyNameBox.Width / 2, playerAvatar.Position.Y + playerAvatar.Height + lobbyNameBox.Height/2);
            playerNameBox.Editable = true;
            playerNameBox.TextElement.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
            playerNameBox.TextElement.Value = "Enter Player Name";
            playerNameBox.TextElement.SetColor(new Color(0, 0, 0, 0.6f));
            playerNameBox.TextSelectable = true;
            playerNameBox.Visible = false;
            playerNameBox.MaxLength = 24;
            playerNameBox.Opacity = 0.6f;
            playerNameBox.TextElement.HorizontalAlignment = HorizontalAlignment.Center;
            playerNameBox.TextElement.VerticalAlignment = VerticalAlignment.Center;
            playerNameBox.TextChanged += PlayerNameBox_TextChanged;

            serverList.Name = "ServerList";
            serverList.SetSize((Graphics.Width / 3) * 2, (Graphics.Height / 4));
            serverList.SetPosition((Graphics.Width / 2) - serverList.Width / 2, (Graphics.Height / 15) * 8);
            serverList.Visible = false;
            serverList.Opacity = 0.6f;

            exitButton.Texture = cache.GetTexture2D("Textures/exitButton.png"); // Set texture
            exitButton.BlendMode = BlendMode.Replace;
            exitButton.SetSize(50, 50);
            exitButton.VerticalAlignment = VerticalAlignment.Top;
            exitButton.HorizontalAlignment = HorizontalAlignment.Right;
            exitButton.Name = "ExitButton";
            exitButton.Pressed += ExitButton_Pressed;
            exitButton.Visible = false;

            copyrightNotice.Name = "CopyrightNotice";
            copyrightNotice.Value = "Copyright © Advantage Software Group 2016 - 2017. All Rights Reserved.";
            copyrightNotice.HorizontalAlignment = HorizontalAlignment.Center;
            copyrightNotice.VerticalAlignment = VerticalAlignment.Bottom;
            copyrightNotice.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            copyrightNotice.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);

            gameTitle.Name = "GameLogo";
            gameTitle.Texture = cache.GetTexture2D("Textures/gameTitle.png");
            gameTitle.BlendMode = BlendMode.Replace;
            gameTitle.SetSize((Graphics.Width / 5) * 3, (Graphics.Width / 5) * 3);
            gameTitle.SetPosition((Graphics.Width / 2) - (gameTitle.Width / 2), 0);

            settingsButton.Texture = cache.GetTexture2D("Textures/settingsButton.png"); // Set texture
            settingsButton.BlendMode = BlendMode.Replace;
            settingsButton.SetSize(Graphics.Width / 10, Graphics.Width / 10);
            settingsButton.SetPosition(Graphics.Width - settingsButton.Width - 20, 20);
            settingsButton.Name = "SettingsButton";
            settingsButton.Pressed += SettingsButton_Pressed;

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

            coords.Name = "coords";
            coords.SetColor(new Color(1.0f, 1.0f, 1.0f, 1f));
            coords.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
            coords.Value = "";
            coords.VerticalAlignment = VerticalAlignment.Center;
            coords.HorizontalAlignment = HorizontalAlignment.Center;
            coords.Visible = true;

            statusInfoText.Name = "StatusInformationLabel";
            statusInfoText.SetColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));
            statusInfoText.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
            statusInfoText.HorizontalAlignment = HorizontalAlignment.Center;
            statusInfoText.VerticalAlignment = VerticalAlignment.Top;
            statusInfoText.SetPosition(0, statusInfoText.Height / 2);
            statusInfoText.Visible = false;
         

            createLobbyButton.Texture = cache.GetTexture2D("Textures/createLobbyButton.png"); // Set texture
            createLobbyButton.BlendMode = BlendMode.Replace;
            createLobbyButton.SetSize((Graphics.Width / 3) * 2, Graphics.Width / 5);
            createLobbyButton.SetPosition(Graphics.Width / 2 - createLobbyButton.Width / 2, (Graphics.Height / 4) * 3);
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

            tableMessage.Name = "TableMessage";
            tableMessage.SetFont(cache.GetFont("Fonts/arial.ttf"), 30);
            tableMessage.SetColor(new Color(1f, 1f, 1f, 1f));
            tableMessage.HorizontalAlignment = HorizontalAlignment.Center;
            tableMessage.VerticalAlignment = VerticalAlignment.Top;
            tableMessage.UseDerivedOpacity = false;
            tableMessage.Visible = false;
        
            UI.Root.AddChild(gameTitle);
            UI.Root.AddChild(copyrightNotice);
            UI.Root.AddChild(joinButton);
            UI.Root.AddChild(hostButton);
            UI.Root.AddChild(settingsButton);
          
            UI.Root.AddChild(backButton);
            
            UI.Root.AddChild(createLobbyButton);
            UI.Root.AddChild(joinLobbyButton);

            UI.Root.AddChild(playerAvatar);
            UI.Root.AddChild(playerNameBox);
            UI.Root.AddChild(serverList);

            UI.Root.AddChild(lobbyNameBox);

            UI.Root.AddChild(coords);
            UI.Root.AddChild(statusInfoText);

            UI.Root.AddChild(exitButton);

            UI.Root.AddChild(tableMessage);
        }

        private void LobbyNameBox_TextChanged(TextChangedEventArgs obj)
        {
            var textNode = (LineEdit)UI.Root.GetChild("LobbyNameBox", true);
            if (textNode.Text.Length > 0)
            {
                textNode.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 1f));
            }
            else
            {
                textNode.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.5f));
                textNode.TextElement.Value = "Enter Lobby Name";
            }
        }

        private Scene LoadMenuScene(){
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
      //Figure out a way of playing this on devices that can handle it
      rotateCamera(TargetNode);

      return menuScene;
    }

   private Scene LoadTableScene(){
      var cache = ResourceCache;
      Scene tableScene = new Scene();
      tableScene.LoadXmlFromCache(cache, "Scenes/Table.xml");
            Node musicNode = tableScene.CreateChild("SFX");
            SoundSource musicSource = musicNode.CreateComponent<SoundSource>();
            //TODO: Make the camera update when the scene is changed (EVENT)
            CameraNode = tableScene.GetChild("MainCamera", true);
      camera = CameraNode.GetComponent<Camera>();
      return tableScene;
    }

    private void LoadJoiningScene(){
      toggleMainMenuUI();
      UI.Root.GetChild("PlayerAvatar", true).Visible = true;
      UI.Root.GetChild("JoinLobbyButton", true).Visible = true;
      //Disabled until a valid server has been selected
    //UI.Root.GetChild("JoinLobbyButton", true).Enabled = false;
      UI.Root.GetChild("JoinLobbyButton", true).Enabled = true; //Enabled for debugging purposes
      UI.Root.GetChild("BackButton", true).Visible = true;
      UI.Root.GetChild("PlayerNameBox", true).Visible = true;
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

    private void toggleMainMenuUI(){
      UI.Root.GetChild("JoinGameButton", true).Visible = !UI.Root.GetChild("JoinGameButton", true).Visible;
      UI.Root.GetChild("HostGameButton", true).Visible = !UI.Root.GetChild("HostGameButton", true).Visible;
      UI.Root.GetChild("GameLogo", true).Visible = !UI.Root.GetChild("GameLogo", true).Visible;
      UI.Root.GetChild("CopyrightNotice", true).Visible = !UI.Root.GetChild("CopyrightNotice", true).Visible;
            UI.Root.GetChild("SettingsButton", true).Visible = !UI.Root.GetChild("SettingsButton", true).Visible;

        }

    private void LoadHostingScene(){
      toggleMainMenuUI();
      UI.Root.GetChild("CreateLobbyButton", true).Visible = true;
      UI.Root.GetChild("LobbyNameBox", true).Visible = true;
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
        private void hideSecondaryMenus()
        {
            UI.Root.GetChild("BackButton", true).Visible = false;
            UI.Root.GetChild("CreateLobbyButton", true).Visible = false;
            UI.Root.GetChild("JoinLobbyButton", true).Visible = false;
            UI.Root.GetChild("PlayerAvatar", true).Visible = false;
            UI.Root.GetChild("PlayerNameBox", true).Visible = false;
            UI.Root.GetChild("ServerList", true).Visible = false;
            UI.Root.GetChild("LobbyNameBox", true).Visible = false;
        }

    private void BackButton_Pressed(PressedEventArgs obj){
      toggleMainMenuUI();
            hideSecondaryMenus();
      panToOriginalPosition();
      rotateCamera(TargetNode);
    }

    private void panToOriginalPosition(){
      CameraNode.RunActions(new Parallel(new MoveTo(1,initialCameraPos),
                                         new RotateTo(1, 20f,0f,0f)));
      CameraNode.LookAt(TargetNode.Position, Vector3.Up, TransformSpace.World);
    }

    private void panToHost(){
      CameraNode.RunActions(new Parallel(new MoveTo(1, new Vector3(0.00544398f, 0.176587f, 0.159439f)),
                                         new RotateTo(1, 60f, -180f, 0f)));
    }

    private void panToJoin(){
      CameraNode.RunActions(new Parallel(new MoveTo(1, new Vector3(0f, 0.106208f, -0.139909f)),
                                         new RotateTo(1, 20f, 0f, 0f)));
    }

    private async void rotateCamera(Node target){
        await CameraNode.RunActionsAsync(new RepeatForever(new RotateAroundBy(60, TargetNode.Position, 0.0f, 360.0f, 0.0f,
                                                           TransformSpace.World)));
    }

  
    //TODO: Allow avatar selection
    private void PlayerAvatar_Pressed(PressedEventArgs obj){}
        //Next two functions almost identical.  Was about to move duplicate code
        //to another function, then wondered if any of the values needed can be
        //passed via the TextChangeedEventArg object? - GRT

        //Had an experiment - May be able to get stuff from OBJ, but it is 
        //okay for now, leave it until everything is working, then refactor - LKR
        

    private void PlayerNameBox_TextChanged(TextChangedEventArgs obj){
            var textNode = (LineEdit)UI.Root.GetChild("PlayerNameBox", true);
            if (textNode.Text.Length > 0)
            {
                textNode.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 1f));
            }
            else
            {
                textNode.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.5f));
                textNode.TextElement.Value = "Enter Player Name";
            }
        }
        
    private void JoinLobbyButton_Pressed(PressedEventArgs obj){
      UIElement nameNode = UI.Root.GetChild("PlayerNameBox", true);
      LineEdit name = (LineEdit)nameNode;

      String myName = name.Text;
      var cache = ResourceCache;

            hideSecondaryMenus();

            Node soundnode = scene.GetChild("Music", true);
            SoundSource sound = soundnode.GetComponent<SoundSource>(true);
            sound.Stop();
            scene.UpdateEnabled = false;
            
            //JACK: Setup client/server interaction and information here (100 needs
            //to be replaced with server "Buy In" amount and null, the socket)
            Player me = new Player(myName,100, null);
            
            var playerScene = me.initPlayerScene(cache, UI, Current.Input);
            PlayerViewport = new Viewport(Context, playerScene, me.getCamera(), null);
            //Load Lobby Scene
            //LoadLobbyScene();

            //Load Playing Scene

            SetupViewport(PlayerViewport);
        }

    private void CreateLobbyButton_Pressed(PressedEventArgs obj){
        //Load Hosting Scene
        Node soundnode = scene.GetChild("Music", true);
        SoundSource sound = soundnode.GetComponent<SoundSource>(true);

        sound.Stop();

            hideSecondaryMenus();

        var hostingScene = LoadTableScene();

            PlayerViewport = new Viewport(Context, hostingScene, camera, null);
            
            SetupViewport(PlayerViewport);
            
            initTableCardPositions();
          
            Room room = new Room();

        //TODO: Wait until players join to start game
        room.addPlayer(new Player("Luke", 1000, null));
        room.addPlayer(new Player("Jack", 1000, null));
        room.addPlayer(new Player("George", 1000, null));
        room.addPlayer(new Player("Xinyi", 1000, null));
        room.addPlayer(new Player("Rick", 1000, null));
        room.addPlayer(new Player("Mike", 1000, null));
        ///////

        var game = new PokerGame(room,hostingScene, UI, ResourceCache, 1000);

        game.start();
    }

    //TODO: Add About box information
    private void InfoButton_Pressed(PressedEventArgs obj) { }
    private void SetupViewport(Viewport vp) { Renderer.SetViewport(0, vp);  }
    private void HostButton_Pressed(PressedEventArgs obj){
        //Do host game stuff
        //TODO: Add intermediate host connection handling and setup
        LoadHostingScene();
    }
    private void JoinButton_Pressed(PressedEventArgs obj){
        //Do join game stuff
        //TODO: Add intermediate join connection handling and setup
        LoadJoiningScene();
    }
    //Do settings stuff
    private void SettingsButton_Pressed(PressedEventArgs obj) { }

        private void restartMainMenu()
        {
            Node soundnode = scene.GetChild("Music", true);
            SoundSource sound = soundnode.GetComponent<SoundSource>(true); 
            sound.Play(ResourceCache.GetSound("Music/MenuBGM.wav"));
            
            PlayerViewport = new Viewport(Context, scene, camera, null);
            SetupViewport(PlayerViewport);
            panToOriginalPosition();
            rotateCamera(TargetNode);
            toggleMainMenuUI();
        }

        private void ExitButton_Pressed(PressedEventArgs obj)
        {
            restartMainMenu();

            UI.Root.GetChild("TableMessage", true).Visible = false;
            UI.Root.GetChild("ExitButton", true).Visible = false;
            UI.Root.GetChild("StatusInformationLabel", true).Visible = false;

            //TODO: Extra exit handling code
        }
    }

}
