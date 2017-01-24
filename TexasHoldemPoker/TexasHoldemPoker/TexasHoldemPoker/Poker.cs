using Urho;
using Urho.Gui;
using Urho.Actions;
using Urho.Audio;
using System;
using TexasHoldemPoker.Game.Utils;
using Urho.Resources;

namespace TexasHoldemPoker{
  public class Poker : Application{
    public Viewport MenuViewport { get; private set; }
        public Viewport PlayerViewport { get; private set; }

        //TODO: Comment or No Space
        public Poker() : base(new ApplicationOptions(assetsFolder: "Data")) { }
    //TODO: Comment or No Space - Consider for rest of code?
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
            LoadMenuUIFromXML();
      //LoadMenuUI();
      SetupViewport(MenuViewport);
            init();
    }

        private void LoadMenuUIFromXML()
        {
            XmlFile uiXML = ResourceCache.GetXmlFile("UI/MainMenu.xml");
            UI.Root.SetDefaultStyle(uiXML);
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

    private void LoadJoiningScene(){
      toggleMainMenuUI();
      UI.Root.GetChild("PlayerAvatar", true).Visible = true;
      UI.Root.GetChild("JoinLobbyButton", true).Visible = true;
      //Disabled until a valid server has been selected
    //UI.Root.GetChild("JoinLobbyButton", true).Enabled = false;
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

    private void toggleMainMenuUI(){
      UI.Root.GetChild("JoinGameButton", true).Visible = !UI.Root.GetChild("JoinGameButton", true).Visible;
      UI.Root.GetChild("HostGameButton", true).Visible = !UI.Root.GetChild("HostGameButton", true).Visible;
      UI.Root.GetChild("InfoButton", true).Visible = !UI.Root.GetChild("InfoButton", true).Visible;
      UI.Root.GetChild("GameLogo", true).Visible = !UI.Root.GetChild("GameLogo", true).Visible;
      UI.Root.GetChild("CopyrightNotice", true).Visible = !UI.Root.GetChild("CopyrightNotice", true).Visible;
    }

    private void LoadHostingScene(){
      toggleMainMenuUI();
      UI.Root.GetChild("CreateLobbyButton", true).Visible = true;
      //Disabled until all server options have been set
    //UI.Root.GetChild("CreateLobbyButton", true).Enabled = false;
      //Enabled for debugging
      UI.Root.GetChild("CreateLobbyButton", true).Enabled = true;
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

    private void BackButton_Pressed(PressedEventArgs obj){
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

    private void LoadMenuUI(){
      //TODO: Function has a mix of declaration styles
      //
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
    //TODO: Allow avatar selection
    private void PlayerAvatar_Pressed(PressedEventArgs obj){}
        //Next two functions almost identical.  Was about to move duplicate code
        //to another function, then wondered if any of the values needed can be
        //passed via the TextChangeedEventArg object? - GRT

        //Had an experiment - May be able to get stuff from OBJ, but it is 
        //okay for now, leave it until everything is working, then refactor - LKR

    private void LobbyNameBox_TextChanged(TextChangedEventArgs obj){
      var textElement = (Text)UI.Root.GetChild("LobbyNameText", true);
      var textNode = (LineEdit)UI.Root.GetChild("LobbyNameBox", true);
      if (textNode.Text.Length > 0){
        textElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 1f));
        textElement.Value = textNode.Text;
      } else{
        textElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.5f));
        textElement.Value = "Enter Lobby Name";
      }
      textElement.SetPosition((Graphics.Width / 2) - textElement.Width / 2,
                              textNode.Position.Y + textElement.Height / 2);
    }
    private void PlayerNameBox_TextChanged(TextChangedEventArgs obj){
      var textElement = (Text)UI.Root.GetChild("PlayerNameText", true);
      var textNode = (LineEdit)UI.Root.GetChild("PlayerNameBox", true);
      if (textNode.Text.Length > 0){
        textElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 1f));
        textElement.Value = textNode.Text;
      } else{
        textElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.5f));
        textElement.Value = "Enter Player Name";
      }
      textElement.SetPosition((Graphics.Width / 2) - textElement.Width / 2,
                              textNode.Position.Y + textElement.Height / 2);
    }
        
    private void JoinLobbyButton_Pressed(PressedEventArgs obj){
      UIElement nameNode = UI.Root.GetChild("PlayerNameBox", true);
      LineEdit name = (LineEdit)nameNode;

      String myName = name.Text;
      var cache = ResourceCache;


            UI.Root.RemoveAllChildren();
            Node soundnode = scene.GetChild("Music", true);
            SoundSource sound = soundnode.GetComponent<SoundSource>(true);
            sound.Stop();
            scene.Clear(true, true);

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

        var game = new PokerGame(room,scene, UI,1000);

        game.start();
    }

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
  }
}
