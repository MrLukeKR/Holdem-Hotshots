using System;
using System.Collections.Generic;
using Urho;
using Urho.Actions;

namespace HoldemHotshots{
  public class ClientPlayer{
		String name;
		uint chips;
		public List<Card> hand { get; } = new List<Card>();
		public ServerInterface connection;
		private bool folded = false;
        private bool inputEnabled = false;

    public ClientPlayer(String name, uint startBalance){
      this.name = name;
      chips = startBalance;
    }

		public void Init()
		{
			UIUtils.DisplayPlayerMessage("Preparing Game");
			UIUtils.UpdatePlayerBalance(chips);
			Application.Current.Input.TouchBegin += Input_TouchBegin;
			Application.Current.Input.TouchEnd += Input_TouchEnd;

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
        
        private void Input_TouchEnd(TouchEndEventArgs obj) { HoldCards(); }
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
                    Card card = hand[index];
                    Node cardNode = card.getNode();
                    Console.WriteLine("Assigned CardNode");
                    cardNode.Position = Card.cardDealingPos;
                    Console.WriteLine("Set card position");
                    cardNode.Name = "Card" + (index + 1);
                    Console.WriteLine("Named Card");

                    SceneManager.playScene.AddChild(card.getNode());
                    Console.WriteLine("Added Card to Scene");

                    if (index == 0)
                        cardNode.RunActions(new MoveTo(.5f, Card.card1HoldingPos));
                    else if (index == 1)
                        cardNode.RunActions(new MoveTo(.5f, Card.card2HoldingPos));
                }
            }));
        }

    public override String ToString(){
      String playerInfo = name;
      return playerInfo;
    }
    public bool hasFolded() { return folded; }

        internal void giveCard(int suit, int rank)
        {
            hand.Add(new Card((Card.Suit)suit, (Card.Rank)rank));
            animateCard(hand.Count - 1);
        }

        public void call()
        {
            if (inputEnabled)
            {
                //TODO: Call code
                inputEnabled = false;
                UIUtils.disableIO();

                connection.sendCall();
            }
        }
    public void allIn() {
            if (inputEnabled)
            {
                //TODO: All In Code
                inputEnabled = false;
                UIUtils.disableIO();

                connection.sendAllIn();
            }
        }
    public void check() {
            if (inputEnabled)
            {
                //TODO: Check code
                inputEnabled = false;
                UIUtils.disableIO();

                connection.sendCheck();
            }
        }

        internal void setBuyIn(int buyin)
        {
            //TODO: Write setBuyIn code
        }

        public void fold()
        {
            if (inputEnabled) { 
                Console.WriteLine(name + " folded");
                folded = true;
                inputEnabled = false;
                UIUtils.disableIO();

                connection.sendFold();
           }
        }

        public void raise()
        {
            if (inputEnabled)
            {
                Console.WriteLine(name + " raised");
                //TODO: Raise code

                inputEnabled = false;
                UIUtils.disableIO();

                connection.sendRaise(0); //TODO: Get raise amount
            }
        }

        internal IEnumerable<Card> getCards(){ return hand; }
    public void giveChips(uint amount) { chips += amount; }
    public uint takeChips(uint amount) {
      if (chips >= amount){
        chips -= amount;
        return amount;
      } else return 0;
    }

    public void takeTurn(){
            inputEnabled = true;

            UIUtils.enableIO();
    }

    public void payBlind(bool isBigBlind) { }

    public String getName() { return name; }
    public uint getChips() { return chips; }


    }
}
