using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedRealityPoker.Game.PokerObjects
{
    class Lobby
    {
        private int numberOfPlayers;
        private Player[] players;

        public Lobby(int numberOfPlayers)
        {
            this.numberOfPlayers = numberOfPlayers;
            players = new Player[numberOfPlayers];
        }
    }
}
