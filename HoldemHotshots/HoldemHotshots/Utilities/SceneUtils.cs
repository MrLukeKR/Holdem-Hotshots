using HoldemHotshots.GameLogic;
using HoldemHotshots.GameLogic.Player;
using HoldemHotshots.Managers;
using System;
using System.Collections.Generic;
using Urho;
using Urho.Gui;

namespace HoldemHotshots.Utilities
{
    class SceneUtils
    {
        public static readonly Vector3[] PLAYER_POSITIONS = { //TODO: Initialise these positions
            Card.CARD_TABLE_POSITIONS[2] - (Card.CARD_TABLE_POSITIONS[2] - Card.CARD_TABLE_POSITIONS[1]),
            Card.CARD_TABLE_POSITIONS[2] + (Card.CARD_TABLE_POSITIONS[2] - Card.CARD_TABLE_POSITIONS[1]),
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 0.0f),
    };
        
        public static void UpdatePotBalance(uint amount)
        {
            Text3D potText = null;

            Application.InvokeOnMain(new Action(() =>
            {
                foreach (Node node in SceneManager.hostScene.Children)
                    if (node.Name == "PotInfoText")
                        potText = node.GetComponent<Text3D>();

                if (potText != null)
                    potText.Text = "Pot\n$" + amount;
            }));
        }

        public static void UpdatePlayerInformation(string playerName, string information)
        {
            Text3D playerText = null;

            Application.InvokeOnMain(new Action(() =>
            {
                foreach (Node node in SceneManager.hostScene.Children)
                    if (node.Name == playerName)
                        playerText = node.GetComponent<Text3D>();

                if (playerText != null)
                    playerText.Text = playerName + "\n" + information;
            }));
        }

        public static void ShowPlayerCards(string playerName, Card card1, Card card2)
        {

        }

        public static void DisplayWinner(string playerName, string hand, Card card1, Card card2)
        {

        }

        public static void InitPlayerInformation(List<ServerPlayer> players)
        {
            int i = 0;
            foreach (ServerPlayer player in players)
            {
                //This information will contain Player Name, Latest Action and at the end, the player's cards
                var playerNameNode = SceneManager.hostScene.CreateChild(player.name);
                var playerName = playerNameNode.CreateComponent<Text3D>();
                playerName.Text = player.name;
                playerName.TextAlignment = HorizontalAlignment.Center;
                playerName.SetFont(Application.Current.ResourceCache.GetFont("Fonts/arial.ttf"), 40);
                playerNameNode.Position = PLAYER_POSITIONS[i++];
                //TODO: Dynamic allocation of player name positions based on the amount of players
            }
        }
    }
}
