using System;
using System.Net;
using System.Net.Sockets;

namespace HoldemHotshots
{
    class ClientSession
    {

        private ServerConnection connection;
        public ClientPlayer player = null; //TODO: Try and privatise this

        public ClientSession(String address, int portNumber, ClientPlayer player)
        {
            this.player = player;
            Socket connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(address), portNumber);
            connectionSocket.Connect(endpoint);

            connection = new ServerConnection(connectionSocket);

            player.connection = connection;
        }

        public void init()
        {
            CommandListenerThread commandlistenerthread = new CommandListenerThread(connection, player);
            commandlistenerthread.Start();
        }

    }
}
