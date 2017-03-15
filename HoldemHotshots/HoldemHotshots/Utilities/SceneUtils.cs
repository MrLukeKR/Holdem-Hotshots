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
            Card.CARD_TABLE_POSITIONS[2] - new Vector3(2.75f, -4.5f, 0), //TODO: Normalise this to be some percentage of the screen size
            Card.CARD_TABLE_POSITIONS[2] - new Vector3(2.75f, -2.75f, 0),
            Card.CARD_TABLE_POSITIONS[2] - new Vector3(2.75f, -1.0f, 0),
            Card.CARD_TABLE_POSITIONS[2] - new Vector3(2.75f, 1.0f, 0),
            Card.CARD_TABLE_POSITIONS[2] - new Vector3(2.75f, 2.75f, 0),
            Card.CARD_TABLE_POSITIONS[2] - new Vector3(2.75f, 4.5f, 0),
    };

        public static readonly Vector3 PLAYER_CARD1_OFFSET = new Vector3(0.6f, -0.4f, 0);
        public static readonly Vector3 PLAYER_CARD2_OFFSET = new Vector3(0.6f, 0.4f, 0);

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

                foreach (Node node in SceneManager.hostScene.Children)
                    if (node.Name == playerName)
                        playerText = node.GetComponent<Text3D>();

                if (playerText != null)
                    playerText.Text = playerName + "\n" + information;
        }

        public static void ShowPlayerCards(int index, string playerName, string hand, Card card1, Card card2, bool folded)
        {
            Application.InvokeOnMain(new Action(() =>
            {
                card1.node.Rotate(new Quaternion(0, 0, 90), TransformSpace.Local);
                card1.node.Position = PLAYER_POSITIONS[index] + PLAYER_CARD1_OFFSET;
                card1.node.Scale = new Vector3(0.75f, 1.05f, 0);
                

                card2.node.Rotate(new Quaternion(0, 0, 90), TransformSpace.Local);
                card2.node.Position = PLAYER_POSITIONS[index] + PLAYER_CARD2_OFFSET;
                card2.node.Scale = new Vector3(0.75f, 1.05f, 0);

                SceneManager.hostScene.AddChild(card1.node);
                SceneManager.hostScene.AddChild(card2.node);

                if (folded)
                    UpdatePlayerInformation(playerName, "Folded");
                else
                    UpdatePlayerInformation(playerName, hand);
            }));   
        }

        public static void DisplayWinner(string playerName, string hand, Card card1, Card card2)
        {

        }

        public static void InitPlayerInformation(List<ServerPlayer> players)
        {

            Application.InvokeOnMain(new Action(() =>
            {
                int i = 0;
                foreach (ServerPlayer player in players)
                {
                    //This information will contain Player Name, Latest Action and at the end, the player's cards
                    var playerNameNode = SceneManager.hostScene.CreateChild(player.name);
                    var playerName = playerNameNode.CreateComponent<Text3D>();
                    playerName.Text = player.name;
                    playerName.TextAlignment = HorizontalAlignment.Center;
                    playerName.HorizontalAlignment = HorizontalAlignment.Center;
                    playerName.SetFont(Application.Current.ResourceCache.GetFont("Fonts/arial.ttf"), 20);
                    playerNameNode.Position = PLAYER_POSITIONS[i++];
                        if(i > 3)//TODO: Switch sides and rotation when each side has more than 3 players listed
                            playerNameNode.Rotate(new Quaternion(0, 0, 90),TransformSpace.Local);
                        else
                            playerNameNode.Rotate(new Quaternion(0, 0, -90), TransformSpace.Local);
                        //TODO: Dynamic allocation of player name positions based on the amount of players    
            }
            }));
        }
    }
}
