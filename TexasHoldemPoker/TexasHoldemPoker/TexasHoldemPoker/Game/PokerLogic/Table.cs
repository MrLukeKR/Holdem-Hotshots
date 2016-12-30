
using System;
using System.Collections.Generic;

namespace TexasHoldemPoker
{
    class Table
    {
        private Deck deck = new Deck();
        public List<Card> hand { get; } = new List<Card>();
        
        private Pot pot = new Pot();
        private Room room;

        public Table()
        {

        }
        
        public void flop()
        {
            for (int i = 0; i < 3; i++)
                deck.dealTo(hand);
        }

        public void dealCards(int amount)
        {
            for(int j = 0; j < amount; j++)
                for(int i = 0; i < room.getRoomSize(); i++)
                    deck.dealTo(room.getPlayer(i).hand);
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
