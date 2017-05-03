using HoldemHotshots.Managers;
using HoldemHotshots.Networking.ClientNetworkEngine;
using HoldemHotshots.Utilities;
using System;
using System.Collections.Generic;
using Urho;
using Urho.Actions;
using Urho.Audio;

namespace HoldemHotshots.GameLogic.Player
{
    /// <summary>
    /// Holds the information about the player on the client-side
    /// </summary>
    public class ClientPlayer
    {
        private Node soundNode;
        private SoundSource sound;
        private bool inputEnabled = false;
        public uint playerBid = 0;

        public List<Card> hand { get; private set; } = new List<Card>();
        public uint chips { get; private set; } = 0;
        public ServerInterface connection { get; set; }

        public readonly string name;

        public ClientPlayer(string name, uint startBalance)
        {
            this.name = name;
            chips = startBalance;
        }

        /// <summary>
        /// Initialises the scene's sound node
        /// </summary>
        private void InitSound()
        {
            soundNode   = SceneManager.playScene.GetChild("SFX", true);
            sound       = soundNode.GetComponent<SoundSource>(true);
        }

        /// <summary>
        /// Initialises the touchscreen input from the client side
        /// </summary>
        public void Init()
		{
			UIUtils.UpdatePlayerBalance(chips);

            Application.Current.Input.TouchBegin    += Input_TouchBegin;
			Application.Current.Input.TouchEnd      += Input_TouchEnd;

            InitSound();
		}

        /// <summary>
        /// Puts the player's cards into view and flips them to the visible side
        /// </summary>
        private void ViewCards()
        {
            if (hand[0] != null || hand.Count < 1)
            {
                SceneManager.playScene.GetChild("Card1", true).RunActions(new MoveTo(.1f, Card.CARD_1_VIEWING_POSITION));
                hand[0].ShowCard();
            }

            if (hand[1] != null || hand.Count < 2)
            {
                SceneManager.playScene.GetChild("Card2", true).RunActions(new MoveTo(.1f, Card.CARD_2_VIEWING_POSITION));
                hand[1].ShowCard();
            }
        }

        /// <summary>
        /// Returns the player's cards back to the hidden position in the corner of the screen
        /// </summary>
        private void HoldCards()
        {
            if (hand.Count >= 1)
                if (hand[0] != null)
                {
                    var card1 = SceneManager.playScene.GetChild("Card1", true);

                    if (card1.Position != Card.CARD_1_HOLDING_POSITION)
                        card1.RunActions(new MoveTo(.1f, Card.CARD_1_HOLDING_POSITION));

                    hand[0].HideCard();
                }

            if(hand.Count ==2)
               if (hand[1] != null)
                {
                    var card2 = SceneManager.playScene.GetChild("Card2", true);
                        
                    if (card2.Position != Card.CARD_2_HOLDING_POSITION)
                        card2.RunActions(new MoveTo(.1f, Card.CARD_2_HOLDING_POSITION));

                    hand[1].HideCard();
                }
        }
        
        /// <summary>
        /// Detects when the player has stopped touching the screen
        /// </summary>
        /// <param name="obj">Touch input event</param>
        private void Input_TouchEnd(TouchEndEventArgs obj)
        {
            HoldCards();
        }

        /// <summary>
        /// Detects when the player has started touching the screen and then shows the player's cards if they are touching them
        /// </summary>
        /// <param name="obj">Touch input event</param>
        private void Input_TouchBegin(TouchBeginEventArgs obj)
        {
            Node tempNode = PositionUtils.GetNodeAt(Application.Current.Input.GetTouch(0).Position, SceneManager.playScene);

            if (tempNode != null)
                if (tempNode.Name.Contains("Card"))
                    ViewCards();
        }

        /// <summary>
        /// Glides the card from the dealing to the holding position
        /// </summary>
        /// <param name="index">Which card to animate</param>
        public void AnimateCard(int index)
        {
            Application.InvokeOnMain(new Action(() =>
            {
                if (index >= 0 && index < hand.Count)
                {
                    sound.Play(UIManager.cache.GetSound("Sounds/Swish.wav"));

                    Card card = hand[index];
                    Node cardNode = card.node;

                    cardNode.Position = Card.CARD_DEALING_POSITION;
                    cardNode.Name = "Card" + (index + 1);

                    SceneManager.playScene.AddChild(card.node);

                    if (index == 0)
                        cardNode.RunActions(new MoveTo(.5f, Card.CARD_1_HOLDING_POSITION));
                    else if (index == 1)
                        cardNode.RunActions(new MoveTo(.5f, Card.CARD_2_HOLDING_POSITION));

                    card.HideCard();
                }
            }
            ));
        }

        /// <summary>
        /// Returns cards to the dealing position and removes them from the scene
        /// </summary>
        internal void ResetInterface()
        {
            UIUtils.DisplayPlayerMessage("");

            Application.InvokeOnMain(new Action(() =>
            {
                foreach (Card card in hand)
                {
                    card.node.RunActions(new MoveTo(.5f, Card.CARD_DEALING_POSITION));
                    card.node.Remove();
                }

                hand.Clear();
                
                SceneManager.CreatePlayScene();
                SceneManager.ShowScene(SceneManager.playScene);
            }));

            ClientManager.highestBid = 0;
            playerBid = 0;
        }
        
        /// <summary>
        /// Gives the player a card of a given suit and rank (strictly for display - this card is not a Remote Object from the server)
        /// </summary>
        /// <param name="suit">Suit of the card</param>
        /// <param name="rank">Rank of the card</param>
        internal void GiveCard(int suit, int rank)
        {
            hand.Add(new Card((Card.Suit)suit, (Card.Rank)rank));
            AnimateCard(hand.Count - 1);
        }

        /// <summary>
        /// Checks whether a "Call" can be done and then sends the command to the server
        /// </summary>
        public void Call()
        {
            if (inputEnabled)
            {
                if (ClientManager.highestBid > 0 && ClientManager.highestBid - playerBid <= chips && chips > 0)
                {
                    inputEnabled = false;
                    UIUtils.DisableIO();

                    if (ClientManager.highestBid == chips + playerBid)
                        connection.SendAllIn();
                    else if (ClientManager.highestBid < chips + playerBid)
                        connection.SendCall();
                }
                else
                    UIUtils.DisplayPlayerMessage("Insufficient Chips!");
            }
        }
        
        /// <summary>
        /// Checks whether an "All-In" can be done and then sends the command to the server
        /// </summary>
        public void AllIn()
        {
            if (inputEnabled)
            {
                if (ClientManager.highestBid <= chips && chips > 0)
                {
                    inputEnabled = false;
                    UIUtils.DisableIO();
                    connection.SendAllIn();
                }
                else
                    UIUtils.DisplayPlayerMessage("Insufficient Chips!");
            }
        }
        
        /// <summary>
        /// Sends a "Check" command to the server
        /// </summary>
        public void Check()
        {
            if (inputEnabled && ClientManager.highestBid == 0)
            {
                inputEnabled = false;
                UIUtils.DisableIO();
                connection.SendCheck();
            }
        }

        /// <summary>
        /// Sets the buy-in for the game
        /// </summary>
        /// <param name="buyin">Buy-in amount</param>
        public void SetBuyIn(int buyin)
        {
            //TODO: Write setBuyIn code
        }

        /// <summary>
        /// Sends a "Fold" command to the server 
        /// </summary>
        public void Fold()
        {
            if (inputEnabled)
            { 
                inputEnabled = false;
                UIUtils.DisableIO();
                connection.SendFold();
                ClearCards();
           }
        }

        /// <summary>
        /// Removes all cards from the client's hand
        /// </summary>
        private void ClearCards()
        {
            for (int i = 0; i < 2; i++)
                hand[i].node.Remove();
            hand.Clear();
        }

        /// <summary>
        /// Checks if the player has enough chips to perform the set "Raise" command and then sends the command to the server
        /// </summary>
        public void Raise()
        {
            if (inputEnabled)
            {
                uint amount = UIUtils.GetRaiseAmount(true);
                uint difference = ClientManager.highestBid - playerBid;
                uint total = difference + amount;

                if (amount > 0 && chips > 0 && total <= chips)
                {
                    playerBid += total;
                    inputEnabled = false;
                    UIUtils.DisableIO();

                    if (total == chips)
                        connection.SendAllIn();
                    else
                        connection.SendRaise(amount);
                }
                else
                    UIUtils.DisplayPlayerMessage("Insufficient Chips!");
            }
        }
        
        /// <summary>
        /// Sets the amount of chips the player has
        /// </summary>
        /// <param name="amount"></param>
        public void SetChips(uint amount)
        {
            chips = amount;
            UIUtils.UpdatePlayerBalance(amount);
        }
        
        /// <summary>
        /// Enables the input of the screen and buttons so the player can take their turn
        /// </summary>
        public void TakeTurn()
        {
            if (ClientManager.highestBid == 0)
                playerBid = 0;

            inputEnabled = true;
            UIUtils.EnableIO();
            
            UIUtils.ToggleCallOrCheck(ClientManager.highestBid);
        }
    }
}