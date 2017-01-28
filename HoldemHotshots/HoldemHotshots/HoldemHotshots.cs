using System.Collections.Generic;
using System.Diagnostics;
using Urho;
using Urho.Gui;

namespace HoldemHotshots
{
	public class HoldemHotshots : Application
	{
		//Scenes
		Scene menuScene;
		Scene playScene;
		Scene hostScene;

		//Menu UIs
		List<UIElement> menuUI = new List<UIElement>();
		List<UIElement> joinUI = new List<UIElement>();
		List<UIElement> hostUI = new List<UIElement>();

		//In-Game UIs
		List<UIElement> playerUI = new List<UIElement>();
		List<UIElement> tableUI  = new List<UIElement>();

		public Viewport Viewport { get; private set;}

		[Preserve]
		public HoldemHotshots() : base(new ApplicationOptions(assetsFolder: "Data") { Height = 1024, Width = 576, Orientation = ApplicationOptions.OrientationType.Portrait}) { }

		[Preserve]
		public HoldemHotshots(ApplicationOptions opts) : base(opts) { }

		static HoldemHotshots()
		{
			UnhandledException += (s, e) =>
			{
				if (Debugger.IsAttached)
					Debugger.Break();
				e.Handled = true;
			};
		}

		protected override void Start()
		{
			base.Start();
			CreateMenuScene();
			CreateMenuUI();

			ShowMenuScene();
			UIUtils.ShowUI(menuUI);
		}

		private void CreateMenuScene()
		{
			menuScene = new Scene();
			menuScene.CreateComponent<Octree>();

			var cameraNode = menuScene.CreateChild();
			cameraNode.Name = "MainCamera";
			cameraNode.Position = (new Vector3(0.0f,0.0f,-10.0f));
			cameraNode.CreateComponent<Camera>();
		}

		private void CreateMenuUI()
		{
			//Size parameters
			var logoWidthAndHeight = (Graphics.Width / 5) * 3;

			var buttonWidth = Graphics.Width / 8;
			var buttonHeight = Graphics.Width / 3;

			//Create UI objects

			var gameLogo = new BorderImage()
			{
				Name = "GameLogo",
				Texture = ResourceCache.GetTexture2D("Textures/gameTitle.png"),
				BlendMode = BlendMode.Replace,
				Size = new IntVector2(logoWidthAndHeight, logoWidthAndHeight),
				Position = new IntVector2((Graphics.Width / 2) - ((Graphics.Width / 5) * 3) / 2, 0)
			};

			var joinButton = new Button()
			{
				Name = "JoinGameButton",
				Texture = ResourceCache.GetTexture2D("Textures/joinGameButton.png"),
				BlendMode = BlendMode.Replace,
				Size = new IntVector2(buttonHeight, buttonWidth),
				Position = new IntVector2((Graphics.Width - buttonWidth) / 5, (Graphics.Height / 6) * 5)
			};

			var hostButton = new Button()
			{
				Name = "HostGameButton",
				Texture = ResourceCache.GetTexture2D("Textures/hostGameButton.png"),
				BlendMode = BlendMode.Replace,
				Size = new IntVector2(buttonHeight, buttonWidth),
				Position = new IntVector2(((Graphics.Width - buttonWidth) / 5) * 3, (Graphics.Height / 6) * 5)
			};

			var copyrightNotice = new Text() { 
				Name = "CopyrightNotice", 
				Value="Copyright Advantage Software Group 2016 ~ 2017. All Rights Reserved.",
				HorizontalAlignment=HorizontalAlignment.Center,
				VerticalAlignment=VerticalAlignment.Bottom
			};

			copyrightNotice.SetColor(new Color(1.0f,1.0f,1.0f,1.0f));
			copyrightNotice.SetFont(ResourceCache.GetFont("Fonts/arial.ttf"),10);

			//Subscribe to Events

			joinButton.Pressed += JoinButton_Pressed;
			hostButton.Pressed += HostButton_Pressed;

			//Add to the UI

			UI.Root.AddChild(gameLogo);
			UI.Root.AddChild(joinButton);
			UI.Root.AddChild(hostButton);
			UI.Root.AddChild(copyrightNotice);

			//Add to the MenuUI List
			menuUI.Add(gameLogo);
			menuUI.Add(joinButton);
			menuUI.Add(hostButton);
			menuUI.Add(copyrightNotice);
		}

		private void CreateJoinUI()
		{
			//Size parameters
			var avatarWidthAndHeight = Graphics.Width / 3;
			var backButtonWidthAndHeight = Graphics.Width / 10;
			var nameBoxHeight = (Graphics.Height / 20);
			var nameBoxWidth = (Graphics.Width / 3) * 2;

			//Create UI objects
			var joinBackButton = new Button()
			{
				Name = "JoinBackButton",
				Texture = ResourceCache.GetTexture2D("Textures/backButton.png"),
				Size = new IntVector2(backButtonWidthAndHeight, backButtonWidthAndHeight),
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				Visible = false,
				Enabled = false
			};

			var playerAvatar = new Button()
			{
				Name = "PlayerAvatar",
				Size = new IntVector2(avatarWidthAndHeight, avatarWidthAndHeight),
				Position = new IntVector2(0, Graphics.Height / 8),
				HorizontalAlignment = HorizontalAlignment.Center,
				Visible = false,
				Enabled = false,
				Opacity = 0.6f
			};

			var playerNameBox = new LineEdit()
			{
				Name = "PlayerNameBox",
				Size = new IntVector2(nameBoxWidth, nameBoxHeight),
				Position = new IntVector2(0, playerAvatar.Position.Y + playerAvatar.Height + nameBoxHeight / 2),
				HorizontalAlignment = HorizontalAlignment.Center,
				Editable = true,
				Visible = false,
				Enabled = false,
				MaxLength = 24,
				Opacity = 0.6f
			};

			//PlayerNameBox TextElement properties
			playerNameBox.TextElement.SetFont(ResourceCache.GetFont("Fonts/arial.ttf"), 20);
			playerNameBox.TextElement.Value = "Enter Player Name";
			playerNameBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
			playerNameBox.TextElement.HorizontalAlignment = HorizontalAlignment.Center;
			playerNameBox.TextElement.VerticalAlignment = VerticalAlignment.Center;

			//Subscribe to Events
			joinBackButton.Pressed += JoinBackButton_Pressed;
			playerAvatar.Pressed += PlayerAvatar_Pressed;
			playerNameBox.TextChanged += PlayerNameBox_TextChanged;

			//Add to the UI
			UI.Root.AddChild(joinBackButton);
			UI.Root.AddChild(playerAvatar);
			UI.Root.AddChild(playerNameBox);

			//Add to the HostUI List
			joinUI.Add(joinBackButton);
			joinUI.Add(playerAvatar);
			joinUI.Add(playerNameBox);
		}

		private void CreateHostUI()
		{
			
		}

		//Scene Management

		private void ShowMenuScene()
		{
			var cameraNode = menuScene.GetChild("MainCamera", true);

			Viewport = new Viewport(Context, menuScene, cameraNode.GetComponent<Camera>(), null);
			SetupRenderer();
		}

		private void SetupRenderer(){	Renderer.SetViewport(0, Viewport);}

		//Event Handlers

		void JoinButton_Pressed(PressedEventArgs obj) { if (joinUI.Count == 0) CreateJoinUI(); UIUtils.SwitchUI(menuUI, joinUI); }
		void HostButton_Pressed(PressedEventArgs obj) { if (hostUI.Count == 0) CreateHostUI(); UIUtils.SwitchUI(menuUI, hostUI); }

		void PlayerAvatar_Pressed(PressedEventArgs obj)
		{

		}

		void JoinBackButton_Pressed(PressedEventArgs obj)
		{
			UIUtils.SwitchUI(joinUI, menuUI);
		}

		void PlayerNameBox_TextChanged(TextChangedEventArgs obj)
		{
			var textBox = (LineEdit)UI.Root.GetChild("PlayerNameBox", true);

			if (textBox.Text.Length > 0) { textBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 1.0f)); }
			else 
			{
				textBox.TextElement.Value = "Enter Player Name";
				textBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
			}
		}
	}
}
