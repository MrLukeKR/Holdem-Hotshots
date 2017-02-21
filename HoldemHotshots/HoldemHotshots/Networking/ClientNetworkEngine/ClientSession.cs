using System;
using System.Net;
using System.Net.Sockets;

namespace HoldemHotshots
{
    class ClientSession
    {

        private ServerConnection connection;
        private ClientPlayer player = null;

        public ClientSession(String address,int portNumber,ClientPlayer player)
        {
            Socket connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(address), portNumber);
            connectionSocket.Connect(endpoint);

            connection = new ServerConnection(connectionSocket);
        }

        public void init()
        {
            CommandListenerThread commandlistenerthread = new CommandListenerThread(connection, player);
            commandlistenerthread.Start();
        }

    }
}
