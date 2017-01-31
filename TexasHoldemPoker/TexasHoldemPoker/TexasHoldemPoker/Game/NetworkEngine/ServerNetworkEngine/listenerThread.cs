using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TexasHoldemPoker.Game.NetworkEngine.ServerNetworkEngine
{
    class listenerThread : Thread
    {

        private Socket serverListener;
        private int listenerPortNumber = 8741;
        private IPEndPoint listenerEndpoint;
        private Room gameLobby;

        public listenerThread(Room gameLobby)
        {
            this.gameLobby = gameLobby;
        }

        public void Start()
        {
            setupSockets();
            listenForConnections();
        }

        private void setupSockets()
        {
            serverListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenerEndpoint = new IPEndPoint(0, listenerPortNumber);
            serverListener.Bind(listenerEndpoint);

        }

        private void listenForConnections()
        {
          
            while (true)
            {
                serverListener.Listen(0);
                Socket connection = serverListener.Accept();
                ClientConnection client = new ClientConnection(connection);

                if (gameLobby.getRoomSize() >= gameLobby.MaxRoomSize)
                {

                    client.sendTooManyPlayers();
                }
                else
                {
                    string name = client.askName();
                    gameLobby.addPlayer(new Player(name, 0, client));
                }
            }
        }
    }
}
