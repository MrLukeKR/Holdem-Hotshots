using System;
using System.Collections.Generic;
using System.Text;
using MixedRealityPoker.Game.PokerObjects;

namespace TexasHoldemPoker.Game.PokerObjects
{
    class Room
    {
        private Player[] players;

        public Room(Lobby lobby)
        {
            populateRoom(lobby);

        }

        public void populateRoom(Lobby lobby)
        {
            players = lobby.getPlayers();
        }

        public void removePlayer(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
