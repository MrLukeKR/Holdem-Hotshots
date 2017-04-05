using HoldemHotshots.GameLogic.Player;
using HoldemHotshots.Networking.ClientNetworkEngine;
using HoldemHotshots.Networking.ServerNetworkEngine;
using HoldemHotshots.Utilities;
using System;
using System.Threading;
using Urho;
using Urho.Gui;

namespace HoldemHotshots.Managers
{
    class InputManager
    {
        public static void RaiseCancelButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.UpdateRaiseBalance(0);
            UIUtils.SwitchUI(UIManager.playerUI_raise, UIManager.playerUI);
            UIUtils.ToggleCallOrCheck(ClientManager.highestBid);
        }

        public static void RaiseConfirmButton_Pressed(PressedEventArgs obj)
        {
            ClientManager.session.player.Raise();
            UIUtils.SwitchUI(UIManager.playerUI_raise, UIManager.playerUI);
            UIUtils.ToggleCallOrCheck(ClientManager.highestBid);
        }

        public static void IncreaseBetButton_Pressed(PressedEventArgs obj)
        {
            var amount = UIUtils.GetRaiseAmount(false);
            var playerBalance = UIUtils.GetPlayerBalance();
            var button = (Button)obj.Element;
            
            if (amount + 1 <= playerBalance)
                UIUtils.UpdateRaiseBalance(UIUtils.GetRaiseAmount(false) + 1);
        }

        public static void DecreaseBetButton_Pressed(PressedEventArgs obj)
        {
            var amount = UIUtils.GetRaiseAmount(false);

            if (amount > 1)
                UIUtils.UpdateRaiseBalance(amount - 1);
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
            if (UIUtils.GetPlayerBalance() > 0)
                UIUtils.SwitchUI(UIManager.playerUI, UIManager.playerUI_raise);
            else
                UIUtils.DisplayPlayerMessage("Insufficient Chips!");
        }
        
        public static void QrCodeButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.ConvertClientInfoToQR();
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
            ClientManager.session.Disconnect();
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

        public static void GameRestartButton_Pressed(PressedEventArgs obj)
        {
            foreach(ServerPlayer player in Session.Lobby.players)
                player.Reset();

            ServerPlayer backOfQueue = Session.Lobby.players[0];
            Session.Lobby.players.RemoveAt(0);
            Session.Lobby.players.Add(backOfQueue);

            foreach (UIElement element in UIManager.tableUI)
                if (element.Name == "GameRestartButtonNoAutoLoad")
                    UIUtils.DisableAndHide(element);

            SceneManager.CreateHostScene();
            SceneManager.ShowScene(SceneManager.hostScene);

            SceneUtils.InitPlayerInformation(Session.Lobby.players);

            new Thread(UIUtils.RestartGame).Start();
        }

        public static void HostButton_Pressed(PressedEventArgs obj)
        {
            Session.Getinstance().Init();
            if (UIManager.lobbyUI.Count == 0)
                UIManager.CreateLobbyUI();
            foreach (UIElement elem in UIManager.lobbyUI)
                if (elem.Name == "StartGameButton")
                    UIUtils.DisableAccess(elem);
            UIUtils.SwitchUI(UIManager.menuUI, UIManager.lobbyUI);
        }

        public static void SettingsButton_Pressed(PressedEventArgs obj)
        {
            UIManager.CreateSettingsUI();
            UIUtils.SwitchUI(UIManager.menuUI, UIManager.settingsUI);
        }
        
        public static void StartGameButton_Pressed(PressedEventArgs obj)
        {
            if (Session.Lobby.players.Count < 2)
                return;

            UIManager.CreateTableUI();
            SceneManager.CreateHostScene();

            SceneUtils.InitPlayerInformation(Session.Lobby.players);

            new Thread(UIUtils.StartGame).Start();
        }

        public static void JoinBackButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.SwitchUI(UIManager.joinUI, UIManager.menuUI);
        }

        public static void LobbyBackButton_Pressed(PressedEventArgs obj)
        {
            UIUtils.SwitchUI(UIManager.lobbyUI, UIManager.menuUI);
            Session.DisposeOfSockets();
        }

        public static async void ScanQRButton_Pressed(PressedEventArgs obj)
        {
            var result  = await UIUtils.GetQRCode();

            if(result.Length > 0)
                UIUtils.UpdateServerAddress(result);
        }

        public static void PlayerNameBox_TextChanged(TextChangedEventArgs obj)
        {
            UIUtils.AlterLineEdit("PlayerNameBox", "Enter Player Name", UIManager.joinUI);
        }
        
        public static void JoinLobbyButton_Pressed(PressedEventArgs obj)
        {
            if (!(UIUtils.ValidateServer() && UIUtils.ValidatePort() &&UIUtils.ValidateKey() && UIUtils.ValidateIV()))
                return;

            string address = ClientManager.serverAddress;
            string port = ClientManager.serverPort;
            string key = ClientManager.serverKey;
            string iv = ClientManager.serverIV;

            var newPlayer = new ClientPlayer(UIUtils.GetPlayerName(), 0);
           
            ClientManager.session = new ClientSession(address, int.Parse(port), newPlayer, key, iv);

            if (ClientManager.session.Connect())
            {
                ClientManager.session.Init();
                UIManager.CreatePlayerUI();              
                SceneManager.CreatePlayScene();

                newPlayer.Init();

                SceneManager.ShowScene(SceneManager.playScene);
                UIUtils.SwitchUI(UIManager.joinUI, UIManager.playerUI);

                UIUtils.ToggleCallOrCheck(0);

                Application.InvokeOnMain(new Action(() => UIUtils.DisableIO()));
            }
            else
            {
                //TODO: Error message
            }
        }
    }
}