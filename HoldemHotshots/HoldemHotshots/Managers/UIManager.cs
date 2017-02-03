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

			var settingsButtonWidthAndHeight = graphics.Width / 10;

			//Create UI objects
			var settingsButton = new Button()
			{
				Name = "SettingsButton",
				Texture = cache.GetTexture2D("Textures/settingsButton.png"),
				Size = new IntVector2(settingsButtonWidthAndHeight, settingsButtonWidthAndHeight),
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Top,
				Visible = false,
				Enabled = false
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
			menuUI.Add(settingsButton);
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
            var serverBoxWidth = (graphics.Width / 2);

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

			var serverAddressBox = new LineEdit()
			{
				Name = "ServerAddressBox",
				Size = new IntVector2(nameBoxWidth, nameBoxHeight),
				Position = new IntVector2(0, playerNameBox.Position.Y + playerNameBox.Height + nameBoxHeight / 2),
                HorizontalAlignment = HorizontalAlignment.Center,
                Editable = true,
				Opacity = 0.6f,
				MaxLength = 21
			};

			//ServerAddressBox TextElement properties
			serverAddressBox.TextElement.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
			serverAddressBox.TextElement.Value = "Enter Server Address";
			serverAddressBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
			serverAddressBox.TextElement.HorizontalAlignment = HorizontalAlignment.Center;
			serverAddressBox.TextElement.VerticalAlignment = VerticalAlignment.Center;

            var scanQRButton = new Button()
            {
                Name = "ScanQRButton",
                Texture = cache.GetTexture2D("Textures/scanQRButton.png"),
                BlendMode = BlendMode.Replace,
                Size = new IntVector2((graphics.Width / 6) , graphics.Width / 6),
                Position = new IntVector2(0, serverAddressBox.Position.Y + serverAddressBox.Height + serverAddressBox.Height / 2),
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
			playerAvatar.Pressed += PlayerAvatar_Pressed;
			playerNameBox.TextChanged += PlayerNameBox_TextChanged;
			serverAddressBox.TextChanged += ServerAddressBox_TextChanged;
            scanQRButton.Pressed += ScanQRButton_Pressed;
			joinLobbyButton.Pressed += JoinLobbyButton_Pressed;

			//Add to the HostUI List
			joinUI.Add(joinBackButton);
			joinUI.Add(playerAvatar);
			joinUI.Add(playerNameBox);
			joinUI.Add(serverAddressBox);
            joinUI.Add(scanQRButton);
			joinUI.Add(joinLobbyButton);

			AddToUI(joinUI);
		}
        
        static private void CreateHostUI()
		{
			if (hostUI.Count > 0)
				return;

			//Size parameters
			var hostButtonWidth = (graphics.Width / 3) * 2;
			var hostButtonHeight = graphics.Width / 5;
			var lobbyBoxWidth = (graphics.Width / 3) * 2;
			var lobbyBoxHeight = graphics.Height / 20;
			var backButtonWidthAndHeight = graphics.Width / 10;

			//Create UI objects
			var hostBackButton = new Button()
			{
				Name = "HostBackButton",
				Texture = cache.GetTexture2D("Textures/backButton.png"),
				Size = new IntVector2(backButtonWidthAndHeight, backButtonWidthAndHeight),
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				Visible = false,
				Enabled = false
			};

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

			var buyInAmountBox = new LineEdit()
			{
				Name = "BuyInAmountBox",
				Size = new IntVector2(lobbyBoxWidth, lobbyBoxHeight),
				Position = new IntVector2(0, lobbyNameBox.Position.Y + lobbyNameBox.Height + lobbyNameBox.Height / 2),
				HorizontalAlignment = HorizontalAlignment.Center,
				Editable = true,
				Opacity = 0.6f,
				MaxLength = 15,
			};

			//BuyInAmountBox TextElement properties
			buyInAmountBox.TextElement.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
			buyInAmountBox.TextElement.Value = "Enter Buy~In Amount";
			buyInAmountBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
			buyInAmountBox.TextElement.HorizontalAlignment = HorizontalAlignment.Center;
			buyInAmountBox.TextElement.VerticalAlignment = VerticalAlignment.Center;

			var addressQRCode = new BorderImage()
			{
				Name = "AddressQRCode",
				Position = new IntVector2(0, buyInAmountBox.Position.Y + buyInAmountBox.Height * 2),
				HorizontalAlignment = HorizontalAlignment.Center
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

			var qrScreenWidth = (graphics.Width / 10) * 9;
			var qrElemDistance = Math.Abs( createLobbyButton.Position.Y - (buyInAmountBox.Position.Y + buyInAmountBox.Height * 4) );

			Console.WriteLine("DEBUG COMPARE: WIDTH " + qrScreenWidth + " AND DISTANCE " + qrElemDistance);

			if (qrScreenWidth < qrElemDistance)
				addressQRCode.Size = new IntVector2(qrScreenWidth, qrScreenWidth);
			else
				addressQRCode.Size = new IntVector2(qrElemDistance, qrElemDistance);

			var addressText = new Text()
			{
				Name = "AddressText",
				Position = new IntVector2(0, addressQRCode.Position.Y + addressQRCode.Height + lobbyBoxHeight / 2),
				HorizontalAlignment = HorizontalAlignment.Center
			};

			addressText.SetFont(cache.GetFont("Fonts/arial.ttf", true), 20);
			addressText.SetColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));

			//LobbyNameBox TextElement properties
			lobbyNameBox.TextElement.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
			lobbyNameBox.TextElement.Value = "Enter Lobby Name";
			lobbyNameBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
			lobbyNameBox.TextElement.HorizontalAlignment = HorizontalAlignment.Center;
			lobbyNameBox.TextElement.VerticalAlignment = VerticalAlignment.Center;

			//Subscribe to Events
			hostBackButton.Pressed += HostBackButton_Pressed;
			lobbyNameBox.TextChanged += LobbyNameBox_TextChanged;
			createLobbyButton.Pressed += CreateLobbyButton_Pressed;

			hostUI.Add(hostBackButton);
			hostUI.Add(lobbyNameBox);
			hostUI.Add(buyInAmountBox);
			hostUI.Add(addressQRCode);
			hostUI.Add(addressText);
			hostUI.Add(createLobbyButton);

			AddToUI(hostUI);
		}

		static void JoinButton_Pressed(PressedEventArgs obj) 
		{ 
			if (joinUI.Count == 0) CreateJoinUI(); 
			UIUtils.SwitchUI(menuUI, joinUI); 
		}

		static void HostButton_Pressed(PressedEventArgs obj) 
		{ 
			if (hostUI.Count == 0) CreateHostUI(); 
			UIUtils.SwitchUI(menuUI, hostUI);
		}

		static void PlayerAvatar_Pressed(PressedEventArgs obj)
		{
			//TODO: Implement player avater press
		}

		static void SettingsButton_Pressed(PressedEventArgs obj)
		{
			//TODO: Implement settings press
		}
		static void JoinBackButton_Pressed(PressedEventArgs obj) { UIUtils.SwitchUI(joinUI, menuUI); }
		static void HostBackButton_Pressed(PressedEventArgs obj) { UIUtils.SwitchUI(hostUI, menuUI); }

		static void PlayerNameBox_TextChanged(TextChangedEventArgs obj) { AlterLineEdit("PlayerNameBox", "Enter Player Name", joinUI); }
		static void ServerAddressBox_TextChanged(TextChangedEventArgs obj) { AlterLineEdit("ServerAddressBox", "Enter Server Address", joinUI); }
		static void LobbyNameBox_TextChanged(TextChangedEventArgs obj) { AlterLineEdit("LobbyNameBox", "Enter Lobby Name", hostUI); }

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

		static void JoinLobbyButton_Pressed(PressedEventArgs obj)
		{
			SceneManager.CreatePlayScene();

			Node cameraNode = SceneManager.playScene.GetChild("MainCamera", true);
			var camera = cameraNode.GetComponent<Camera>();

			if (camera != null)
				PositionUtils.InitPlayerCardPositions(camera);

			SceneManager.ShowScene(SceneManager.playScene);
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

			if (size > 15)
				size = 15;

			String trimmedResult = result.Text.Substring(0, size);


		}

		static public void GenerateQRCode(String qrDataString) //TODO: Move to a QRUtils class when GetQRCode() can also be moved
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

			var stream = new MemoryStream();

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

			var image = new Image();
			image.Load(new Urho.MemoryBuffer(stream));

			qrCodeImage.SetData(image, true);

			ShowAddress(qrCodeImage, qrDataString);
		}

		private static void ShowAddress(Texture qrCodeImg, String address)
		{
			BorderImage qrCode = null;
			Text addressText = null;

			foreach (var element in hostUI) { if (element.Name == "AddressQRCode") qrCode = (BorderImage)element; }
			foreach (var element in hostUI) { if (element.Name == "AddressText") addressText = (Text)element; }

			if(qrCode!=null) qrCode.Texture = qrCodeImg;
			if (addressText != null) addressText.Value = address;
		}

		private static void UpdateServerAddress(String value)
		{
			LineEdit serverAddress = null;
			foreach (var element in joinUI) { if (element.Name == "ServerAddressBox") serverAddress = (LineEdit)element; }
			if (serverAddress != null) { Urho.Application.InvokeOnMain(new Action(() => serverAddress.Text = value)); }
		}
        
        static void ScanQRButton_Pressed(PressedEventArgs obj)
        {
            GetQRCode();
        }

        static void CreateLobbyButton_Pressed(PressedEventArgs obj)
		{
			
			SceneManager.CreateHostScene();

			Node cameraNode = SceneManager.hostScene.GetChild("MainCamera", true);
			var camera = cameraNode.GetComponent<Camera>();

			if (camera != null)
				PositionUtils.InitTableCardPositions(camera);

			SceneManager.ShowScene(SceneManager.hostScene);

		}

		static public void AddToUI(List<UIElement> elements)
		{
			foreach (var element in elements) { ui.Root.AddChild(element); }
		}
	}
}
