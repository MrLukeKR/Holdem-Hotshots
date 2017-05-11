using HoldemHotshots.Networking.ClientNetworkEngine;

namespace HoldemHotshots.Managers
{
    /// <summary>
    /// Contains Client networking information with static accessibility
    /// </summary>
    static class ClientManager
    {
        static public ClientSession session;
        static public string serverAddress = "";
        static public string serverPort = "";
        static public string serverKey = "";
        static public string serverIV = "";
        static public uint highestBid = 0;
    }
}
