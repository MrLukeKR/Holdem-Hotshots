using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MixedRealityPoker.Game.PokerObjects;

namespace TexasHoldemPoker.Game.NetworkEngine.AndroidNetworkEngine
{
    class AndroidNetworkEngine : NetworkEngineInterface
    {

        private Socket serverListener;
        private Lobby gameLobby;
        private int listenerPortNumber = 8741;

       public AndroidNetworkEngine()
        {




        }


        private void listenForConnections()
        {
            
            serverListener = new Socket(AddressFamily.InterNetwork ,SocketType.Stream,ProtocolType.Tcp);
            serverListener.Bind(new IPEndPoint(0, listenerPortNumber));

            while (true)
            {

                serverListener.Listen(0);

                Socket connection = serverListener.Accept();

                PlayerConnection player = new PlayerConnection(connection);

                //TODO : Call function to add PlayerConnection to lobby

            }


        }

        private void broadcastGameInfo()
        {





        }



    }
}
