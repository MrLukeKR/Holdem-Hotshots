using System;
using MixedRealityPoker.Game.PokerObjects;

namespace TexasHoldemPoker.Game.PokerObjects
{
    class Room
    {
        private Player[] players;

        public Room()
        {
        }

        public Room(Lobby lobby)
        {
            populateRoom(lobby);

        }

        public void populateRoom(Lobby lobby)
        {
            players = lobby.getPlayers();
        }

        public void removePlayer(int id)
        {
            throw new NotImplementedException();
        }
    }
}
