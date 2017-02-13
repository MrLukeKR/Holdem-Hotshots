using System;
using System.Collections.Generic;
using Urho;
using Urho.Actions;

namespace HoldemHotshots{
  public class ClientPlayer{
		String name;
		uint chips;
		public List<Card> hand { get; } = new List<Card>();
		public ClientInterface connection;
		private bool folded = false;
        private bool inputReceived = false;

    public ClientPlayer(String name, uint startBalance, ClientInterface connection){
      this.name = name;
      chips = startBalance;
      this.connection = connection;
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
        }

    public override String ToString(){
      String playerInfo = name;
      return playerInfo;
    }
    public bool hasFolded() { return folded; }

    public void call()
        {
            //TODO: Call code
            inputReceived = true;
        }
    public void allIn() {
            //TODO: All In Code
            inputReceived = true;
        }
    public void check() {
            //TODO:Check code
            inputReceived = true;
        }
    public void fold(){
            Console.WriteLine(name + " folded");
            folded = true;
            inputReceived = true;
        }

    internal IEnumerable<Card> getCards(){ return hand; }
    public void giveChips(uint amount) { chips += amount; }
    public uint takeChips(uint amount) {
      if (chips >= amount){
        chips -= amount;
        return amount;
      } else return 0;
    }

    public String takeTurn(){
      Console.WriteLine(name + "'s turn:\n");

            //TODO: Player UI enabling/showing of actions

            //enableInput();
            
            while (!inputReceived) ; // busy waiting

            inputReceived = false;
            //disableInput();

            return "";
    }

    public void payBlind(bool isBigBlind) { }

    public String getName() { return name; }
    public uint getChips() { return chips; }
  }
}
