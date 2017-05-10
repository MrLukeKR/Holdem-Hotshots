#if __IOS__
using Foundation;
#endif
using HoldemHotshots.GameLogic;
using HoldemHotshots.Managers;
using HoldemHotshots.Networking.ServerNetworkEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Urho;
using Urho.Audio;
using Urho.Gui;
using Urho.Resources;
using Urho.Urho2D;
using ZXing.Mobile;

namespace HoldemHotshots.Utilities
{
    /// <summary>
    /// This class performs operations on the User Interface and provides helpful, commonly used functions for UI object manipulation
    /// </summary>
	public static class UIUtils
	{
        const float DISABLED_OPACITY = 0.2f;
        const float ENABLED_OPACITY   = 1.0f;

        /// <summary>
        /// Searches a UI Element list for a given element name
        /// </summary>
        /// <param name="elementName">Name of the UI element</param>
        /// <param name="uiToSearch">UI Element list to search through</param>
        /// <returns></returns>
        public static UIElement FindUIElement(string elementName, List<UIElement> uiToSearch)
        {
            foreach (UIElement element in uiToSearch)
                if (element.Name == elementName)
                    return element;
            return null;
        }

        /// <summary>
        /// Find UI Element of a specific type, given its name
        /// </summary>
        /// <typeparam name="T">UI Element type</typeparam>
        /// <param name="elementName">Name of the UI element</param>
        /// <param name="uiToSearch">UI Element list to search through</param>
        /// <returns></returns>
        public static T FindUIElement<T>(string elementName, List<UIElement> uiToSearch) where T : UIElement
        {
            return (T)FindUIElement(elementName, uiToSearch);
        }

        /// <summary>
        /// Displays the UIElement on the screen and enables its methods (e.g. button pressing)
        /// </summary>
        /// <param name="element">The UIElement to enable and display to the screen</param>
        public static void EnableAndShow(UIElement element)
		{
            Application.InvokeOnMain(new Action(() =>
            {
                element.Visible = true;
                element.Enabled = true;
            }));
		}

        /// <summary>
        /// Removes the UIElement from the display and disables its methods (e.g. button pressing)
        /// </summary>
        /// <param name="element">The UIElement to disable and remove from the display</param>
        public static void DisableAndHide(UIElement element)
        {
            Application.InvokeOnMain(new Action(() =>
            {
                element.Visible = false;
                element.Enabled = false;
            }));
        }

        /// <summary>
        /// Display the restart game button on the screen
        /// </summary>
        internal static void ShowRestartOptions()
        {
            EnableAndShow( FindUIElement("GameRestartButtonNoAutoLoad", UIManager.tableUI) );
        }
        
        /// <summary>
        /// Disables a UIElement and makes the graphic greyed-out
        /// </summary>
        /// <param name="element"></param>
        public static void DisableAccess(UIElement element)
        {
                element.Enabled = false;
                element.Opacity = DISABLED_OPACITY;
        }

        /// <summary>
        /// Enables a UIElement and removes the grey-out effect from the graphic, if necessary
        /// </summary>
        /// <param name="element"></param>
        public static void EnableAccess(UIElement element)
        {
                element.Enabled = true;
                element.Opacity = ENABLED_OPACITY;
        }

		/// <summary>
        /// Iterates through a list of UIElements and displays each one in the collection to the screen
        /// </summary>
        /// <param name="uiCollection">The list of UIElements that are to be displayed on-screen</param>
		public static void ShowUI(List<UIElement> uiCollection)
        {
            foreach (var uiElement in uiCollection)
                if(!uiElement.Name.Contains("NoAutoLoad"))
                    EnableAndShow(uiElement);
        }

        /// <summary>
        /// Iterates through a list of UIElements and removes each one in the collection from the screen
        /// </summary>
        /// <param name="uiCollection">The list of UIElements that are to be removed from the screen</param>
		public static void HideUI(List<UIElement> uiCollection)
        {
            foreach (var uiElement in uiCollection)
                DisableAndHide(uiElement);
        }

        /// <summary>
        /// Hides all elements from one UIElement list and shows all elements from the second UIElement list
        /// </summary>
        /// <param name="from">UIElement list to remove from the display</param>
        /// <param name="to">UIElement list to add to the display</param>
		public static void SwitchUI(List<UIElement> from, List<UIElement> to)
        {
            HideUI(from);
            ShowUI(to);
        }

        /// <summary>
        /// Shows a message at the top of the client's UI (if they're in-game)
        /// </summary>
        /// <param name="message">Message to be displayed on the client Player's screen</param>
        internal static void DisplayPlayerMessage(string message)
        {
            Application.InvokeOnMain(new Action(() =>
            {
                if (UIManager.playerUI == null)
                    return;

                Text statusText = FindUIElement<Text>("PlayerInfoText", UIManager.playerUI);

                if (statusText != null)
                    statusText.Value = message;
            }));
        }

        /// <summary>
        /// Prints the Player's current balance to their UI
        /// </summary>
        /// <param name="balance">Amount of chips the player currently has</param>
        internal static void UpdatePlayerBalance(uint balance)
        {
            Application.InvokeOnMain(new Action(() =>
            {
                Text statusText = FindUIElement<Text>("PlayerBalanceText", UIManager.playerUI);

                if (statusText != null)
                    statusText.Value = "$" + balance.ToString() + " ";
            }));
        }
        
        /// <summary>
        /// Extracts the text entered into the "Player Name" textbox and returns it as a string
        /// </summary>
        /// <returns>Player name</returns>
        public static string GetPlayerName()
        {
            LineEdit playerName = FindUIElement<LineEdit>("PlayerNameBox", UIManager.joinUI);

            if (playerName == null)
                return "Unknown Player";


            if (playerName.Text.Length == 0)
                return "Unknown Player";
            else
                return playerName.Text;
        }

        public static uint GetPlayerBalance()
        {
            Text balance = FindUIElement<Text>("PlayerBalanceText", UIManager.playerUI);
            
            return uint.Parse(balance.Value.Substring(1));
        }

        /// <summary>
        /// Extracts the text stored in the "IP Address" and "Port" textboxes and converts them to a QR code
        /// </summary>
        public static void ConvertClientInfoToQR()
        {
            string address = ClientManager.serverAddress;
            string port = ClientManager.serverPort;
            string key = ClientManager.serverKey;
            string iv = ClientManager.serverIV;

            if (address == "" || port == "" || key == "" || iv == "")
                return;

            CreateQRCode(address + ":" + port + "+" + key + "+"+ iv, false);
        }

        /// <summary>
        /// Prints a list of current players to the table UI, using a room object
        /// </summary>
        /// <param name="room">Room object containing currently active players</param>
        public static void UpdatePlayerList(Room room)
        {
            string  playerList  = "";
            
            for (int i = 0; i < room.players.Count; i++)
                playerList += room.players[i].name + "\n";

            for (int i = room.players.Count + 1; i <= Room.MAX_ROOM_SIZE; i++)
                playerList += "Waiting for Player " + i + "...\n";

            Application.InvokeOnMain(new Action(() =>
            {
                Text playerNames = FindUIElement<Text>("PlayerNames", UIManager.lobbyUI);

                if (playerNames != null)
                    playerNames.Value = playerList;
            }));
        }

        /// <summary>
        /// Prints a text message to the Server lobby's UI
        /// </summary>
        /// <param name="message">Message to be displayed on-screen</param>
        public static void DisplayLobbyMessage(string message)
        {
                Application.InvokeOnMain(new Action(() =>
                {
                    Text lobbyText = FindUIElement<Text>("LobbyMessageText", UIManager.lobbyUI);
                    
                    if (lobbyText != null)
                        lobbyText.Value = message; //TODO: Alter the position to remove the preceding spacing
                }));
        }

        /// <summary>
        /// Iterates through the Player's UI and disables all buttons
        /// </summary>
        internal static void DisableIO()
        {
            foreach (UIElement element in UIManager.playerUI)
                if (element.Name.Contains("Button") && element.Name != "PlayerExitButton")
                    DisableAccess(element);
        }

        /// <summary>
        /// Iterates through the Player's UI and enables all buttons
        /// </summary>
        internal static void EnableIO()
        {
            DisplayPlayerMessage("It's Your Turn!");
            
            foreach (UIElement element in UIManager.playerUI)
                if (element.Name.Contains("Button") && element.Name != "PlayerExitButton")
                    EnableAccess(element);
        }

        /// <summary>
        /// Extracts the amount of chips to bet, from the Player's Raise UI; Can either reset the amount to zero or not afterwards (e.g. if they back out of the raise without confirming)
        /// </summary>
        /// <param name="reset">Whether or not to reset the raise amount to zero once the value has been extracted</param>
        /// <returns></returns>
        public static uint GetRaiseAmount(bool reset)
        {
            Text raiseText = FindUIElement<Text>("CurrentBetText", UIManager.playerUI_raise);
            uint amount    = uint.Parse(raiseText.Value.Substring(1));

            if(reset)
                UpdateRaiseBalance(1);

            return amount;
        }

        /// <summary>
        /// Gives visual feedback to the user to show that they have increased or decreased the raise amount when using the up/down raise buttons
        /// </summary>
        /// <param name="amount">The amount of chips to print to the screen</param>
        internal static void UpdateRaiseBalance(uint amount)
        {
            Text raiseText = FindUIElement<Text>("CurrentBetText", UIManager.playerUI_raise);

            raiseText.Value = "$" + amount;
        }

        /// <summary>
        /// Takes a string of data and generates a QR code representing that data
        /// </summary>
        /// <param name="qrDataString">Data to convert to a QR code</param>
        /// <param name="isServer">Whether the code is being generated on the server or client side</param>
        [SecurityCritical]
        public static void GenerateQRCode(string qrDataString, bool isServer)
        {
            if (isServer)
                Application.InvokeOnMain(new Action(() => CreateQRCode(qrDataString, isServer)));
            else
                CreateQRCode(qrDataString, isServer);
        }

        /// <summary>
        /// Creates a QR code from a given string of data
        /// </summary>
        /// <param name="data">Data to encode into a QR code</param>
        /// <param name="isServer">True/False of whether or not this device is the server</param>
        public static void CreateQRCode(string data, bool isServer)
        {
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
                var bitmap = barcodeWriter.Write(data);

                Texture2D qrCodeImage = new Texture2D();
                qrCodeImage.SetSize(512, 512, Graphics.RGBFormat, TextureUsage.Dynamic);
                qrCodeImage.SetNumLevels(1);

                MemoryStream stream = new MemoryStream();

#if __ANDROID__
				bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, stream);
#elif __IOS__
                using (NSData imageData = bitmap.AsJPEG())
                {
                    byte[] myByteArray = new byte[imageData.Length];
                    System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));
                    stream.Write(myByteArray, 0, myByteArray.Length);
                }
#endif
                stream.Position = 0;

                Image image = new Image();
                image.Load(new MemoryBuffer(stream));

                qrCodeImage.SetData(image, false);
                
                if (isServer)
                    ShowServerAddress(qrCodeImage);
                else
                    ShowClientAddress(qrCodeImage);
            }
        }

        /// <summary>
        /// Displays the QR code
        /// </summary>
        /// <param name="qrCodeImg">QR Code image data</param>
        private static void ShowServerAddress(Texture qrCodeImg)
        {
            BorderImage qrCode = FindUIElement<BorderImage>("AddressQRCode", UIManager.lobbyUI);

            if (qrCode != null)
                qrCode.Texture = qrCodeImg;
        }

        /// <summary>
        /// Updates the server address value in the Client Manager
        /// </summary>
        /// <param name="value">Value to assign to the server address variables</param>
        public static void UpdateServerAddress(string value)
        {
                string[] data = value.Split(':');

                ClientManager.serverAddress = data[0];
                ClientManager.serverPort = data[1];
                ClientManager.serverKey = data[2];
                ClientManager.serverIV = data[3];

            AlterJoin(ValidateJoinGame());
        }

        /// <summary>
        /// Determines if the game can be joined (based on server RegEx etc)
        /// </summary>
        /// <returns>True/False of if all variables are valid</returns>
        static public bool ValidateJoinGame()
        {
            return (ValidateServer() && ValidatePort() && ValidateKey() && ValidateIV());
        }

        /// <summary>
        /// Returns if the encryption key is not null
        /// </summary>
        /// <returns>True/False of if the encryption key is valid</returns>
        static public bool ValidateKey()
        {
            return ClientManager.serverKey.Length > 0;
        }

        /// <summary>
        /// Returns if the encryption IV is not null
        /// </summary>
        /// <returns>True/False of if the encryption IV is valid</returns>
        static public bool ValidateIV()
        {
            return ClientManager.serverIV.Length > 0;
        }

        /// <summary>
        /// Returns the QR code's string from a camera input screen
        /// </summary>
        /// <returns>Scanned QR code data</returns>
        static public async Task<string> GetQRCode()
        {
            var scanner = new MobileBarcodeScanner();
            var options = new MobileBarcodeScanningOptions();

            options.PossibleFormats = new List<ZXing.BarcodeFormat>()
            {
                ZXing.BarcodeFormat.QR_CODE
            };

            scanner.TopText          = "Join Lobby";
            scanner.BottomText       = "Scan the QR code on the host's device";
            scanner.CancelButtonText = "Cancel";

            var result = await scanner.Scan(options);

            if (result == null)
                return "";

            return result.Text;
        }

        /// <summary>
        /// Displays the QR code on the client device
        /// </summary>
        /// <param name="qrCodeImg">QR code image data</param>
        private static void ShowClientAddress(Texture qrCodeImg)
        {
            BorderImage qrCode = FindUIElement<BorderImage>("ClientQRCode", UIManager.joinUI);
            
            if (qrCode != null)
                qrCode.Texture = qrCodeImg;
        }

        /// <summary>
        /// Starts the "Game Starting" countdown
        /// </summary>
        static public void Countdown()
        {
            var soundnode   = SceneManager.hostScene.GetChild("SFX", true);
            var sound       = soundnode.GetComponent<SoundSource>(true);

            Application.InvokeOnMain(new Action(() => sound.Play(UIManager.cache.GetSound("Sounds/Shuffle.wav"))));
            
            foreach (UIElement element in UIManager.lobbyUI)
                if (element.Name != "LobbyMessageText")
                    DisableAndHide(element);

            for (int i = 3; i > 0; Thread.Sleep(1000))
                DisplayLobbyMessage("Starting game in " + i--);
        }

        /// <summary>
        /// Begins the currently set-up game
        /// </summary>
        static public void StartGame()
        {
            Countdown();
            Application.InvokeOnMain(new Action(() => SceneManager.ShowScene(SceneManager.hostScene)));
            SwitchUI(UIManager.lobbyUI, UIManager.tableUI);
            
            DisplayLobbyMessage("Players in Room"); //Reset the message
 
            new PokerGame(Session.Getinstance().getRoom()).Start();
        }

        /// <summary>
        /// Enables the Start Game button if the game can be started
        /// </summary>
        static public void ValidateStartGame()
        {
            if (Session.Lobby.players.Count >= 2)
                EnableAccess(FindUIElement("StartGameButton", UIManager.lobbyUI));
        }

        /// <summary>
        /// Creates a new game and plays it
        /// </summary>
        static public void RestartGame()
        {
            SwitchUI(UIManager.tableUI, UIManager.lobbyUI);
            StartGame();
        }

        /// <summary>
        /// Adds an element to the UI
        /// </summary>
        /// <param name="elements">List of UI elements</param>
        static public void AddToUI(List<UIElement> elements)
        {
            foreach (var element in elements)
                UIManager.ui.Root.AddChild(element);
        }

        /// <summary>
        /// Determines whether the Server Address is valid
        /// </summary>
        /// <returns>True/False result of Server Address validation</returns>
        static public bool ValidateServer()
        {
            bool isValid = false;

            string server = ClientManager.serverAddress;

            if (server != "" && server.Length > 0)
                isValid = Regex.IsMatch(server, "^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");

            return isValid;;
        }

        /// <summary>
        /// Toggles the Call or Check buttons depending on the current stake
        /// </summary>
        /// <param name="stake">Current pot stake</param>
        static public void ToggleCallOrCheck(uint stake)
        {
            if (stake == 0)
            {
                DisableAndHide( FindUIElement("CallButton" , UIManager.playerUI) );
                EnableAndShow ( FindUIElement("CheckButton", UIManager.playerUI) );
            }
            else
            {
                DisableAndHide( FindUIElement("CheckButton", UIManager.playerUI) );
                EnableAndShow ( FindUIElement("CallButton" , UIManager.playerUI) );
            }
        }

        /// <summary>
        /// Determines if the Port is valid
        /// </summary>
        /// <returns>True/False result of Port validation</returns>
        static public bool ValidatePort()
        {
            string port = ClientManager.serverPort;
            if (port.Length > 0)
            {
                var portNumber = int.Parse(port);
                
                if (portNumber >= 0 && portNumber <= 65535)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Enables or disables the join button depending on the input
        /// </summary>
        /// <param name="enable">True/False of whether or not to enable the join button</param>
        static public void AlterJoin(bool enable)
        {
            Button joinButton = FindUIElement<Button>("JoinLobbyButton", UIManager.joinUI);
            
            if (enable)
                EnableAccess(joinButton);
            else if (!enable)
                DisableAccess(joinButton);
        }

        /// <summary>
        /// Changes the colour and value of a given text box depending on its current value
        /// </summary>
        /// <param name="boxName">TextBox to find</param>
        /// <param name="emptyText">Text to display if the TextBox is empty</param>
        /// <param name="uiCollection">UI Element list to search through for the TextBox</param>
        public static void AlterLineEdit(string boxName, string emptyText, List<UIElement> uiCollection)
        {
            LineEdit textBox = FindUIElement<LineEdit>(boxName, uiCollection);
            
            if (textBox == null)
                return;

            if (textBox.Text.Length > 0)
                textBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 1.0f));
            else
            {
                textBox.TextElement.Value = emptyText;
                textBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
            }
        }

        /// <summary>
        /// Changes the colour and value of a given text box dependining on its current numeric value
        /// </summary>
        /// <param name="boxName">TextBox to find</param>
        /// <param name="emptyText">Text to display if the TextBox is empty</param>
        /// <param name="uiCollection">UI Element list to search through for the TextBox</param>
        public static void AlterNumericLineEdit(string boxName, string emptyText, List<UIElement> uiCollection)
        {
            LineEdit textBox = FindUIElement<LineEdit>(boxName, uiCollection);

            if (textBox == null)
                return;

            if (textBox.Text.Length > 0)
            {
                textBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 1.0f));
                textBox.Text = Regex.Replace(textBox.Text, "[^0-9.]", "");
            }
            else
            {
                textBox.TextElement.Value = emptyText;
                textBox.TextElement.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
            }
        }
    }
}