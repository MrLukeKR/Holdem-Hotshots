using System.Threading;
using Urho.Gui;

namespace HoldemHotshots.Managers
{
    class InputManager
    {
        public static void RaiseCancelButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.SwitchUI(UIManager.playerUI_raise, UIManager.playerUI);
        }

        public static void RaiseConfirmButton_Pressed(PressedEventArgs obj)
        {
            ClientManager.session.player.Raise();
            UIUtils.SwitchUI(UIManager.playerUI_raise, UIManager.playerUI);
        }

        public static void IncreaseBetButton_Pressed(PressedEventArgs obj)
        {
            var amount = UIUtils.GetRaiseAmount();
            var playerBalance = 1000; //TODO: Get player balance

            if (amount + 1 < playerBalance)
                UIUtils.UpdateRaiseBalance(UIUtils.GetRaiseAmount() + 1);
        }

        public static void DecreaseBetButton_Pressed(PressedEventArgs obj)
        {
            var amount = UIUtils.GetRaiseAmount();
            if (amount > 0) UIUtils.UpdateRaiseBalance(amount - 1);
        }

        public static void RaiseExitButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.SwitchUI(UIManager.playerUI_raise, UIManager.menuUI);
        }

        public static void AllInButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.DisplayPlayerMessage("All  In");
            ClientManager.session.player.AllIn();
        }

        public static void RaiseButton_Pressed(PressedEventArgs obj)
        {
            UIManager.CreatePlayerRaiseUI();
            UIUtils.SwitchUI(UIManager.playerUI, UIManager.playerUI_raise);
        }

        public static void CallButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.DisplayPlayerMessage("Call");
            ClientManager.session.player.Call();
        }

        public static void CheckButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.DisplayPlayerMessage("Check");
            ClientManager.session.player.Check();
        }

        public static void FoldButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.DisplayPlayerMessage("Fold");
            ClientManager.session.player.Fold();
        }

        public static void PlayerExitButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.SwitchUI(UIManager.playerUI, UIManager.menuUI);
            SceneManager.ShowScene(SceneManager.menuScene);
        }

        public static void TableExitButton_Pressed(PressedEventArgs obj)
        {
            Session.DisposeOfSockets();

            UIUtils.SwitchUI(UIManager.tableUI, UIManager.menuUI);
            SceneManager.ShowScene(SceneManager.menuScene);
        }

        public static void SettingsExitButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.SwitchUI(UIManager.settingsUI, UIManager.menuUI);
        }


        public static void JoinButton_Pressed(PressedEventArgs obj)
        {
            if (UIManager.joinUI.Count == 0) UIManager.CreateJoinUI();
            UIUtils.SwitchUI(UIManager.menuUI, UIManager.joinUI);
        }

        public static void HostButton_Pressed(PressedEventArgs obj)
        {
            Session.getinstance().init();
            if (UIManager.lobbyUI.Count == 0) UIManager.CreateLobbyUI();
            UIUtils.SwitchUI(UIManager.menuUI, UIManager.lobbyUI);
        }

        public static void SettingsButton_Pressed(PressedEventArgs obj)
        {
            UIManager.CreateSettingsUI();
            UIUtils.SwitchUI(UIManager.menuUI, UIManager.settingsUI);
        }


        public static void StartGameButton_Pressed(PressedEventArgs obj)
        {
            UIManager.CreateTableUI();
            SceneManager.CreateHostScene();

            var countdown = new Thread(UIUtils.StartGame);
            countdown.Start();
        }

        public static void JoinBackButton_Pressed(PressedEventArgs obj) { UIUtils.SwitchUI(UIManager.joinUI, UIManager.menuUI); }
        public static void LobbyBackButton_Pressed(PressedEventArgs obj) { UIUtils.SwitchUI(UIManager.lobbyUI, UIManager.menuUI); Session.DisposeOfSockets(); } //TODO: Move this when the "waiting in lobby" UI is implemented

        public static async void ScanQRButton_Pressed(PressedEventArgs obj)
        {
            var result = await UIUtils.GetQRCode();

            var size = result.Length;

            string trimmedResult;

            if (size > UIManager.QRStringLength) //123.456.789.000:65535
                trimmedResult = result.Substring(0, UIManager.QRStringLength);
            else
                trimmedResult = result.Substring(0, size);

            UIUtils.UpdateServerAddress(trimmedResult);
        }

        public static void PlayerNameBox_TextChanged(TextChangedEventArgs obj)
        {
            UIUtils.AlterLineEdit("PlayerNameBox", "Enter Player Name", UIManager.joinUI);
        }

        public static void ServerAddressBox_TextChanged(TextChangedEventArgs obj)
        {
            UIUtils.AlterLineEdit("ServerAddressBox", "Enter Server IP Address", UIManager.joinUI);
            UIUtils.AlterJoin(UIUtils.ValidateServer() && UIUtils.ValidatePort());
        }

        public static void ServerPortBox_TextChanged(TextChangedEventArgs obj)
        {
            UIUtils.AlterLineEdit("ServerPortBox", "Enter Server IP Port", UIManager.joinUI);
            UIUtils.AlterJoin(UIUtils.ValidateServer() && UIUtils.ValidatePort());
        }
    }
}