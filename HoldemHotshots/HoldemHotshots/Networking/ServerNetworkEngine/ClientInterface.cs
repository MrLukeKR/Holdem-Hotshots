namespace HoldemHotshots.Networking.ServerNetworkEngine
{
    public interface ClientInterface
    {
        void getName();
        void sendTooManyPlayers();
        void sendPlayerKicked();
        void animateCard(int cardValue);
        void giveCard(int suit, int rank);
        void takeTurn();
        void sendKicked();
        void sendCurrentState(string state);
        void startGame();
        void returnToLobby();
        void setChips(uint chips);
        void DisplayMessage(string message);
        void ResetInterface();
        bool IsConnected();
    }
}