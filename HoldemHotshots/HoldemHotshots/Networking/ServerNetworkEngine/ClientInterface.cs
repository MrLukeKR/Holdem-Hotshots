using System;

namespace HoldemHotshots
{
    interface ClientInterface
    {
        String getName();
        void sendTooManyPlayers();
        void sendPlayerKicked();
        String getPlayerAction();
        int getBet();
    }
}