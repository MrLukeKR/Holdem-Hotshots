using System;
using System.Collections.Generic;
using Urho.Gui;

namespace HoldemHotshots
{
	public static class UIUtils
	{
		private static void enableAndShow(UIElement element)
		{
			element.Visible = true;
			element.Enabled = true;
		}

		private static void disableAndHide(UIElement element)
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
            String playerList = "";

            Text playerNames = null;

            var max = room.MaxRoomSize;
            var curr = room.getRoomSize();

            for (int i = 0; i < curr - 1; i++) playerList += room.getPlayer(i).getName() + "\n";
            for (int i = curr; curr < max; i++) playerList += "Waiting for Player " + i + "...\n";

            foreach (UIElement element in UIManager.lobbyUI) if (element.Name == "PlayerNames") playerNames = (Text)element;

            if(playerNames != null)
                playerNames.Value = playerList;
        }
    }
}
