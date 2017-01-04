using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Urho;
using Urho.Actions;
using Urho.Gui;
using Urho.Resources;

namespace TexasHoldemPoker{
  //This class uses getter and setter functions, where as other parts of the
  //code base use the C# style of writing the getter and setter into the
  //property declaration - need discussion on consistant style.
  
  class Table{
    private Deck deck = new Deck();
    public List<Card> hand { get; } = new List<Card>();
    private Scene tableScene;
        private ResourceCache cache;
        private UI UI;
    private Pot pot = new Pot();
    private Room room { get;  set; }
    private uint buyIn { get; set; }
    public Table(Room room, Scene scene, UI ui, uint buyIn) {
      this.buyIn = buyIn;
      setRoom(room);
      setScene(scene);
            setUI(ui);
      if (buyIn / 200 > 1) {
        pot.setSmallBlind(buyIn / 200);
        pot.setBigBlind(buyIn / 100);
      } else {
        pot.setSmallBlind(1);
        pot.setBigBlind(2);
      }
      deck.shuffle();
            
    }
        
    public void setScene(Scene scene) { tableScene = scene; }
        public void setUI(UI ui) { UI = ui; }
    public void flop(){
            for (int i = 0; i < 3; i++)
                dealToTable(i);
    }

        public void dealToTable(int index)
        {
            Card newCard;
            Node newCardNode;

            deck.dealTo(hand);
            newCard = hand[index];
            newCardNode = newCard.getNode();
            newCardNode.Position = Card.cardTableDealingPos;
            newCardNode.RunActions(new ScaleBy(0, 0.009f));

            Application.InvokeOnMain(new Action(() =>
            doAnimationOnMainThread(index, newCard, newCardNode))); //Do UI based stuff on the UI thread
        }

        private void doAnimationOnMainThread(int index, Card newCard, Node newCardNode)
        {
           tableScene.AddChild(newCardNode);
           animateCardDeal(index, newCard);
        }

        private void animateCardDeal(int index, Card card)
        {
            Console.WriteLine(card.ToString());
            switch (index)
            {
                case 0:
                    card.getNode().RunActions(new MoveTo(1f, Card.card1TablePos));
                    break;
                case 1:
                    card.getNode().RunActions(new MoveTo(1f, Card.card2TablePos));
                    break;
                case 2:
                    card.getNode().RunActions(new MoveTo(1f, Card.card3TablePos));
                    break;
                case 3:
                    card.getNode().RunActions(new MoveTo(1f, Card.card4TablePos));
                    break;
                case 4:
                   card.getNode().RunActions(new MoveTo(1f, Card.card5TablePos));
                    break;
            }
        }


        public void clearMessage()
        {
            displayMessage("");
        }

        public void displayMessage(String textMessage)
        {
            UIElement messageNode = UI.Root.GetChild("TableMessage", true);
            Text message = (Text)messageNode;

            message.Value = textMessage;
        }

        public void dealToPlayers(){
            for (int i = 0; i < room.getRoomSize(); i++)
            {
                deck.dealTo(room.getPlayer(i).hand);
                //room.getPlayer(i).animateCard(i);   //Needs scene generation (upon client side joining) to be able to animate
                //TODO: Client side information sending and animations
            }
        }

    public async Task placeBets() {
            Player curr;

            Application.InvokeOnMain(new Action(() => displayMessage("Place your bets!")));

            for (int i = 0 ; i < room.getRoomSize(); i++){
                curr = room.getPlayer(i);
                if (!curr.hasFolded())
                   await Task.Factory.StartNew(()=> { curr.takeTurn(); });
            }

            Application.InvokeOnMain(new Action(() => clearMessage()));
        }
    public void showdown() { }
    public void printHand(){
            displayMessage("Showdown!");
      Console.WriteLine("Table Cards:\n");
      for (int i = 0; i < hand.Count; i++)
        Console.WriteLine(hand[i].ToString());
      Console.WriteLine();
            clearMessage();
    }
    internal void setRoom(Room room) { this.room = room; }
  }
}
