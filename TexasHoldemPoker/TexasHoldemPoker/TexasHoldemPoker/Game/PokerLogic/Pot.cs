
using System;

namespace TexasHoldemPoker
{
    class Pot
    {
        private uint amount = 0;
        private uint sBlind, bBlind;

        public Pot()
        {

        }

        public Pot(uint smallBlind, uint bigBlind)
        {
            sBlind = smallBlind;
            bBlind = bigBlind;
        }

        public uint getSmallBlind()
        {
            return sBlind;
        }

        public uint getBigBlind()
        {
            return bBlind;
        }
        
        public void setSmallBlind(uint amount)
        {
            sBlind = amount;
        }

        public void setBigBlind(uint amount)
        {
            bBlind = amount;
        }

        public void payIn(uint amount)
        {
            this.amount += amount;
            Console.WriteLine(amount + " paid into pot");
        }

        public uint cashout()
        {
            uint jackpot = amount;
            amount = 0;
            return jackpot;
        }
    }
}
