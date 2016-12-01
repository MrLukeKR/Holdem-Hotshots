using System;

namespace TexasHoldemPoker.Game.PokerObjects
{
    class Room
    {
        private Player[] players; // Convert to cyclic queue linked list
        private Table table;

        public Room()
        {
            table = new Table();
        }

        public Room(Lobby lobby)
        {
            populateRoom(lobby);
        }

        public void populateRoom(Lobby lobby)
        {
            players = lobby.getPlayers(); // Convert to cyclic queue linked list
        }

        public void removePlayer(int id)
        {
            throw new NotImplementedException();
        }

        internal int getNumberOfPlayers()
        {
            return players.Length;
        }
    }
}
