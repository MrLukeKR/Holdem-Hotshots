using System;
using System.Collections.Generic;
using Urho;
using Urho.Actions;
using Urho.Audio;
using Urho.Gui;
using Urho.Resources;

namespace HoldemHotshots{
  //This class uses getter and setter functions, where as other parts of the
  //code base use the C# style of writing the getter and setter into the
  //property declaration - need discussion on consistant style.
  
  class Table{
    private Deck deck = new Deck();
    public List<Card> hand { get; } = new List<Card>();
    private Pot pot = new Pot();
        Node soundnode;
        SoundSource sound;

        private Room room { get;  set; }
        
        public Table(Room room) {
            setRoom(room);
            initSound();
   
            deck.shuffle();
        }
        
        
        private void initSound()
        {
                soundnode = SceneManager.hostScene.GetChild("SFX", true);
                sound = soundnode.GetComponent<SoundSource>(true);
        }
        
        public void flop() { for (int i = 0; i < 3; i++) dealToTable(i); }

        public void dealToTable(int index)
        {
            Card newCard;
            Node newCardNode;

            hand.Add(deck.TakeCard());
            newCard = hand[index];
            newCardNode = newCard.getNode();
            newCardNode.Position = Card.cardTableDealingPos;

            doAnimation(index, newCard, newCardNode);
        }

        private void doAnimation(int index, Card newCard, Node newCardNode)
        {
            Application.InvokeOnMain(new Action(() =>
            {
                SceneManager.hostScene.AddChild(newCardNode);
                animateCardDeal(index, newCard);
            }
            ));
        }

        private void animateCardDeal(int index, Card card)
        {
            Console.WriteLine(card.ToString());
            card.getNode().RunActions(new Urho.Actions.Parallel(new RotateBy(0f, 0, 0, 90), new MoveTo(0.1f, Card.cardTablePositions[index])));
            sound.Play(UIManager.cache.GetSound("Sounds/Swish.wav"));
            //Need to add this to some form of copyright message in the App: http://www.freesfx.co.uk
        }
        
        public void dealToPlayers(){
            ServerPlayer currPlayer = null;
            for (int i = 0; i < room.getRoomSize(); i++)
            {
                currPlayer = room.getPlayer(i);
                Console.WriteLine("Dealing card to " + currPlayer.getName());
                currPlayer.GiveCard(deck.TakeCard());
            }
        }

    public void placeBets() {
            Console.WriteLine("Room size is " + room.getRoomSize());
            for (int i = 0 ; i < room.getRoomSize(); i++) room.getPlayer(i).takeTurn();
        }

    public void showdown() {
            
        }

    internal void setRoom(Room room) { this.room = room; }
  }
}
