using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PokerLogic
{
    class Player
    {
        String name;
        Socket connection;

        List<Card> hand = new List<Card>();
        uint chips;

        Pot tablePot;
        
        private bool folded = false;

        public Player(String name, Socket connection)
        {
            this.name = name;
            this.connection = connection;
        }

        public void setPot(Pot pot)
        {
            tablePot = pot;
        }

        public override String ToString()
        {
            String playerInfo = name;
            
            return playerInfo;
        }

        public void giveCard(Card card)
        {
            hand.Add(card);
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
            tablePot.payIn(chips);
            chips = 0;
        }

        public void check()
        {

        }

        public void fold()
        {
            Console.WriteLine(name + " folded");
            folded = true;
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
            uint paid = 0;

            if (isBigBlind) paid = takeChips(tablePot.getBigBlind());
            else paid = takeChips(tablePot.getSmallBlind());

            if (paid == 0)
                fold();
            else
                tablePot.payIn(paid);
        }
        
        private void printHand()
        {
            for (int i = 0; i < hand.Count(); i++)
                Console.WriteLine(hand[i].ToString());
            Console.WriteLine();
        }
    }
}
