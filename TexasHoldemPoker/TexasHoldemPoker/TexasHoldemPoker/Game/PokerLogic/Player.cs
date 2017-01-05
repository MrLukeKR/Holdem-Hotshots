using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using TexasHoldemPoker.Game.Utils;
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
        private UI UI;

        private bool inputReceived = false;

    public Player(String name, uint startBalance, Socket connection){
      this.name = name;
      chips = startBalance;
      this.connection = connection;
    }

        public  Scene initPlayerScene(ResourceCache cache, UI UI)
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

            this.UI = UI;

            UI.Root.AddChild(coords);
            UI.Root.AddChild(statusInfoText);

            Application.Current.Input.TouchBegin += Input_TouchBegin;
            Application.Current.Input.TouchEnd += Input_TouchEnd;
            Application.Current.Input.TouchMove += Input_TouchMove;

            return playerScene;
        }
        
        public Camera getCamera()
        {
            if (camera == null)
                System.Console.WriteLine("Camera is null");
            return camera;
        }

        private void ViewCards()
        {
            playerScene.GetChild("Card1", true).RunActions(new MoveTo(.1f, Card.card1ViewingPos));
            playerScene.GetChild("Card2", true).RunActions(new MoveTo(.1f, Card.card2ViewingPos));
        }


        private void ToggleActionMenu()
        {
            //TODO: Implement these buttons once Xinyi has created the graphics for them
            UI.Root.GetChild("CheckButton", true).Visible = !UI.Root.GetChild("CheckButton", true).Visible;
            UI.Root.GetChild("FoldButton", true).Visible = !UI.Root.GetChild("FoldButton", true).Visible;
            UI.Root.GetChild("RaiseButton", true).Visible = !UI.Root.GetChild("RaiseButton", true).Visible;
            UI.Root.GetChild("CallButton", true).Visible = !UI.Root.GetChild("CallButton", true).Visible;
            UI.Root.GetChild("AllInButton", true).Visible = !UI.Root.GetChild("AllInButton", true).Visible;
        }

        private void HoldCards()
        {
            var card1 = playerScene.GetChild("Card1", true);
            var card2 = playerScene.GetChild("Card2", true);
            if (card1.Position != Card.card1HoldingPos)
                card1.RunActions(new MoveTo(.1f, Card.card1HoldingPos));
            if (card2.Position != Card.card2HoldingPos)
                card2.RunActions(new MoveTo(.1f, Card.card2HoldingPos));
        }
        
        private void updateCoords()
        {
            var input = Application.Current.Input;
            TouchState state = input.GetTouch(0);
            var pos = state.Position;
            var coordsNode = UI.Root.GetChild("coords", true);
            var coords = (Text)coordsNode;
            Vector3 a = WorldNavigationUtils.GetScreenToWorldPoint(pos, 15f, camera);
            coords.Value = "X:" + pos.X + " Y: " + pos.Y + "\nWS: " + a;
        }

        private void Input_TouchEnd(TouchEndEventArgs obj) { HoldCards(); }
        private void Input_TouchMove(TouchMoveEventArgs obj) { updateCoords(); }
        private void Input_TouchBegin(TouchBeginEventArgs obj)
        {
            Node tempNode = WorldNavigationUtils.GetNodeAt(Application.Current.Input.GetTouch(0).Position, playerScene);
            if (tempNode != null)
                if (tempNode.Name.Contains("Card"))
                    ViewCards();
                else if (tempNode.Name.Contains("Chip"))
                    ToggleActionMenu();
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
