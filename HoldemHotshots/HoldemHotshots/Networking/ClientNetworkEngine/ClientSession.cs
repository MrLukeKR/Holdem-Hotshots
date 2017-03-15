using HoldemHotshots.GameLogic.Player;
using System.Net;
using System.Net.Sockets;

namespace HoldemHotshots.Networking.ClientNetworkEngine
{
    class ClientSession
    {
        private readonly Socket connectionSocket;
        private readonly IPEndPoint endpoint;
        public  readonly ClientPlayer player;

        public ClientSession(string address, int portNumber, ClientPlayer player)
        {
            this.player = player;
            connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            endpoint = new IPEndPoint(IPAddress.Parse(address), portNumber);
            
            player.connection = new ServerConnection(connectionSocket);
        }

        public void Init()
        {
            new CommandListenerThread((ServerConnection)player.connection, player).Start();
        }

        public bool Connect()
        {
            if (!connectionSocket.Connected)
                    connectionSocket.Connect(endpoint);
                else
                    return false;

            return true;
        }
    }
}
