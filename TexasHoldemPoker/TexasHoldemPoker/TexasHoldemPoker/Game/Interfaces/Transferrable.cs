using System;
using System.Collections.Generic;
using System.Text;
using TexasHoldemPoker.Game.PokerObjects;

namespace TexasHoldemPoker.Game.Interfaces
{
    interface Transferrable
    {
        void transfer(GameEntity entity);
    }
}
