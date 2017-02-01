using System;
using System.Collections.Generic;
using System.Text;

namespace TexasHoldemPoker.Game.NetworkEngine
{
    interface MultiplayerSessionInterface
    {
        public static ServerNetworkEngine getinstance();
        public void init();
    }
}
