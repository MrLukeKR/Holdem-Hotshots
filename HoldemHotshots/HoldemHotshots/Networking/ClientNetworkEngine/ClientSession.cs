using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace HoldemHotshots
{
    class ClientSession
    {

        private ServerConnection connection;

        public ClientSession(String address,int portNumber)
        {
            Socket connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IPv4);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(address), portNumber);
            connectionSocket.Connect(endpoint);

            this.connection = new ServerConnection(connectionSocket);
        }

        public void init()
        {
            CommandListenerThread commandlistenerthread = new CommandListenerThread(this.connection);
            commandlistenerthread.Start();

        }

    }
}
