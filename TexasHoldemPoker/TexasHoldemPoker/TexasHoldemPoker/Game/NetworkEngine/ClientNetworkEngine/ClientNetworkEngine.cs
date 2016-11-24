using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace TexasHoldemPoker.Game.NetworkEngine.ClientNetworkEngine
{
    class ClientNetworkEngine
    {

        Socket connection;

        /*
         * Takes a ipv4 address and portnumber and sets a connection to the server
         */

        public void connectToServer(String address,int portnumber)
        {

           Socket connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.IPv4);

           IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(address),portnumber);

            connection.Connect(endpoint);

        }

        public void waitForServerCommands()
        {

            while (true)
            {




            }


        }

    }

}
