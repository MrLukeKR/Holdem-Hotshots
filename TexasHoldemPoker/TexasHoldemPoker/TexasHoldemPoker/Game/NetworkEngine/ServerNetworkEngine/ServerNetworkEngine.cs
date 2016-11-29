using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TexasHoldemPoker.Game.NetworkEngine.Server
{
    class AndroidNetworkEngine : NetworkEngineInterface
    {

        private Socket serverListener;
        private Lobby gameLobby;
        private int listenerPortNumber = 8741;

       public AndroidNetworkEngine()
        {




        }

        public void init()
        {

            //TODO : Create udp broadcast function and add it here
            //TODO : implement multithreading
            listenForConnections();

        }


        private void listenForConnections()
        {
            
            serverListener = new Socket(AddressFamily.InterNetwork ,SocketType.Stream,ProtocolType.Tcp);
            serverListener.Bind(new IPEndPoint(0, listenerPortNumber));

            while (true)
            {

                serverListener.Listen(0);

                Socket connection = serverListener.Accept();

                        
                //TODO : Call function to add PlayerConnection to lobby

            }


        }

        private void broadcastGameInfo()
        {





        }



    }
}
