using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Urho;
using Urho.Actions;
using Urho.Gui;
using Urho.Resources;

namespace TexasHoldemPoker{
  class Player{
    String name;
    uint chips;
    public List<Card> hand { get; } = new List<Card>();
    Socket connection;
    private bool folded = false;
    private Scene playerScene;
        private Node CameraNode;
        private Camera camera;

        private bool inputReceived = false;

    public Player(String name, uint startBalance, Socket connection){
      this.name = name;
      chips = startBalance;
      this.connection = connection;
    }

        public  Viewport initPlayerScene(ResourceCache cache, UI UI, Context Context)
        {
            playerScene = new Scene();

            playerScene.LoadXmlFromCache(cache, "Scenes/Player.xml");

            //TODO: Make the camera update when the scene is changed (EVENT)
            CameraNode = playerScene.GetChild("MainCamera", true);
            camera = CameraNode.GetComponent<Camera>();

            Text coords = new Text();
            coords.Name = "coords";
            coords.SetColor(new Color(1.0f, 1.0f, 1.0f, 1f));
            coords.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
            coords.Value = "X: 0, Y: 0";
            coords.VerticalAlignment = VerticalAlignment.Center;
            coords.HorizontalAlignment = HorizontalAlignment.Center;
            coords.Visible = true;

            var statusInfoText = new Text();
            statusInfoText.Name = "StatusInformationLabel";
            statusInfoText.SetColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));
            statusInfoText.SetFont(cache.GetFont("Fonts/arial.ttf"), 20);
            statusInfoText.HorizontalAlignment = HorizontalAlignment.Center;
            statusInfoText.VerticalAlignment = VerticalAlignment.Top;
            statusInfoText.SetPosition(0, statusInfoText.Height / 2);
            statusInfoText.Visible = true;
            statusInfoText.Value = getName() + " - $" + getChips();

            UI.Root.AddChild(coords);
            UI.Root.AddChild(statusInfoText);

            return new Viewport(Context, playerScene, camera, null);
        }

        public void setScene(Scene scene)
        {
            playerScene = scene;
        }

        public void animateCard(int index)
        {
            Card card = hand[index];
            Node cardNode = card.getNode();
            Console.WriteLine("Assigned CardNode");
            cardNode.Position = Card.card1DealingPos;
            Console.WriteLine("Set card position");
            cardNode.Name = "Card" + (index+1);
            Console.WriteLine("Named Card");

            playerScene.AddChild(card.getNode());
            Console.WriteLine("Added Card to Scene");

            if (index == 0)
                cardNode.RunActions(new MoveTo(.5f, Card.card1HoldingPos));
            else if(index == 1)
                cardNode.RunActions(new MoveTo(.5f, Card.card2HoldingPos));
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
    internal IEnumerable<Card> getCards(){ throw new NotImplementedException();}
    public void giveChips(uint amount) { chips += amount; }
    public uint takeChips(uint amount) {
      if (chips >= amount){
        chips -= amount;
        return amount;
      } else return 0;
    }

    public void takeTurn(){
      Console.WriteLine(name + "'s turn:\n");
      printHand();

            //TODO: Player UI enabling/showing of actions

            //inputEnabled = true;
            
                check();


            while (!inputReceived) ; // busy waiting

            inputReceived = false;
            //inputEnabled = false;
    }

    public void payBlind(bool isBigBlind) { }
    private void printHand(){
        for (int i = 0; i < hand.Count(); i++)
            Console.WriteLine(hand[i].ToString());
        Console.WriteLine();
    }

    public String getName() { return name; }
    public uint getChips() { return chips; }
  }
}
