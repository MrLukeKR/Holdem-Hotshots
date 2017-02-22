using System;
using System.Collections.Generic;
using System.Linq;
using Urho;
using Urho.Actions;

namespace HoldemHotshots{
  public class ServerPlayer{
		String name;
		uint chips;
		public List<Card> hand { get; } = new List<Card>();
		public ClientInterface connection;
		private bool folded = false;

    public ServerPlayer(String name, uint startBalance, ClientInterface connection){
      this.name = name;
      chips = startBalance;
      this.connection = connection;
    }
        
    public override String ToString(){
      String playerInfo = name;
      return playerInfo;
    }
    public bool hasFolded() { return folded; }
        
    internal IEnumerable<Card> getCards(){ return hand; }
    public void giveChips(uint amount) { chips += amount; }
    public uint takeChips(uint amount) {
      if (chips >= amount){
        chips -= amount;
        return amount;
      } else return 0;
    }

        public void takeTurn()
        {
            connection.takeTurn();
        }

    public void payBlind(bool isBigBlind) { }

    public String getName() { return name; }
    public uint getChips() { return chips; }

        internal void GiveCard(Card card)
        {
            hand.Add(card);
            connection.giveCard((int)card.suit, (int)card.rank);
        }

        internal void animateCard(int index)
        {
            connection.animateCard(index);
        }

        internal void fold()
        {
            folded = true;
        }
    }
}
