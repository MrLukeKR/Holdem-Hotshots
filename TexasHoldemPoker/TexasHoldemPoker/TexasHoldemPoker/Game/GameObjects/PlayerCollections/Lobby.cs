using System;

namespace TexasHoldemPoker.Game.PokerObjects
{
    class Lobby : PlayerCollection
    {
        static private uint MAX_NUMBER_OF_PLAYERS = 6;
        private uint numberOfPlayers;
        private Player[] players;
        private uint currentID = 0;

        ///<summary>
        ///The lobby constructor initialises the players array, gives each new player an ID and assigns it a socket
        /// </summary>
        
        public Lobby(uint numberOfPlayers)
        {
            this.numberOfPlayers = numberOfPlayers;
            players = new Player[numberOfPlayers];

            for (int i = 0; i < numberOfPlayers; i++)
                players[i] = new Player(currentID++, "SOCKET GOES HERE"); //TODO: Create with an IP socket
            
        }
        
        public void addPlayer(Player player)
        {
          //TODO: Create new array of numberOfPlayers + 1, add the new player to the end, free() players and assign it to the new array
        }

        public void removePlayer(uint id)
        {
            //TODO: Create new array of numberOfPlayers - 1, add the first numberOfPlayers-1 players, free() players and assign it to the new array
        }

        public Player[] getPlayers()
        {
            return players;
        }
    }
}
