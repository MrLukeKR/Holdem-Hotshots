using System;
using System.Collections.Generic;

namespace HoldemHotshots{
  public class ServerPlayer{
		String name;
		uint chips;
        Pot pot;
		public List<Card> hand { get; } = new List<Card>();
		public ClientInterface connection;
		private bool folded = false;

    public ServerPlayer(String name, ClientInterface connection){
      this.name = name;
      this.connection = connection;
            giveChips(1000); //TODO: Allow players to "purcahse" chips
        }
        
    public override String ToString(){
      String playerInfo = name;
      return playerInfo;
    }
    public bool hasFolded() { return folded; }
        
    internal IEnumerable<Card> getCards(){ return hand; }
    public void giveChips(uint amount) { chips += amount; connection.setChips(chips); }

        internal void SetPot(Pot pot) { this.pot = pot; }

        public uint takeChips(uint amount) {
      if (chips >= amount){
        chips -= amount;
                connection.setChips(chips);
        return amount;
      } else return 0;
    }

        internal void ResetInterface()
        {
            foreach(Card card in hand) { card.getNode().Dispose(); }
            connection.ResetInterface();
        }

        public void takeTurn()
        {
            if (!folded)
            {
               var reponse = connection.takeTurn();
               ServerCommandManager.getInstance((ClientConnection)connection, this).runCommand(reponse);
            }
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
            Console.WriteLine(name + " has folded");
        }

        internal void DisplayMessage(string message)
        {
            connection.DisplayMessage(message);
        }
    }
}
