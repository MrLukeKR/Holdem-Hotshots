using System;
using System.Collections.Generic;
using System.Text;

namespace TexasHoldemPoker.Game.NetworkEngine.ServerNetworkEngine
{
    interface ClientInterface
    {
        public String getName();
        public void sendTooManyPlayers();
        public void sendPlayerKicked();
        public void getPlayerAction();
        public int getBet();


    }
}
