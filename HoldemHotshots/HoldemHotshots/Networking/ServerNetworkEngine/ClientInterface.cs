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
        void sendBuyIn(int buyIn);
        void sendKicked();
        void sendCurrentState(string state);
        void giveChips(uint chips);
        void takeChips(uint chips);
        void startGame();
        void returnToLobby();
    }
}