using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
    private uint buyIn { get; set; }

    public Table(Room room, uint buyIn) {
      this.buyIn = buyIn;
      setRoom(room);
          //  initSound();
      if (buyIn / 200 > 1) {
        pot.setSmallBlind(buyIn / 200);
        pot.setBigBlind(buyIn / 100);
      } else {
        pot.setSmallBlind(1);
        pot.setBigBlind(2);
      }
            //sound.Play(cache.GetSound("Sounds/Shuffle.wav")); //TODO: Make a SoundManager class
            deck.shuffle();
        }
        
        private void initSound()
        {
            soundnode = SceneManager.hostScene.GetChild("SFX", true);
            sound = soundnode.GetComponent<SoundSource>(true);
            sound.SetSoundType(SoundType.Effect.ToString());
        }
        
        public void flop()
        {
            for (int i = 0; i < 3; i++)
            {
                dealToTable(i);
            }
        }

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
            switch (index)
            {
                case 0: card.getNode().RunActions(new Urho.Actions.Parallel(new RotateBy(0f, 0, 0, 90), new MoveTo(0.1f, Card.card1TablePos))); break;
                case 1: card.getNode().RunActions(new Urho.Actions.Parallel(new RotateBy(0f, 0, 0, 90), new MoveTo(0.1f, Card.card2TablePos))); break;
                case 2: card.getNode().RunActions(new Urho.Actions.Parallel(new RotateBy(0f, 0, 0, 90), new MoveTo(0.1f, Card.card3TablePos))); break;
                case 3: card.getNode().RunActions(new Urho.Actions.Parallel(new RotateBy(0f, 0, 0, 90), new MoveTo(0.1f, Card.card4TablePos))); break;
                case 4: card.getNode().RunActions(new Urho.Actions.Parallel(new RotateBy(0f, 0, 0, 90), new MoveTo(0.1f, Card.card5TablePos))); break;
            }
            //sound.Play(cache.GetSound("Sounds/Swish.wav")); //TODO: Make a sound manager
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

    public async Task placeBets() {
            ServerPlayer curr;

            for (int i = 0 ; i < room.getRoomSize(); i++){
                curr = room.getPlayer(i);
                if (!curr.hasFolded())
                   await Task.Factory.StartNew(()=> { curr.takeTurn(); });
            }
        }
    public void showdown() {

        }

    internal void setRoom(Room room) { this.room = room; }
  }
}
