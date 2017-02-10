using System;
using System.Collections.Generic;
using Urho;
using Urho.Gui;

namespace HoldemHotshots
{
	public static class UIUtils
	{
		public static void enableAndShow(UIElement element)
		{
			element.Visible = true;
			element.Enabled = true;
		}

		public static void disableAndHide(UIElement element)
		{
			element.Visible = false;
			element.Enabled = false;
		}

		//UI switching
		public static void ShowUI(List<UIElement> uiCollection) { foreach (var uiElement in uiCollection) UIUtils.enableAndShow(uiElement); }
		public static void HideUI(List<UIElement> uiCollection) { foreach (var uiElement in uiCollection) UIUtils.disableAndHide(uiElement); }
		public static void SwitchUI(List<UIElement> from, List<UIElement> to) { HideUI(from); ShowUI(to); }

        internal static void DisplayPlayerMessage(string message)
        {
            if (UIManager.playerUI == null) return;
            
            Text statusText = null;

            foreach (UIElement element in UIManager.playerUI) if (element.Name == "PlayerInfoText") statusText = (Text)element;

            if(statusText != null) statusText.Value = message; //TODO: Limit to a certain number of characters
        }

        internal static void UpdatePlayerBalance(uint balance)
        {
            Text statusText = null;

            foreach (UIElement element in UIManager.playerUI) if (element.Name == "PlayerBalanceText") statusText = (Text)element;

            if (statusText != null) statusText.Value = "$" + balance.ToString() + " "; //TODO: Alter the position to remove the preceding spacing
        }

        public static String GetPlayerName()
        {
            LineEdit playerName = null;

            foreach (UIElement element in UIManager.joinUI) if (element.Name == "PlayerNameBox") playerName = (LineEdit)element;

            if (playerName != null)
                if (playerName.Text.Length == 0)
                    return "Unknown Player";
                else
                    return playerName.Text;
            else
                return "Unknown Player";
        }

       public static void UpdatePlayerList(Room room)
        {
            Console.WriteLine("Updating Player List...");
            String playerList = "";

            Text playerNames = null;

            var max = room.MaxRoomSize;
            var curr = room.getRoomSize();

            for (int i = 0; i < curr; i++) playerList += room.getPlayer(i).getName() + "\n";
            for (int i = curr + 1; i <= max; i++) playerList += "Waiting for Player " + i + "...\n";

            Console.WriteLine("Generated List: ");
            Console.WriteLine(playerList);

            Application.InvokeOnMain(new Action(() =>
            {
                foreach (UIElement element in UIManager.lobbyUI) if (element.Name == "PlayerNames") playerNames = (Text)element;

                if (playerNames != null)
                {
                    playerNames.Value = playerList;
                    Console.WriteLine("Updated Player List Successfully!");
                }
            }
            ));
        }

        internal static uint GetBuyIn() //TODO: Get the buy in from user entry box
        {
            return 0;
        }

        public static void DisplayLobbyMessage(string message)
        {
            Text lobbyText = null;

            foreach (UIElement element in UIManager.lobbyUI) if (element.Name == "LobbyMessageText") lobbyText = (Text)element;

            if (lobbyText != null) lobbyText.Value =message; //TODO: Alter the position to remove the preceding spacing
        }
    }
}
