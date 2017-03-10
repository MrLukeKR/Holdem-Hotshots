using HoldemHotshots.Utilities;
using System;

namespace HoldemHotshots{
  //This class uses getter and setter functions, where as other parts of the
  //code base use the C# style of writing the getter and setter into the
  //property declaration - need discussion on consistant style.

  class Pot{
    private uint amount = 0;
    private uint sBlind, bBlind;
        private uint latestBet = 0;
    public Pot() { }
    public Pot(uint smallBlind, uint bigBlind){
      sBlind = smallBlind;
      bBlind = bigBlind;
    }
    public uint getSmallBlind() { return sBlind; }
    public uint getBigBlind() { return bBlind; }
    public void setSmallBlind(uint amount) { sBlind = amount; }
    public void setBigBlind(uint amount) { bBlind = amount; }
    public void payIn(uint amount) {
      this.amount += amount;
            latestBet = amount;
            UIUtils.UpdatePotBalance(this.amount);
            Console.WriteLine(amount + " paid into pot");
    }
    public uint cashout() {
      uint jackpot = amount;
      amount = 0;
            UIUtils.UpdatePotBalance(this.amount);
      return jackpot;
    }

        public uint GetLatestBet()
        {
            return latestBet;
        }
    }
}
