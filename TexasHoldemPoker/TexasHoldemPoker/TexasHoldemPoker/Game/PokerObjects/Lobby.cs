using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldemPoker.Game.PokerObjects;

namespace MixedRealityPoker.Game.PokerObjects
{
    class Lobby : PlayerCollection
    {
        private int numberOfPlayers;
        private Player[] players;

        internal Player[] getPlayers()
        {
            return players;
        }

        
        public Lobby(int numberOfPlayers)
        {
            this.numberOfPlayers = numberOfPlayers;
            players = new Player[numberOfPlayers];
        }
        
        public void addPlayer(Player player)
        {
            throw new NotImplementedException();
        }

        public void removePlayer(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
