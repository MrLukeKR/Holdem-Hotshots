using System;
using System.Collections.Generic;
using Urho;
using Urho.Gui;
using Urho.Resources;

namespace HoldemHotshots
{
	public static class UIManager
	{
		static public Graphics graphics;
		static public ResourceCache cache;
		static public UI ui;

		//Menu UIs
		static public List<UIElement> menuUI { get; internal set; } = new List<UIElement>();
		static public List<UIElement> joinUI { get; internal set; } = new List<UIElement>();
		static public List<UIElement> hostUI { get; internal set; } = new List<UIElement>();

		//In-Game UIs
		static public List<UIElement> playerUI { get; internal set; } = new List<UIElement>();
		static public List<UIElement> tableUI { get; internal set; } = new List<UIElement>();

		static public void SetReferences(ResourceCache resCache, Graphics currGraphics, UI currUI) { cache = resCache; graphics = currGraphics; ui = currUI; }

		static private uint serverCount;

		static public void PopulateServerList(String serverName, uint currentPlayers, uint maxPlayers, uint buyIn)
		{
			if (serverCount >= 5)
				return; //TODO: Support scrolling to allow more that 5 game servers to be shown

			ListView serverList = null;

			foreach (var element in joinUI) { if (element.Name == "ServerList") serverList = (ListView)element; }

			if (serverList == null)
				return; //TODO: Throw and exception

			var position = (int)((serverList.Height / 5) * serverCount);

			var serverButton = new Button()
			{
				Size = new IntVector2(serverList.Width, serverList.Height / 5),
				HorizontalAlignment = HorizontalAlignment.Center,
				Position = new IntVector2(0, position)
			};

			System.Console.WriteLine(position);

			var sName = new Text()
			{
				Value = " " + serverName,
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Center
			};

			var sPlayers = new Text()
			{
				Value = currentPlayers + "/" + maxPlayers,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			};

			var sBuyIn = new Text()
			{
				Value = "Buy In: $" + buyIn + " ",
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Center
			};

			sName.SetFont(cache.GetFont("Fonts/arial.ttf"), 15);
			sPlayers.SetFont(cache.GetFont("Fonts/arial.ttf"), 15);
			sBuyIn.SetFont(cache.GetFont("Fonts/arial.ttf"), 15);

			if (currentPlayers == maxPlayers)
			{
				sPlayers.SetColor(new Color(1.0f, 0.0f, 0.0f, 1.0f));
				serverButton.SetColor(new Color(1.0f, 0.0f, 0.0f, 0.4f));
			}
			else
			{
				sPlayers.SetColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));
				serverButton.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.4f));
			}
			
			sName.SetColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));
			sBuyIn.SetColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));

			serverButton.AddChild(sName);
			serverButton.AddChild(sPlayers);
			serverButton.AddChild(sBuyIn);

			//TODO: Server button press handling
			serverList.AddItem(serverButton);
			serverCount++;
		}

		static public void CreateMenuUI()
		{
			if (menuUI.Count > 0)
				return;

			//Size parameters
			var logoWidthAndHeight = (graphics.Width / 5) * 3;

			var buttonWidth = graphics.Width / 8;
			var buttonHeight = graphics.Width / 3;

			//Create UI objects

			var gameLogo = new BorderImage()
			{
				Name = "GameLogo",
				Texture = cache.GetTexture2D("Textures/gameTitle.png"),
				BlendMode = BlendMode.Replace,
				Size = new IntVector2(logoWidthAndHeight, logoWidthAndHeight),
				Position = new IntVector2((graphics.Width / 2) - ((graphics.Width / 5) * 3) / 2, 0)
			};

			var joinButton = new Button()
			{
				Name = "JoinGameButton",
				Texture = cache.GetTexture2D("Textures/joinGameButton.png"),
				BlendMode = BlendMode.Replace,
				Size = new IntVector2(buttonHeight, buttonWidth),
				Position = new IntVector2((graphics.Width - buttonWidth) / 5, (graphics.Height / 6) * 5)
			};

			var hostButton = new Button()
			{
				Name = "HostGameButton",
				Texture = cache.GetTexture2D("Textures/hostGameButton.png"),
				BlendMode = BlendMode.Replace,
				Size = new IntVector2(buttonHeight, buttonWidth),
				Position = new IntVector2(((graphics.Width - buttonWidth) / 5) * 3, (graphics.Height / 6) * 5)
			};

			var copyrightNotice = new Text()
			{
				Name = "CopyrightNotice",
				Value = "Copyright Advantage Software Group 2016 ~ 2017. All Rights Reserved.",
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Bottom
			};

			copyrightNotice.SetColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));
			copyrightNotice.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);

			//Subscribe to Events

			joinButton.Pressed += JoinButton_Pressed;
			hostButton.Pressed += HostButton_Pressed;

			//Add to the MenuUI List
			menuUI.Add(gameLogo);
			menuUI.Add(joinButton);
			menuUI.Add(hostButton);
			menuUI.Add(copyrightNotice);


			AddToUI(menuUI);
		}

		private static void CreateJoinUI()
		{
			if (joinUI.Count > 0)
				return;

			//Size parameters
			var avatarWidthAndHeight = graphics.Width / 3;
			var backButtonWidthAndHeight = graphics.Width / 10;
			var nameBoxHeight = (graphics.Height / 20);
			var nameBoxWidth = (graphics.Width / 3) * 2;

			//Create UI objects
			var joinBackButton = new Button()
			{
				Name = "JoinBackButton",
				Texture = cache.GetTexture2D("Textures/backButton.png"),
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
				Position = new IntVector2(0, graphics.Height / 8),
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
				MaxLength = 15,
				Opacity = 0.6f
			};

			var serverList = new ListView()
			{
				Name = "ServerList",
				Size = new IntVector2((graphics.Width / 3) * 2, graphics.Height / 4),
				Position = new IntVector2(0, (graphics.Height / 15) * 7),
				HorizontalAlignment = HorizontalAlignment.Center,
				Visible = false,
				Enabled = false,
				Opacity = 0.6f
			};

			var joinLobbyButton = new Button()
			{
				Name = "JoinLobbyButton",
				Texture = cache.GetTexture2D("Textures/joinLobbyButton.png"),
				BlendMode = BlendMode.Replace,
				Size = new IntVector2((graphics.Width / 3) * 2, graphics.Width / 5),
				Position = new IntVector2(0, (graphics.Height / 6) * 5),
				HorizontalAlignment = HorizontalAlignment.Center
			};

			//PlayerNameBox TextElement properties
			playerNameBox.TextElement.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
			playerNameBox.TextElement.Value = "Enter Player Name";
			playerNameBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
			playerNameBox.TextElement.HorizontalAlignment = HorizontalAlignment.Center;
			playerNameBox.TextElement.VerticalAlignment = VerticalAlignment.Center;

			//Subscribe to Events
			joinBackButton.Pressed += JoinBackButton_Pressed;
			playerAvatar.Pressed += PlayerAvatar_Pressed;
			playerNameBox.TextChanged += PlayerNameBox_TextChanged;
			joinLobbyButton.Pressed += JoinLobbyButton_Pressed;

			//Add to the HostUI List
			joinUI.Add(joinBackButton);
			joinUI.Add(playerAvatar);
			joinUI.Add(playerNameBox);
			joinUI.Add(serverList);
			joinUI.Add(joinLobbyButton);

			AddToUI(joinUI);

			PopulateServerList("Luke's Room", 4, 5 , 5);
			PopulateServerList("Jack's Room", 5, 5 , 10);
			PopulateServerList("Xinyi's Room", 1, 4, 25);
			PopulateServerList("George's Room", 2, 6, 15);
			PopulateServerList("Mike's Room", 3, 3, 25);
			PopulateServerList("Rick's Room", 0, 5, 100);
		}

		static private void CreateHostUI()
		{
			if (hostUI.Count > 0)
				return;

			//Size parameters
			var hostButtonWidth = (graphics.Width / 3) * 2;
			var hostButtonHeight = graphics.Width / 5;
			var lobbyBoxWidth = (graphics.Width / 3) *  2;
			var lobbyBoxHeight = graphics.Height / 20;

			//Create UI objects
			var lobbyNameBox = new LineEdit()
			{
				Name = "LobbyNameBox",
				Size = new IntVector2(lobbyBoxWidth, lobbyBoxHeight),
				Position = new IntVector2(0, graphics.Height / 7),
				HorizontalAlignment = HorizontalAlignment.Center,
				Editable = true,
				Opacity = 0.6f,
				MaxLength = 15
			};

			var createLobbyButton = new Button()
			{
				Name = "CreateLobbyButton",
				Texture = cache.GetTexture2D("Textures/createLobbyButton.png"),
				BlendMode = BlendMode.Replace,
				Size = new IntVector2(hostButtonWidth, hostButtonHeight),
				Position = new IntVector2(0, (graphics.Height / 4) * 3),
				HorizontalAlignment = HorizontalAlignment.Center
			};

			//LobbyBox TextElement properties
			lobbyNameBox.TextElement.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
			lobbyNameBox.TextElement.Value = "Enter Lobby Name";
			lobbyNameBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
			lobbyNameBox.TextElement.HorizontalAlignment = HorizontalAlignment.Center;
			lobbyNameBox.TextElement.VerticalAlignment = VerticalAlignment.Center;

			//Subscribe to Events
			lobbyNameBox.TextChanged += LobbyNameBox_TextChanged;
			createLobbyButton.Pressed += CreateLobbyButton_Pressed;

			hostUI.Add(lobbyNameBox);
			hostUI.Add(createLobbyButton);

			AddToUI(hostUI);
		}

		static void JoinButton_Pressed(PressedEventArgs obj) { if (joinUI.Count == 0) CreateJoinUI(); UIUtils.SwitchUI(menuUI, joinUI); }
		static void HostButton_Pressed(PressedEventArgs obj) { if (hostUI.Count == 0) CreateHostUI(); UIUtils.SwitchUI(menuUI, hostUI); }

		static void PlayerAvatar_Pressed(PressedEventArgs obj)
		{

		}

		static void JoinBackButton_Pressed(PressedEventArgs obj)
		{
			UIUtils.SwitchUI(joinUI, menuUI);
		}

		static void PlayerNameBox_TextChanged(TextChangedEventArgs obj)
		{
			LineEdit textBox = null;

			foreach (var element in joinUI)
				if (element.Name == "PlayerNameBox") { textBox = (LineEdit)element; break; }

			if (textBox == null) return;

			if (textBox.Text.Length > 0) { textBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 1.0f)); }
			else
			{
				textBox.TextElement.Value = "Enter Player Name";
				textBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
			}
		}

		static void LobbyNameBox_TextChanged(TextChangedEventArgs obj)
		{
			LineEdit textBox = null;

			foreach (var element in hostUI)
				if (element.Name == "LobbyNameBox") { textBox = (LineEdit)element; break; }

			if (textBox == null) return;

			if (textBox.Text.Length > 0) { textBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 1.0f)); }
			else
			{
				textBox.TextElement.Value = "Enter Lobby Name";
				textBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
			}
		}

		static void JoinLobbyButton_Pressed(PressedEventArgs obj)
		{
			SceneManager.CreatePlayScene();

		}

		static void CreateLobbyButton_Pressed(PressedEventArgs obj)
		{

		}

		static public void AddToUI(List<UIElement> elements)
		{
			foreach (var element in elements) { ui.Root.AddChild(element); }
		}
	}
}
