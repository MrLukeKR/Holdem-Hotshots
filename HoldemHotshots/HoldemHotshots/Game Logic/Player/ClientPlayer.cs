using HoldemHotshots.Managers;
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
        private uint latestBid = 0;

        public List<Card> hand { get; private set; } = new List<Card>();
        public uint chips { get; private set; } = 0;
        public ServerInterface connection { private get; set; }

        public readonly string name;

        public ClientPlayer(String name, uint startBalance)
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
			UIUtils.DisplayPlayerMessage("Preparing Game");
			UIUtils.UpdatePlayerBalance(chips);

			Application.Current.Input.TouchBegin    += Input_TouchBegin;
			Application.Current.Input.TouchEnd      += Input_TouchEnd;

            InitSound();
		}

        private void ViewCards()
        {
            if(hand[0] != null || hand.Count < 1)
                SceneManager.playScene.GetChild("Card1", true).RunActions(new MoveTo(.1f, Card.card1ViewingPos));

            if (hand[1] != null || hand.Count < 2)
                SceneManager.playScene.GetChild("Card2", true).RunActions(new MoveTo(.1f, Card.card2ViewingPos));
        }

        private void HoldCards()
        {
            if (hand.Count >= 1)
                if (hand[0] != null)
                {
                    var card1 = SceneManager.playScene.GetChild("Card1", true);

                    if (card1.Position != Card.card1HoldingPos)
                        card1.RunActions(new MoveTo(.1f, Card.card1HoldingPos));
                }

            if(hand.Count ==2)
               if (hand[1] != null)
                {
                    var card2 = SceneManager.playScene.GetChild("Card2", true);
                        
                    if (card2.Position != Card.card2HoldingPos)
                        card2.RunActions(new MoveTo(.1f, Card.card2HoldingPos));
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

                    cardNode.Position = Card.cardDealingPos;
                    cardNode.Name = "Card" + (index + 1);

                    SceneManager.playScene.AddChild(card.node);

                    if (index == 0)
                        cardNode.RunActions(new MoveTo(.5f, Card.card1HoldingPos));
                    else if (index == 1)
                        cardNode.RunActions(new MoveTo(.5f, Card.card2HoldingPos));
                }
            }
            ));
        }

        internal void ResetInterface()
        {
            foreach (Card card in hand)
            {
                SceneManager.playScene.RemoveChild(card.node);
                card.node.Dispose();
            }

            hand.Clear();
        }
        
        internal void GiveCard(int suit, int rank)
        {
            hand.Add(new Card((Card.Suit)suit, (Card.Rank)rank));
            animateCard(hand.Count - 1);
        }

        public void Call()
        {
            if (inputEnabled)
                if (latestBid <= chips)
                {
                    inputEnabled = false;
                    UIUtils.DisableIO();

                    if (latestBid == chips)
                        connection.sendAllIn();
                    else if (latestBid <  chips)
                        connection.sendCall();
                }
        }
        
        public void AllIn()
        {
            if (inputEnabled)
                if (latestBid <= chips)
                {
                    inputEnabled = false;
                    UIUtils.DisableIO();
                    connection.sendAllIn();
                }
                else
                    UIUtils.DisplayPlayerMessage("Insufficient chips!");
        }
        
        public void Check()
        {
            if (inputEnabled)
            {
                inputEnabled = false;
                UIUtils.DisableIO();
                connection.sendCheck();
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
                connection.sendFold();
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
                uint amount = latestBid + UIUtils.GetRaiseAmount(true);
                
                if (amount <= chips)
                {
                    inputEnabled = false;
                    UIUtils.DisableIO();

                    if (amount == chips)
                        connection.sendAllIn();
                    else
                        connection.sendRaise(amount);
                }
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
        }
    }
}