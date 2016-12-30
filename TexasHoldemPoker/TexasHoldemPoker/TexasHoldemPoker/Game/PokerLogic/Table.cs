
using System;
using System.Collections.Generic;
using Urho;
using Urho.Actions;

namespace TexasHoldemPoker
{
    class Table
    {
        private Deck deck = new Deck();
        public List<Card> hand { get; } = new List<Card>();
        private Scene tableScene;
        
        private Pot pot = new Pot();
        private Room room;
        private uint buyIn { get; set; }

        public Table(Room room, Scene scene, uint buyIn)
        {
            this.buyIn = buyIn;

            setRoom(room);
            setScene(scene);

            if (buyIn / 200 > 1)
            {
                pot.setSmallBlind(buyIn / 200);
                pot.setBigBlind(buyIn / 100);
            }
            else
            {
                pot.setSmallBlind(1);
                pot.setBigBlind(2);
            }

            deck.shuffle();
        }

        public void setScene(Scene scene)
        {
            tableScene = scene;
        }

        public void flop()
        {
            Card newCard;

            for (int i = 0; i < 3; i++) {
                deck.dealTo(hand);
                newCard = hand[i];
                newCard.getNode().Position = Poker.cardTableDealingPos;
                tableScene.AddChild(newCard.getNode());
                animateCardDeal(i, newCard);
            }
        }

        private void animateCardDeal(int index, Card card)
        {
            Console.WriteLine(card.ToString());
            switch(index){
                case 0:
                    card.getNode().RunActions(new Sequence(new MoveTo(0.1f, Poker.card1TablePos), new ScaleBy(1, 0.009f)));
                    break;
                case 1:
                    card.getNode().RunActions(new Sequence(new MoveTo(0.1f, Poker.card2TablePos), new ScaleBy(1, 0.009f)));
                    break;
                case 2:
                    card.getNode().RunActions(new Sequence(new MoveTo(0.1f, Poker.card3TablePos), new ScaleBy(1, 0.009f)));
                    break;
                case 3:
                    card.getNode().RunActions(new Sequence(new MoveTo(0.1f, Poker.card4TablePos), new ScaleBy(1, 0.009f)));
                    break;
                case 4:
                    card.getNode().RunActions(new Sequence(new MoveTo(0.1f, Poker.card5TablePos), new ScaleBy(1, 0.009f)));
                    break;
                }
        }
            
        public void dealCards()
        {
            deck.dealTo(hand);

            for(int i = 0; i < room.getRoomSize(); i++)
                deck.dealTo(room.getPlayer(i).hand);
        }

        public void placeBets()
        {

        }

        public void showdown()
        {

        }

        public void printHand()
        {
            Console.WriteLine("Table Cards:\n");
            for (int i = 0; i < hand.Count; i++)
                Console.WriteLine(hand[i].ToString());
            Console.WriteLine();
        }

        internal void setRoom(Room room)
        {
            this.room = room;
        }
        
    }
}
