using HoldemHotshots.Networking.ServerNetworkEngine;
using System.Collections.Generic;

namespace HoldemHotshots.GameLogic.Player
{
    public class ServerPlayer
    {
        public uint chips { get; private set; }
        public uint currentStake { get; private set;}
        public List<Card> hand { get; private set; } = new List<Card>();
        public bool folded { get; private set; } = false;
        public bool hasTakenTurn = false;
        public string name;
        public ClientInterface connection;

        internal Pot pot { private get; set; }
        
        public ServerPlayer(ClientInterface connection)
        {
            this.connection = connection;
            GiveChips(1000); //TODO: Allow players to "purcahse" chips
        }
        
        public void GiveChips(uint amount)
        {
            chips += amount;
            connection.setChips(chips);
        }

        public void ResetStake()
        {
            currentStake = 0;
        }
        
        public uint TakeChips(uint amount)
        {
            if (chips >= amount)
            {
                chips -= amount;
                currentStake += amount;
                connection.setChips(chips);

                return amount;
            }
            else
                return 0;
        }

        internal void Reset()
        {
            hand.Clear();  

            if(chips > 0)
                folded = false;
            connection.ResetInterface();
        }

        public uint ApplyBlind(uint amount)
        {
            if (chips >= amount)
                return TakeChips(amount);
            else
                Fold();
            return 0;
        }

        internal bool IsConnected()
        {
            return connection.IsConnected();
        }

        public void TakeTurn()
        {
            if (!folded)
                connection.takeTurn();
        }

        internal void Kick()
        {
            connection.sendKicked();
        }
        
        internal void GiveCard(Card card)
        {
            hand.Add(card);
            connection.giveCard((int)card.suit, (int)card.rank);
        }

        internal void AnimateCard(int index)
        {
            connection.animateCard(index);
        }

        internal void Fold()
        {
            folded = true;
            hasTakenTurn = true;
        }

        internal void DisplayMessage(string message)
        {
            connection.DisplayMessage(message);
        }
    }
}