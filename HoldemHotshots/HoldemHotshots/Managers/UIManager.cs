using System.Collections.Generic;
using Urho;
using Urho.Gui;
using Urho.Resources;
using HoldemHotshots.Utilities;

namespace HoldemHotshots.Managers
{
	public static class UIManager
	{
        public const int QR_STRING_LENGTH = 21;

        static public Graphics graphics    { get; private set; }
		static public ResourceCache cache  { get; private set; }
		static public UI ui                { get; private set; }

		//Menu UIs
		static readonly public List<UIElement> menuUI           = new List<UIElement>();
		static readonly public List<UIElement> joinUI           = new List<UIElement>();
        static readonly public List<UIElement> lobbyUI          = new List<UIElement>();
        static readonly public List<UIElement> settingsUI       = new List<UIElement>();

        //In-Game UIs
        static readonly public List<UIElement> playerUI         = new List<UIElement>();
        static readonly public List<UIElement> playerUI_raise   = new List<UIElement>();
        static readonly public List<UIElement> tableUI          = new List<UIElement>();

		static public void SetReferences(ResourceCache resCache, Graphics currGraphics, UI currUI)
        {
            cache = resCache; graphics = currGraphics; ui = currUI;
        }

		static public void CreateMenuUI()
		{
			if (menuUI.Count > 0)
				return;

			//Size parameters
			var logoWidthAndHeight = (graphics.Width / 5) * 3;
			var buttonWidth = graphics.Width / 8;
			var buttonHeight = graphics.Width / 3;
			var settingsButtonWidthAndHeight = graphics.Width / 10;

            //Create UI objects
            var menuBackground = new Window()
            {
                Name = "MenuBackground",
                Texture = cache.GetTexture2D("Textures/Backgrounds/menuBackground.png"),
                Size = new IntVector2(graphics.Width, graphics.Height),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                ImageRect = new IntRect(0, 0, graphics.Width, graphics.Height)
            };

			var settingsButton = new Button()
			{
				Name = "SettingsButton",
				Texture = cache.GetTexture2D("Textures/settingsButton.png"),
				Size = new IntVector2(settingsButtonWidthAndHeight, settingsButtonWidthAndHeight),
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Top
			};

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
			settingsButton.Pressed  += InputManager.SettingsButton_Pressed;
			joinButton.Pressed      += InputManager.JoinButton_Pressed;
			hostButton.Pressed      += InputManager.HostButton_Pressed;

            //Add to the MenuUI List
            menuBackground.AddChild(settingsButton);
            menuBackground.AddChild(gameLogo);
            menuBackground.AddChild(joinButton);
            menuBackground.AddChild(hostButton);
            menuBackground.AddChild(copyrightNotice);

            menuUI.Add(menuBackground);

            UIUtils.AddToUI(menuUI);
		}

		public static void CreateJoinUI()
		{
			if (joinUI.Count > 0)
				return;

			//Size parameters
			var qrWidthAndHeight = graphics.Width / 2;
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

			var qrCodeButton = new Button()
			{
				Name = "ClientQRCode",
				Size = new IntVector2(qrWidthAndHeight, qrWidthAndHeight),
				Position = new IntVector2(0, graphics.Height / 8),
				HorizontalAlignment = HorizontalAlignment.Center,
				Visible = false,
				Enabled = false
			};

			var playerNameBox = new LineEdit()
			{
				Name = "PlayerNameBox",
				Size = new IntVector2(nameBoxWidth, nameBoxHeight),
				Position = new IntVector2(0, qrCodeButton.Position.Y + qrCodeButton.Height + nameBoxHeight / 2),
				HorizontalAlignment = HorizontalAlignment.Center,
				Editable = true,
				Visible = false,
				Enabled = false,
				MaxLength = 15,
				Opacity = 0.6f
			};

			var serverAddressBox = new LineEdit()
			{
				Name = "ServerAddressBox",
				Size = new IntVector2(nameBoxWidth, nameBoxHeight),
				Position = new IntVector2(0, playerNameBox.Position.Y + playerNameBox.Height + nameBoxHeight / 2),
                HorizontalAlignment = HorizontalAlignment.Center,
                Editable = true,
				Opacity = 0.6f,
				MaxLength = QR_STRING_LENGTH
			};

			//ServerAddressBox TextElement properties
			serverAddressBox.TextElement.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
			serverAddressBox.TextElement.Value = "Enter Server IP Address";
			serverAddressBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
			serverAddressBox.TextElement.HorizontalAlignment = HorizontalAlignment.Center;
			serverAddressBox.TextElement.VerticalAlignment = VerticalAlignment.Center;

            var serverPortBox = new LineEdit()
            {
                Name = "ServerPortBox",
                Size = new IntVector2(nameBoxWidth, nameBoxHeight),
                Position = new IntVector2(0, serverAddressBox.Position.Y + nameBoxHeight + nameBoxHeight / 2),
                HorizontalAlignment = HorizontalAlignment.Center,
                Editable = true,
                Opacity = 0.6f,
                MaxLength = 5
            };

            //ServerAddressBox TextElement properties
            serverPortBox.TextElement.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
            serverPortBox.TextElement.Value = "Enter Server IP Port";
            serverPortBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
            serverPortBox.TextElement.HorizontalAlignment = HorizontalAlignment.Center;
            serverPortBox.TextElement.VerticalAlignment = VerticalAlignment.Center;

            var scanQRButton = new Button()
            {
                Name = "ScanQRButton",
                Texture = cache.GetTexture2D("Textures/scanQRButton.png"),
                BlendMode = BlendMode.Replace,
                Size = new IntVector2((graphics.Width / 6) , graphics.Width / 6),
                Position = new IntVector2(0, serverPortBox.Position.Y + nameBoxHeight + nameBoxHeight / 2),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var joinLobbyButton = new Button()
            {
                Name = "JoinLobbyButton",
                Texture = cache.GetTexture2D("Textures/joinLobbyButton.png"),
                BlendMode = BlendMode.Replace,
                Size = new IntVector2((graphics.Width / 3) * 2, graphics.Width / 5),
                Position = new IntVector2(0, (graphics.Height / 6) * 5),
                HorizontalAlignment = HorizontalAlignment.Center,
                Enabled = false,
                Opacity = 0.2f
			};

			//PlayerNameBox TextElement properties
			playerNameBox.TextElement.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
			playerNameBox.TextElement.Value = "Enter Player Name";
			playerNameBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
			playerNameBox.TextElement.HorizontalAlignment = HorizontalAlignment.Center;
			playerNameBox.TextElement.VerticalAlignment = VerticalAlignment.Center;

			//Subscribe to Events
			joinBackButton.Pressed          += InputManager.JoinBackButton_Pressed;
			playerNameBox.TextChanged       += InputManager.PlayerNameBox_TextChanged;
			serverAddressBox.TextChanged    += InputManager.ServerAddressBox_TextChanged;
            serverPortBox.TextChanged       += InputManager.ServerPortBox_TextChanged;
            scanQRButton.Pressed            += InputManager.ScanQRButton_Pressed;
			joinLobbyButton.Pressed         += InputManager.JoinLobbyButton_Pressed;
            qrCodeButton.Pressed            += InputManager.QrCodeButton_Pressed;

			//Add to the HostUI List           
			joinUI.Add(joinBackButton);
			joinUI.Add(qrCodeButton);
			joinUI.Add(playerNameBox);
			joinUI.Add(serverAddressBox);
            joinUI.Add(serverPortBox);
            joinUI.Add(scanQRButton);
			joinUI.Add(joinLobbyButton);

            UIUtils.AddToUI(joinUI);
        }

        public static void CreateTableUI()
        {
            if (tableUI.Count > 0)
                return;

            var exitButtonWidthAndHeight = graphics.Width / 10;

            var tableExitButton = new Button()
            {
                Name = "TableExitButton",
                Texture = cache.GetTexture2D("Textures/exitButtonLandscape.png"),
                Size = new IntVector2(exitButtonWidthAndHeight, exitButtonWidthAndHeight),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Visible = false,
                Enabled = false
            };

            var gameRestartButton = new Button()
            {
                Name = "GameRestartButtonNoAutoLoad",
                Texture = cache.GetTexture2D("Textures/restartButton.png"),
                Size = new IntVector2(exitButtonWidthAndHeight, exitButtonWidthAndHeight),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Visible = false,
                Enabled = false
            };

            tableExitButton.Pressed += InputManager.TableExitButton_Pressed;
            gameRestartButton.Pressed += InputManager.GameRestartButton_Pressed;
            
            tableUI.Add(tableExitButton);
            tableUI.Add(gameRestartButton);

            UIUtils.AddToUI(tableUI);
        }
        
        public static void CreatePlayerUI()
        {
            if (playerUI.Count > 0)
                return;

            var exitButtonWidthAndHeight = graphics.Width / 10;
            var actionButtonWidthAndHeight = graphics.Height / 7;
            var fontSize = graphics.Height/25;
            
            var balanceText = new Text()
            {
                Name = "PlayerBalanceText",
                Value = "$0",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };

            balanceText.SetColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));
            balanceText.SetFont(cache.GetFont("Fonts/vladimir.ttf"), fontSize);

            var playerInfoText = new Text()
            {
                Name = "PlayerInfoText",
                TextAlignment = HorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };

            playerInfoText.SetColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));
            playerInfoText.SetFont(cache.GetFont("Fonts/vladimir.ttf"), fontSize);

            var playerExitButton = new Button()
            {
                Name = "PlayerExitButton",
                Texture = cache.GetTexture2D("Textures/exitButton.png"),
                Size = new IntVector2(exitButtonWidthAndHeight, exitButtonWidthAndHeight),
                HorizontalAlignment=HorizontalAlignment.Left,
                VerticalAlignment=VerticalAlignment.Top
            };

            var callButton = new Button()
            {
                Name = "CallButton",
                Texture = cache.GetTexture2D("Textures/ActionButtons/call.png"),
                Size = new IntVector2(actionButtonWidthAndHeight, actionButtonWidthAndHeight),
                Position = new IntVector2(0, graphics.Height - actionButtonWidthAndHeight),
                Enabled = false,
                Visible = false
            };

            var checkButton = new Button()
            {
                Name = "CheckButton",
                Texture = cache.GetTexture2D("Textures/ActionButtons/check.png"),
                Size = new IntVector2(actionButtonWidthAndHeight, actionButtonWidthAndHeight),
                Position = new IntVector2(0, graphics.Height - actionButtonWidthAndHeight),
                Enabled = false,
                Visible = false
            };

            var raiseButton = new Button()
            {
                Name = "RaiseButton",
                Texture = cache.GetTexture2D("Textures/ActionButtons/raise.png"),
                Size = new IntVector2(actionButtonWidthAndHeight, actionButtonWidthAndHeight),
                Position = new IntVector2(0, callButton.Position.Y - actionButtonWidthAndHeight - actionButtonWidthAndHeight / 10),
                Enabled = false,
                Visible = false
            };

            var allInButton = new Button()
            {
                Name = "AllInButton",
                Texture = cache.GetTexture2D("Textures/ActionButtons/allIn.png"),
                Size = new IntVector2(actionButtonWidthAndHeight, actionButtonWidthAndHeight),
                Position = new IntVector2(0, raiseButton.Position.Y - actionButtonWidthAndHeight - actionButtonWidthAndHeight / 10),
                Enabled = false,
                Visible = false
            };
            
            var foldButton = new Button()
            {
                Name = "FoldButton",
                Texture = cache.GetTexture2D("Textures/ActionButtons/fold.png"),
                Size = new IntVector2(actionButtonWidthAndHeight, actionButtonWidthAndHeight),
                Position = new IntVector2(checkButton.Position.X * 2 + actionButtonWidthAndHeight + actionButtonWidthAndHeight / 10, callButton.Position.Y),
                Enabled = false,
                Visible = false
            };

            playerExitButton.Pressed    += InputManager.PlayerExitButton_Pressed;
            foldButton.Pressed          += InputManager.FoldButton_Pressed;
            checkButton.Pressed         += InputManager.CheckButton_Pressed;
            callButton.Pressed          += InputManager.CallButton_Pressed;
            raiseButton.Pressed         += InputManager.RaiseButton_Pressed;
            allInButton.Pressed         += InputManager.AllInButton_Pressed;

            playerUI.Add(foldButton);
            playerUI.Add(checkButton);
            playerUI.Add(callButton);
            playerUI.Add(raiseButton);
            playerUI.Add(allInButton);
            playerUI.Add(playerInfoText);
            playerUI.Add(balanceText);
            playerUI.Add(playerExitButton);

            UIUtils.AddToUI(playerUI);
        }

        public static void CreatePlayerRaiseUI()
        {
            if (playerUI_raise.Count > 0)
                return;

            var fontSize = graphics.Height / 20;
            var exitButtonWidthAndHeight = graphics.Width / 10;
            var actionButtonWidthAndHeight = graphics.Width / 5;
            var offset = graphics.Width / 10;

            var raiseExitButton = new Button()
            {
                Name = "ExitButton",
                Texture = cache.GetTexture2D("Textures/exitButton.png"),
                Size = new IntVector2(exitButtonWidthAndHeight, exitButtonWidthAndHeight),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            var currentBetText = new Text()
            {
                Name = "CurrentBetText",
                Value = "$1",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = HorizontalAlignment.Center
            };

            currentBetText.SetFont(cache.GetFont("Fonts/arial.ttf"), fontSize);
            currentBetText.SetColor(new Color(1,1,1,1));

            var increaseBetButton = new Button()
            {
                Name = "IncreaseBetButton",
                Texture = cache.GetTexture2D("Textures/ActionButtons/up.png"),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Size = new IntVector2(actionButtonWidthAndHeight, actionButtonWidthAndHeight),
                Position = new IntVector2(offset + actionButtonWidthAndHeight, -actionButtonWidthAndHeight - (actionButtonWidthAndHeight / 2))
            };

            var decreaseBetButton = new Button()
            {
                Name = "DecreaseBetButton",
                Texture = cache.GetTexture2D("Textures/ActionButtons/down.png"),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Size = new IntVector2(actionButtonWidthAndHeight, actionButtonWidthAndHeight),
                Position = new IntVector2(0, -actionButtonWidthAndHeight - (actionButtonWidthAndHeight / 2))
            };

            var raiseCancelButton = new Button()
            {
                Name = "CancelButton",
                Texture = cache.GetTexture2D("Textures/ActionButtons/no.png"),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Size = new IntVector2(actionButtonWidthAndHeight, actionButtonWidthAndHeight)
            };

            var raiseConfirmButton = new Button()
            {
                Name = "ConfirmButton",
                Texture = cache.GetTexture2D("Textures/ActionButtons/yes.png"),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Size = new IntVector2(actionButtonWidthAndHeight, actionButtonWidthAndHeight),
                Position = new IntVector2(offset + actionButtonWidthAndHeight, 0)
            };

            raiseExitButton.Pressed     += InputManager.RaiseExitButton_Pressed;
            raiseConfirmButton.Pressed  += InputManager.RaiseConfirmButton_Pressed;
            raiseCancelButton.Pressed   += InputManager.RaiseCancelButton_Pressed;
            decreaseBetButton.Pressed   += InputManager.DecreaseBetButton_Pressed;
            increaseBetButton.Pressed   += InputManager.IncreaseBetButton_Pressed;

            playerUI_raise.Add(raiseExitButton);
            playerUI_raise.Add(currentBetText);
            playerUI_raise.Add(increaseBetButton);
            playerUI_raise.Add(decreaseBetButton);
            playerUI_raise.Add(raiseCancelButton);
            playerUI_raise.Add(raiseConfirmButton);

            UIUtils.AddToUI(playerUI_raise);
        }

        public static void CreateSettingsUI()
        {
            if (settingsUI.Count > 0)
                return;

            var fontSize = graphics.Height / 30;
            var exitButtonWidthAndHeight = graphics.Width / 10;
            var handButtonSize = new IntVector2(graphics.Width / 5, graphics.Height / 10);

            var settingsExitButton = new Button()
            {
                Name = "SettingsExitButton",
                Texture = cache.GetTexture2D("Textures/exitButton.png"),
                Size = new IntVector2(exitButtonWidthAndHeight, exitButtonWidthAndHeight),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visible = false,
                Enabled = false
            };

            var preferredHandText = new Text()
            {
                Name = "PreferredHandText",
                TextAlignment = HorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Position = new IntVector2(0, graphics.Height / 5),
                Value = "Select your preferred hand"
            };

            preferredHandText.SetFont(cache.GetFont("Fonts/arial.ttf"), fontSize);
            preferredHandText.SetColor(new Color(1, 1, 1, 1));

            var leftHandToggleButton = new Button()
            {
                Name = "LeftHandToggleButton",
                Size = handButtonSize,
                Position = new IntVector2(graphics.Width/2 - handButtonSize.X - handButtonSize.X / 2, preferredHandText.Position.Y+ preferredHandText.Height+ preferredHandText.Height / 2)
            };

            var rightHandToggleButton = new Button()
            {
                Name = "RightHandToggleButton",
                Size = handButtonSize,
                Position = new IntVector2(graphics.Width / 2 + handButtonSize.X -handButtonSize.X /2, preferredHandText.Position.Y + preferredHandText.Height + preferredHandText.Height / 2)
            };

            settingsExitButton.Pressed += InputManager.SettingsExitButton_Pressed;

            settingsUI.Add(settingsExitButton);
            settingsUI.Add(preferredHandText);
            settingsUI.Add(leftHandToggleButton);
            settingsUI.Add(rightHandToggleButton);

            UIUtils.AddToUI(settingsUI);
        }
        
        public static void CreateLobbyUI()
        {
            if (lobbyUI.Count > 0)
                return;

            var lobbyBoxHeight = graphics.Height / 20;
            var qrScreenWidth = (graphics.Width / 5) * 3;
            var backButtonWidthAndHeight = graphics.Width / 10;
            var fontSize = graphics.Height / 25;
            var playerFontSize = fontSize / 2;

            var lobbyBackButton = new Button()
            {
                Name = "LobbyBackButton",
                Texture = cache.GetTexture2D("Textures/backButton.png"),
                Size = new IntVector2(backButtonWidthAndHeight, backButtonWidthAndHeight),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visible = false,
                Enabled = false
            };

            var addressQRCode = new BorderImage()
            {
                Name = "AddressQRCode",
                Position = new IntVector2(0,lobbyBackButton.Position.Y + lobbyBackButton.Height + lobbyBackButton.Height / 2),
                HorizontalAlignment = HorizontalAlignment.Center,
                Size = new IntVector2(qrScreenWidth, qrScreenWidth),
                Visible = false,
                Enabled = false
            };
            
            var addressText = new Text()
            {
                Name = "AddressText",
                Position = new IntVector2(0, addressQRCode.Position.Y + addressQRCode.Height + lobbyBoxHeight / 2),
                HorizontalAlignment = HorizontalAlignment.Center,
                Visible = false,
                Enabled = false
            };

            addressText.SetFont(cache.GetFont("Fonts/arial.ttf", true), 20);
            addressText.SetColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));

            var lobbyMessageText = new Text()
            {
                Name = "LobbyMessageText",
                Value = "Players in Room",
                TextAlignment = HorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Position = new IntVector2(0, addressText.Position.Y + graphics.Height / 20),
                Visible = false,
                Enabled = false
            };

            lobbyMessageText.SetColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));
            lobbyMessageText.SetFont(cache.GetFont("Fonts/vladimir.ttf"), fontSize);

            var playerNames = new Text()
            {
                Name = "PlayerNames",
                Value = "Waiting for Player 1...\nWaiting for Player 2...\nWaiting for Player 3...\nWaiting for Player 4...\nWaiting for Player 5...\nWaiting for Player 6...",
                TextAlignment = HorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Position = new IntVector2(0 , lobbyMessageText.Position.Y + lobbyMessageText.Height),
                Visible = false,
                Enabled = false
            };

            playerNames.SetFont("Fonts/arial.ttf", playerFontSize);
            playerNames.SetColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));
            
            var startGameButton = new Button()
            {
                Name = "StartGameButton",
                Texture = cache.GetTexture2D("Textures/startGameButton.png"),
                BlendMode = BlendMode.Replace,
                Size = new IntVector2((graphics.Width / 3) * 2, graphics.Width / 5),
                Position = new IntVector2(0, (graphics.Height / 6) * 5),
                HorizontalAlignment = HorizontalAlignment.Center,
                Visible = false,
                Enabled = false
            };

            lobbyMessageText.SetFont("Fonts/vladimir.ttf", fontSize);
            lobbyMessageText.SetColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));

            lobbyBackButton.Pressed += InputManager.LobbyBackButton_Pressed;
            startGameButton.Pressed += InputManager.StartGameButton_Pressed;

            lobbyUI.Add(lobbyBackButton);
            lobbyUI.Add(addressQRCode);
            lobbyUI.Add(addressText);
            lobbyUI.Add(playerNames);
            lobbyUI.Add(startGameButton);
            lobbyUI.Add(lobbyMessageText);

            UIUtils.AddToUI(lobbyUI);
        }     
	}
}