namespace HoldemHotshots.Networking.ServerNetworkEngine
{
    public interface ClientInterface
    {
        void GetName();
        void SendTooManyPlayers();
        void SendPlayerKicked();
        void AnimateCard(int cardValue);
        void GiveCard(int suit, int rank);
        void TakeTurn();
        void SendKicked();
        void SendCurrentState(string state);
        void StartGame();
        void SetChips(uint chips);
        void DisplayMessage(string message);
        void ResetInterface();
        bool IsConnected();
        void SetPlayerBid(uint currentStake);
        void SetHighestBid(uint stake);
        void SendResetStakes();
    }
}