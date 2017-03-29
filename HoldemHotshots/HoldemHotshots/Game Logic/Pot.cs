using HoldemHotshots.Utilities;
using System;

namespace HoldemHotshots.GameLogic
{
    class Pot
    {
        public uint amount { get; private set; } = 0;
        public uint smallBlind { get; set; }
        public uint bigBlind { get; set; }
        public uint stake { get; private set;}
        
        public Pot(uint smallBlind, uint bigBlind)
        {
            this.smallBlind = smallBlind;
            this.bigBlind = bigBlind;
        }
        
        public void ResetStake()
        {
            stake = 0;
        }

        public void PayIn(uint amount)
        {
            this.amount += amount;
            if(amount > stake)
                stake = amount;
            SceneUtils.UpdatePotBalance(this.amount);
            Console.WriteLine(amount + " paid into pot");
        }
        
        public uint cashout()
        {
            uint jackpot = amount;
            amount = 0;
            SceneUtils.UpdatePotBalance(this.amount);

            return jackpot;
        }
    }
}
