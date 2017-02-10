using System;
using System.Net;
using System.Net.Sockets;
using HoldemHotshots.ClientPlayer;

namespace HoldemHotshots
{
    class ClientSession
    {

        private ServerConnection connection;
        private ClientPlayer player;

        public ClientSession(String address,int portNumber,ClientPlayer player)
        {
            Socket connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(address), portNumber);
            connectionSocket.Connect(endpoint);

            this.connection = new ServerConnection(connectionSocket);
        }

        public void init()
        {
            CommandListenerThread commandlistenerthread = new CommandListenerThread(this.connection,this.player);
            commandlistenerthread.Start();
        }

    }
}
