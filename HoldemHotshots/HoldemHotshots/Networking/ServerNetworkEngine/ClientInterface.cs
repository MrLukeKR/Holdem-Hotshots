using System;

namespace HoldemHotshots
{
    public interface ClientInterface
    {
        String getName();
        void sendTooManyPlayers();
        void sendPlayerKicked();
        String getPlayerAction();
        int getBet();
    }
}