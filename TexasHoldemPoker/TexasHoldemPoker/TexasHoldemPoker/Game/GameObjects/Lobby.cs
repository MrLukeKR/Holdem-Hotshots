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
            for (int i = 0; i < numberOfPlayers; i++)
                players[i] = new Player(0); //TODO: Create with an IP socket

        }
        
        public void addPlayer(Player player)
        {
            throw new NotImplementedException();
        }

        public void removePlayer(uint id)
        {
            for(int i = 0; i < numberOfPlayers; i ++)
                if (players[i].getID() == id)
                    for(int j = i; i < numberOfPlayers - 1; j++)
                        players[j] = players[j + 1];

            numberOfPlayers--;
            players[numberOfPlayers] = null;
        }
    }
}
