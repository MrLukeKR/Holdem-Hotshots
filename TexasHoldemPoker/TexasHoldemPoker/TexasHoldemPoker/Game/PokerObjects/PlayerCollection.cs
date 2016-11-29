using MixedRealityPoker.Game.PokerObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace TexasHoldemPoker.Game.PokerObjects
{
    interface PlayerCollection
    {
        void addPlayer(Player player);
        void removePlayer(int id);
    }
}
