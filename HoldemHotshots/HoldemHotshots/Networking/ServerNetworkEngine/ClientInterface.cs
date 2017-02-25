using System;

namespace HoldemHotshots
{
    public interface ClientInterface
    {
        String getName();
        void sendTooManyPlayers();
        void sendPlayerKicked();
        void animateCard(int cardValue);
        void giveCard(int suit, int rank);
        string takeTurn();
        void sendKicked();
        void sendCurrentState(string state);
        void startGame();
        void returnToLobby();
        void setChips(uint chips);
    }
}