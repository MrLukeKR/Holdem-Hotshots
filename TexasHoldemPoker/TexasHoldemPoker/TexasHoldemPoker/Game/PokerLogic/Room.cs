using System;
using System.Collections.Generic;

namespace PokerLogic
{
    class Room
    {
        private List<Player> players = new List<Player>();
        private int index = 0;

        public Room()
        {

        }

        public void addPlayer(Player player)
        {
            players.Add(player);
        }

        public void removePlayer(int index)
        {
            players.RemoveAt(index);
        }

        public int getRoomSize()
        {
            return players.Count;
        }

        public void removePlayer(Player player)
        {
            players.Remove(player);
        }

        public Player getNextPlayer()
        {
            Player currentPlayer = players[getIndex(true)];

            while (currentPlayer.hasFolded())
                currentPlayer = players[getIndex(true)];
        
            return currentPlayer;
        }

        public void rotatePlayers()
        {
            Player temp = players[0];
            players.RemoveAt(0);
            players.Add(temp);
        }

        private int getIndex(bool increment)
        {
            if (index == players.Count)
                index = 0;

            if (increment)
                return index++;
            else
                return index;
        }

        internal int countFolded()
        {
            int count = 0;
            for (int i = 0; i < players.Count; i++)
                if (players[i].hasFolded())
                    count++;
            return count;
        }

        public override String ToString()
        {
            String playerList = "PLAYERS IN ROOM:\n\n";

            for(int i = 0; i < players.Count; i ++)
            {
                playerList += players[i].ToString() + "\n";
            }

            return playerList;
        }
    }
}
