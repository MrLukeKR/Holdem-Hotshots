using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldemPoker.Game.PokerObjects;

namespace MixedRealityPoker.Game.PokerObjects
{
    class Player
    {
        uint id;
        String name;
        String ip;
        uint chips;
        Card[] hand;

        public Player()
        {
            //Just going to do some player init stuff here
        }
    }
}
