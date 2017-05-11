namespace HoldemHotshots.Networking.ClientNetworkEngine
{
    public interface ServerInterface
    {
        void SendFold();
        void SendRaise(uint amount);
        void SendCheck();
        void SendAllIn();
        void SendCall();
    }
}