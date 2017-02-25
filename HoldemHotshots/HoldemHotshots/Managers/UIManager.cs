using System;
using System.Collections.Generic;
using System.IO;
#if __IOS__
using Foundation;
#endif
using Urho;
using Urho.Gui;
using Urho.Resources;
using Urho.Urho2D;
using ZXing.Mobile;
using System.Text.RegularExpressions;
using System.Threading;
using Urho.Audio;

namespace HoldemHotshots
{
	public static class UIManager
	{
        static private readonly uint QRStringLength = 21;
		static public Graphics graphics;
		static public ResourceCache cache;
		static public UI ui;

		//Menu UIs
		static public List<UIElement> menuUI     { get; internal set; } = new List<UIElement>();
		static public List<UIElement> joinUI     { get; internal set; } = new List<UIElement>();
        static public List<UIElement> lobbyUI    { get; internal set; } = new List<UIElement>();
        static public List<UIElement> settingsUI { get; internal set; } = new List<UIElement>();

        //In-Game UIs
        static public List<UIElement> playerUI { get; internal set; } = new List<UIElement>();
		static public List<UIElement> tableUI  { get; internal set; } = new List<UIElement>();

		static public void SetReferences(ResourceCache resCache, Graphics currGraphics, UI currUI) { cache = resCache; graphics = currGraphics; ui = currUI; }

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
                ImageRect = new IntRect(0,0,graphics.Width, graphics.Height)
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
			settingsButton.Pressed += SettingsButton_Pressed;
			joinButton.Pressed += JoinButton_Pressed;
			hostButton.Pressed += HostButton_Pressed;

            //Add to the MenuUI List

            menuBackground.AddChild(settingsButton);
            menuBackground.AddChild(gameLogo);
            menuBackground.AddChild(joinButton);
            menuBackground.AddChild(hostButton);
            menuBackground.AddChild(copyrightNotice);

            menuUI.Add(menuBackground);

            AddToUI(menuUI);
		}

		private static void CreateJoinUI()
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

			var qrCode = new BorderImage()
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
				Position = new IntVector2(0, qrCode.Position.Y + qrCode.Height + nameBoxHeight / 2),
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
				MaxLength = QRStringLength
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
                Opacity = 0.2f
			};

			//PlayerNameBox TextElement properties
			playerNameBox.TextElement.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
			playerNameBox.TextElement.Value = "Enter Player Name";
			playerNameBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
			playerNameBox.TextElement.HorizontalAlignment = HorizontalAlignment.Center;
			playerNameBox.TextElement.VerticalAlignment = VerticalAlignment.Center;

			//Subscribe to Events
			joinBackButton.Pressed += JoinBackButton_Pressed;
			playerNameBox.TextChanged += PlayerNameBox_TextChanged;
			serverAddressBox.TextChanged += ServerAddressBox_TextChanged;
            serverPortBox.TextChanged += ServerPortBox_TextChanged;
            scanQRButton.Pressed += ScanQRButton_Pressed;
			joinLobbyButton.Pressed += JoinLobbyButton_Pressed;

			//Add to the HostUI List           
			joinUI.Add(joinBackButton);
			joinUI.Add(qrCode);
			joinUI.Add(playerNameBox);
			joinUI.Add(serverAddressBox);
            joinUI.Add(serverPortBox);
            joinUI.Add(scanQRButton);
			joinUI.Add(joinLobbyButton);

			AddToUI(joinUI);
		}

        static void CreateTableUI()
        {
            if (tableUI.Count > 0) return;

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

            var potInfoText = new Text()
            {
                Name = "PotInfoText",
                Value = "Pot\n$0",
                TextAlignment = HorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left, //TODO: See if this UI can be made landscape
                VerticalAlignment = VerticalAlignment.Center,
                Visible = false,
                Enabled = false
            };

            potInfoText.SetColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));
            potInfoText.SetFont(cache.GetFont("Fonts/arial.ttf"), 30); //TODO: Make relative to screen size
            
            tableExitButton.Pressed += TableExitButton_Pressed;
            
            tableUI.Add(tableExitButton);
            tableUI.Add(potInfoText);

            AddToUI(tableUI);
        }

        
        static void CreatePlayerUI()
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

            var checkButton = new Button()
            {
                Name = "CheckButton",
                Texture = cache.GetTexture2D("Textures/ActionButtons/check.png"),
                Size = new IntVector2(actionButtonWidthAndHeight, actionButtonWidthAndHeight),
                Position = new IntVector2(actionButtonWidthAndHeight + actionButtonWidthAndHeight / 10, callButton.Position.Y),
                Enabled = false,
                Visible = false
            };
            
            
            var foldButton = new Button()
            {
                Name = "FoldButton",
                Texture = cache.GetTexture2D("Textures/ActionButtons/fold.png"),
                Size = new IntVector2(actionButtonWidthAndHeight, actionButtonWidthAndHeight),
                Position = new IntVector2(checkButton.Position.X + actionButtonWidthAndHeight + actionButtonWidthAndHeight / 10, callButton.Position.Y),
                Enabled = false,
                Visible = false
            };

            playerExitButton.Pressed += PlayerExitButton_Pressed;
            foldButton.Pressed += FoldButton_Pressed;
            checkButton.Pressed += CheckButton_Pressed;
            callButton.Pressed += CallButton_Pressed;
            raiseButton.Pressed += RaiseButton_Pressed;
            allInButton.Pressed += AllInButton_Pressed;

            playerUI.Add(foldButton);
            playerUI.Add(checkButton);
            playerUI.Add(callButton);
            playerUI.Add(raiseButton);
            playerUI.Add(allInButton);
            playerUI.Add(playerInfoText);
            playerUI.Add(balanceText);
            playerUI.Add(playerExitButton);

            AddToUI(playerUI);
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

            var dominantHandText = new Text()
            {
                Name = "DominantHandText",
                TextAlignment = HorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Position = new IntVector2(0, graphics.Height / 5),
                Value = "Select your dominant hand"
            };

            dominantHandText.SetFont(cache.GetFont("Fonts/arial.ttf"), fontSize);
            dominantHandText.SetColor(new Color(1, 1, 1, 1));

            var leftHandToggleButton = new Button()
            {
                Name = "LeftHandToggleButton",
                Size = handButtonSize,
                Position = new IntVector2(graphics.Width/2 - handButtonSize.X - handButtonSize.X / 2, dominantHandText.Position.Y+dominantHandText.Height+dominantHandText.Height / 2)
            };

            var rightHandToggleButton = new Button()
            {
                Name = "RightHandToggleButton",
                Size = handButtonSize,
                Position = new IntVector2(graphics.Width / 2 + handButtonSize.X -handButtonSize.X /2, dominantHandText.Position.Y + dominantHandText.Height + dominantHandText.Height / 2)
            };

            settingsExitButton.Pressed += SettingsExitButton_Pressed;

            settingsUI.Add(settingsExitButton);
            settingsUI.Add(dominantHandText);
            settingsUI.Add(leftHandToggleButton);
            settingsUI.Add(rightHandToggleButton);

            AddToUI(settingsUI);
        }
        
        public static void CreateLobbyUI()
        {
            if (lobbyUI.Count > 0)
            { 
                return;
            }

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

            lobbyBackButton.Pressed += LobbyBackButton_Pressed;
            startGameButton.Pressed += StartGameButton_Pressed;

            lobbyUI.Add(lobbyBackButton);
            lobbyUI.Add(addressQRCode);
            lobbyUI.Add(addressText);
            lobbyUI.Add(playerNames);
            lobbyUI.Add(startGameButton);
            lobbyUI.Add(lobbyMessageText);

            AddToUI(lobbyUI);
        }
        
        private static void AllInButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.DisplayPlayerMessage("All  In");
            ClientManager.session.player.allIn();
        }

        private static void RaiseButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.DisplayPlayerMessage("Raise");
            ClientManager.session.player.raise();
        }

        private static void CallButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.DisplayPlayerMessage("Call");
            ClientManager.session.player.call();
        }

        private static void CheckButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.DisplayPlayerMessage("Check");
            ClientManager.session.player.check();
        }

        private static void FoldButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.DisplayPlayerMessage("Fold");
            ClientManager.session.player.fold();
        }

        private static void PlayerExitButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.SwitchUI(playerUI, menuUI);
            SceneManager.ShowScene(SceneManager.menuScene);
        }
        
        private static void TableExitButton_Pressed(PressedEventArgs obj)
        {
            Session.DisposeOfSockets();

            UIUtils.SwitchUI(tableUI, menuUI);
            SceneManager.ShowScene(SceneManager.menuScene);
        }

        private static void SettingsExitButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.SwitchUI(settingsUI, menuUI);
        }


        static void JoinButton_Pressed(PressedEventArgs obj) 
		{
            if (joinUI.Count == 0) CreateJoinUI(); 
			UIUtils.SwitchUI(menuUI, joinUI);
		}

		static void HostButton_Pressed(PressedEventArgs obj) 
		{
            Session.getinstance().init();
            if (lobbyUI.Count == 0) CreateLobbyUI();
            UIUtils.SwitchUI(menuUI, lobbyUI);
		}

		static void SettingsButton_Pressed(PressedEventArgs obj)
		{
            CreateSettingsUI();
            UIUtils.SwitchUI(menuUI, settingsUI);
		}

		static void JoinBackButton_Pressed(PressedEventArgs obj) { UIUtils.SwitchUI(joinUI, menuUI); }
        static void LobbyBackButton_Pressed(PressedEventArgs obj) { UIUtils.SwitchUI(lobbyUI, menuUI); Session.DisposeOfSockets(); } //TODO: Move this when the "waiting in lobby" UI is implemented

        static void PlayerNameBox_TextChanged(TextChangedEventArgs obj) { AlterLineEdit("PlayerNameBox", "Enter Player Name", joinUI); }
		static void ServerAddressBox_TextChanged(TextChangedEventArgs obj) { AlterLineEdit("ServerAddressBox", "Enter Server IP Address", joinUI); }
        static void ServerPortBox_TextChanged(TextChangedEventArgs obj) { AlterLineEdit("ServerPortBox", "Enter Server IP Port", joinUI); }

        static private void AlterLineEdit(String boxName, String emptyText, List<UIElement> uiCollection)
		{
			LineEdit textBox = null;

			foreach (var element in uiCollection)
				if (element.Name == boxName) { textBox = (LineEdit)element; break; }

			if (textBox == null) return;

			if (textBox.Text.Length > 0) { textBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 1.0f)); }
			else
			{
				textBox.TextElement.Value = emptyText;
				textBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
			}
		}

        static private void AlterNumericLineEdit(String boxName, String emptyText, List<UIElement> uiCollection) {
            LineEdit textBox = null;

            foreach (var element in uiCollection)
                if (element.Name == boxName) { textBox = (LineEdit)element; break; }

            if (textBox == null) return;

            if (textBox.Text.Length > 0) {
                var value = Regex.Replace(textBox.Text, "[^0-9.]","");
                textBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 1.0f));
                textBox.Text = value;
            }
            else
            {
                textBox.TextElement.Value = emptyText;
                textBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
            }
        }
        
        static void JoinLobbyButton_Pressed(PressedEventArgs obj)
		{
            CreatePlayerUI();
            SceneManager.CreatePlayScene();

            LineEdit ipAddress = null;
            LineEdit port = null;

            foreach (UIElement element in joinUI) if (element.Name == "ServerAddressBox") ipAddress = (LineEdit)element;
            foreach (UIElement element in joinUI) if (element.Name == "ServerPortBox") port = (LineEdit)element;

            var newPlayer = new ClientPlayer(UIUtils.GetPlayerName(), 0);

            var session = new ClientSession(ipAddress.Text, Int32.Parse(port.Text), newPlayer);

            ClientManager.session = session; //TODO: Refactor this to be non-static

            newPlayer.Init();
            session.init();
            
            Node cameraNode = SceneManager.playScene.GetChild("MainCamera", true);
			cameraNode.GetComponent<Camera>();
            
            SceneManager.ShowScene(SceneManager.playScene);
            UIUtils.SwitchUI(joinUI, playerUI);
            UIUtils.disableIO();
        }

		static private async void GetQRCode() //TODO: See if there is a way to move this to a QRUtils class
		{
			var scanner = new MobileBarcodeScanner();
			var options = new MobileBarcodeScanningOptions();

			options.PossibleFormats = new List<ZXing.BarcodeFormat>() { ZXing.BarcodeFormat.QR_CODE };

			scanner.TopText = "Join Lobby";
			scanner.BottomText = "Scan the QR code on the host's device";
			scanner.CancelButtonText = "Cancel";

			var result = await scanner.Scan(options);

			if (result == null)
				return;

			Console.WriteLine("Found QR: " + result.Text);

			var size = result.Text.Length;

            String trimmedResult;
            
            if (size > QRStringLength) //123.456.789.000:65535
                trimmedResult = result.Text.Substring(0, (int)QRStringLength);
            else
                trimmedResult = result.Text.Substring(0, size);
            
            UpdateServerAddress(trimmedResult);
		}

		static public void GenerateQRCode(String qrDataString, bool isServer) //TODO: Move to a QRUtils class when GetQRCode() can also be moved
		{
			var barcodeWriter = new BarcodeWriter
			{
				Format = ZXing.BarcodeFormat.QR_CODE,
				Options = new ZXing.Common.EncodingOptions
				{
					Width = 512,
					Height = 512,
					Margin = 0
				}
			};

			barcodeWriter.Renderer = new BitmapRenderer();
			var bitmap = barcodeWriter.Write(qrDataString);

			Texture2D qrCodeImage = new Texture2D();
			qrCodeImage.SetSize(512, 512, Graphics.RGBFormat, TextureUsage.Dynamic);
			qrCodeImage.SetNumLevels(1);

			MemoryStream stream = new MemoryStream();

#if __ANDROID__
				bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, stream);
#endif

#if __IOS__
			using (NSData imageData = bitmap.AsJPEG())
			{
				Byte[] myByteArray = new Byte[imageData.Length];
				System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));
				stream.Write(myByteArray, 0, myByteArray.Length);
			}
#endif
			stream.Position = 0;

			Image image = new Image();
			image.Load(new MemoryBuffer(stream));

			qrCodeImage.SetData(image, false);

            if (isServer) ShowServerAddress(qrCodeImage, qrDataString);
            else ShowClientAddress(qrCodeImage);
		}

        private static void ShowClientAddress(Texture qrCodeImg)
        {
            BorderImage qrCode = null;

            foreach (var element in joinUI) { if (element.Name == "ClientQRCode") qrCode = (BorderImage)element; }

            if (qrCode != null) qrCode.Texture = qrCodeImg;
        }

        private static void ShowServerAddress(Texture qrCodeImg, String address)
		{
			BorderImage qrCode = null;
			Text addressText = null;

			foreach (var element in lobbyUI) { if (element.Name == "AddressQRCode") qrCode = (BorderImage)element; }
			foreach (var element in lobbyUI) { if (element.Name == "AddressText") addressText = (Text)element; }

			if(qrCode!=null) qrCode.Texture = qrCodeImg;
			if (addressText != null) addressText.Value = address;
		}

		private static void UpdateServerAddress(String value)
		{
            var address = value.Split(':');
			LineEdit serverAddress = null;
            LineEdit serverPort = null;

			foreach (var element in joinUI) { if (element.Name == "ServerAddressBox") serverAddress = (LineEdit)element; }
            foreach (var element in joinUI) { if (element.Name == "ServerPortBox") serverPort = (LineEdit)element; }
            if (serverAddress != null) { Application.InvokeOnMain(new Action(() => serverAddress.Text = address[0])); }
            if (serverPort != null) { Application.InvokeOnMain(new Action(() => serverPort.Text = address[1])); }
        }
        
        static void ScanQRButton_Pressed(PressedEventArgs obj)
        {
            GetQRCode();
        }

        static void StartGameButton_Pressed(PressedEventArgs obj)
        {
            CreateTableUI();
            SceneManager.CreateHostScene();

            var countdown = new Thread(StartGame); 
            countdown.Start();
        }

        static private void Countdown()
        {
            var soundnode = SceneManager.hostScene.GetChild("SFX", true);
            var sound = soundnode.GetComponent<SoundSource>(true);
            Application.InvokeOnMain(new Action(() => sound.Play(UIManager.cache.GetSound("Sounds/Shuffle.wav"))));

            foreach (UIElement element in lobbyUI) if (element.Name != "LobbyMessageText") UIUtils.disableAndHide(element);

            for (int i = 3; i > 0; i--)
            {
                UIUtils.DisplayLobbyMessage("Starting game in " + i);
                Thread.Sleep(1000);
            }
        }

        static private void StartGame()
        {
            Countdown();
            
            Application.InvokeOnMain(new Action(() => SceneManager.ShowScene(SceneManager.hostScene)));
            UIUtils.SwitchUI(lobbyUI, tableUI);


            UIUtils.DisplayLobbyMessage("Players in Room"); //Reset the message
            
            var game = new PokerGame(Session.getinstance().getRoom());
            game.Start();
        }

        static public void AddToUI(List<UIElement> elements)
		{
			foreach (var element in elements) { ui.Root.AddChild(element); }
		}
	}
}
