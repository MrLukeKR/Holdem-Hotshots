using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace TexasHoldemPoker.Game.NetworkEngine.ServerNetworkEngine
{
    /*
     * This class is a wrapper for the client socket and contains commands that can be sent to the client
     */

    class ClientConnection
    {
        Socket connection;

        public ClientConnection(Socket connection)
        {
            this.connection = connection;
        }

        private void sendCommand(String command){

            byte[] messageBuffer = Encoding.ASCII.GetBytes(command);
            connection.Send(command);

        }

    }
}
