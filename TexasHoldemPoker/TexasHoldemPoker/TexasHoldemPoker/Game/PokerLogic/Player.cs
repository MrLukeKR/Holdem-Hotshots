using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace TexasHoldemPoker
{
    class Player
    {
        String name;
        uint chips;
        public List<Card> hand { get; } = new List<Card>();

        Socket connection;
        
        private bool folded = false;

        public Player(String name, uint startBalance, Socket connection)
        {
            this.name = name;
            chips = startBalance;
            this.connection = connection;
        }
        
        public override String ToString()
        {
            String playerInfo = name;
            
            return playerInfo;
        }

        public bool hasFolded()
        {
            return folded;
        }

        public void call()
        {

        }

        public void allIn()
        {
        }

        public void check()
        {

        }

        public void fold()
        {
            Console.WriteLine(name + " folded");
            folded = true;
        }

        internal IEnumerable<Card> getCards()
        {
            throw new NotImplementedException();
        }

        public void giveChips(uint amount)
        {
            chips += amount;
        }

        public uint takeChips(uint amount)
        {
            if (chips >= amount)
            {
                chips -= amount;
                return amount;
            }
            else
                return 0;
        }

        public void takeTurn()
        {
            Console.WriteLine(name + "'s turn:\n");
            printHand();

            //Wait for input - Need to block here to stop the game loop continuing
        }
        
        public void payBlind(bool isBigBlind)
        {
           
        }
        
        private void printHand()
        {
            for (int i = 0; i < hand.Count(); i++)
                Console.WriteLine(hand[i].ToString());
            Console.WriteLine();
        }

        public String getName()
        {
            return name;
        }

        public uint getChips()
        {
            return chips;
        }
    }
}
