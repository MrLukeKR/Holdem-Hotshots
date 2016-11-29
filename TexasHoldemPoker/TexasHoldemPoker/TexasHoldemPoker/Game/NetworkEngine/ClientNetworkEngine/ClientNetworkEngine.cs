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


        /*
         * Listens for commands from the server and executes them
         */

        public void waitForServerCommands()
        {

            //Initialize command buffer
            Byte[] commandBuffer;
            commandBuffer = new Byte[255];

            while (true)
            {

                //Get message
                int messageSize = connection.Receive(commandBuffer, 0, commandBuffer.Length, 0);
                Array.Resize(ref commandBuffer, messageSize);
                String command = Encoding.Default.GetString(commandBuffer);


                // TODO: Add commands here that the server will tell the client to do
                switch(command)
                {
                    case "COMMAND1":
                        //Do something
                        break;

                    default:
                        Console.Write("Client recieved a message from server that was not found");
                        break;
                }

            }


        }

    }

}
