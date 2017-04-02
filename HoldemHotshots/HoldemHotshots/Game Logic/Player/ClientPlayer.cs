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
    public class ClientPlayer
    {
        private Node soundNode;
        private SoundSource sound;
        private bool inputEnabled = false;
        public uint highestBid = 0;
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

        private void InitSound()
        {
            soundNode   = SceneManager.playScene.GetChild("SFX", true);
            sound       = soundNode.GetComponent<SoundSource>(true);
        }

        public void Init()
		{
			UIUtils.UpdatePlayerBalance(chips);

            Application.Current.Input.TouchBegin    += Input_TouchBegin;
			Application.Current.Input.TouchEnd      += Input_TouchEnd;

            InitSound();
		}

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
        
        private void Input_TouchEnd(TouchEndEventArgs obj)
        {
            HoldCards();
        }

        private void Input_TouchBegin(TouchBeginEventArgs obj)
        {
            Node tempNode = PositionUtils.GetNodeAt(Application.Current.Input.GetTouch(0).Position, SceneManager.playScene);

            if (tempNode != null)
                if (tempNode.Name.Contains("Card"))
                    ViewCards();
        }

        public void animateCard(int index)
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

            highestBid = 0;
            playerBid = 0;
        }
        
        internal void GiveCard(int suit, int rank)
        {
            hand.Add(new Card((Card.Suit)suit, (Card.Rank)rank));
            animateCard(hand.Count - 1);
        }

        public void Call()
        {
            if (inputEnabled)
            {
                if (highestBid <= playerBid + chips && highestBid > 0)
                {
                    inputEnabled = false;
                    UIUtils.DisableIO();

                    if (highestBid == chips + playerBid)
                        connection.SendAllIn();
                    else if (highestBid < chips + playerBid)
                        connection.SendCall();
                }
                else
                    UIUtils.DisplayPlayerMessage("Insufficient Chips!");
            }
        }
        
        public void AllIn()
        {
            if (inputEnabled)
            {
                if (highestBid <= chips)
                {
                    inputEnabled = false;
                    UIUtils.DisableIO();
                    connection.SendAllIn();
                }
                else
                    UIUtils.DisplayPlayerMessage("Insufficient Chips!");
            }
        }
        
        public void Check()
        {
            if (inputEnabled && highestBid == 0)
            {
                inputEnabled = false;
                UIUtils.DisableIO();
                connection.SendCheck();
            }
        }

        public void SetBuyIn(int buyin)
        {
            //TODO: Write setBuyIn code
        }

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

        private void ClearCards()
        {
            for (int i = 0; i < 2; i++)
                hand[i].node.Remove();
            hand.Clear();
        }

        public void Raise()
        {
            if (inputEnabled)
            {
                uint amount = UIUtils.GetRaiseAmount(true);

                if (amount <= chips && amount > 0 && chips > 0)
                {
                    playerBid += amount;
                    inputEnabled = false;
                    UIUtils.DisableIO();

                    if (amount == chips)
                        connection.SendAllIn();
                    else
                        connection.SendRaise(amount);
                }
                else
                    UIUtils.DisplayPlayerMessage("Insufficient Chips!");
            }
        }
        
        public void SetChips(uint amount)
        {
            chips = amount;
            UIUtils.UpdatePlayerBalance(amount);
        }
        
        public void TakeTurn()
        {
            inputEnabled = true;
            UIUtils.EnableIO();
            UIUtils.ToggleCallOrCheck(highestBid);
            //TODO: Disable specific buttons depending on player balance and pot balance
        }
    }
}