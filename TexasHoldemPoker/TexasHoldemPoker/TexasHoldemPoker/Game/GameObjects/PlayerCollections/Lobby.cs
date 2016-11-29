using System;

namespace TexasHoldemPoker.Game.PokerObjects
{
    class Lobby : PlayerCollection
    {
        private uint numberOfPlayers;
        private Player[] players;
        private uint currentID = 0;
        
        public Lobby(uint numberOfPlayers)
        {
            this.numberOfPlayers = numberOfPlayers;
            players = new Player[numberOfPlayers];
            for (int i = 0; i < numberOfPlayers; i++)
                players[i] = new Player(currentID++, "SOCKET GOES HERE"); //TODO: Create with an IP socket
            
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

        public Player[] getPlayers()
        {
            return players;
        }
    }
}
