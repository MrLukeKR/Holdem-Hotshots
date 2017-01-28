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


			AddToUI(UIManager.menuUI);
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
				MaxLength = 24,
				Opacity = 0.6f
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

			//Add to the HostUI List
			joinUI.Add(joinBackButton);
			joinUI.Add(playerAvatar);
			joinUI.Add(playerNameBox);

			AddToUI(UIManager.joinUI);
		}

		static private void CreateHostUI()
		{
			if (hostUI.Count > 0)
				return;

			AddToUI(UIManager.hostUI);
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

			if (textBox.Text.Length > 0) { textBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 1.0f)); }
			else
			{
				textBox.TextElement.Value = "Enter Player Name";
				textBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
			}
		}

		static public void AddToUI(List<UIElement> elements)
		{
			foreach (var element in elements) { ui.Root.AddChild(element); }
		}
	}
}
