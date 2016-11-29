using MixedRealityPoker.Game.PokerObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace TexasHoldemPoker.Game.PokerObjects
{
    class Game
    {
        Room room = new Room();

        public void PopulateGame(Lobby lobby)
        {
            room = lobby;
        }
    }
}
