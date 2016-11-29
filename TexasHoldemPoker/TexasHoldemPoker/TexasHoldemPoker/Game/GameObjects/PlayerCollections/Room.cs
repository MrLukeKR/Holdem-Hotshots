using System;

namespace TexasHoldemPoker.Game.PokerObjects
{
    class Room
    {
        private Player[] players;
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
            players = lobby.getPlayers();
        }

        public void removePlayer(int id)
        {
            throw new NotImplementedException();
        }
    }
}
