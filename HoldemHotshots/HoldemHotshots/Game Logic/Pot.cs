using HoldemHotshots.Utilities;
using System;

namespace HoldemHotshots.GameLogic
{
    class Pot
    {
        public uint amount { get; private set; } = 0;
        public uint smallBlind { get; set; }
        public uint bigBlind { get; set; }
        public uint latestBet { get; private set; } = 0;
        
        public Pot(uint smallBlind, uint bigBlind)
        {
            this.smallBlind = smallBlind;
            this.bigBlind = bigBlind;
        }
        
        public void PayIn(uint amount)
        {
            this.amount += amount;
            latestBet = amount;
            UIUtils.UpdatePotBalance(this.amount);
            Console.WriteLine(amount + " paid into pot");
        }
        
        public uint cashout()
        {
            uint jackpot = amount;
            amount = 0;
            UIUtils.UpdatePotBalance(this.amount);

            return jackpot;
        }
    }
}
