using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Urho;

namespace HoldemHotshots
{
    class ListenerThread
    {
        private Socket serverListener;
        private int listenerPortNumber = 8741;
        private IPEndPoint listenerEndpoint;
        private Room gameLobby;

        public ListenerThread(Room gameLobby)
        {
            this.gameLobby = gameLobby;
        }
        public void Start()
        {
            Console.WriteLine("Listener Starting");
            setupSockets();
            listenForConnections();
        }
        private void setupSockets()
        {
            Console.WriteLine("Setup Sockets Starting");
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            listenerEndpoint = new IPEndPoint(ipAddress, listenerPortNumber);

            serverListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            serverListener.Bind(listenerEndpoint);
            Console.WriteLine("NET ADDRESS: " + listenerEndpoint.Address.ToString());
            Application.InvokeOnMain(new Action(() => UIManager.GenerateQRCode(listenerEndpoint.Address.ToString() + ":" + listenerEndpoint.Port.ToString())));
        }
        private void listenForConnections()
        {
            Console.WriteLine("Listening for Connections...");

            while (true)
            {
                serverListener.Listen(0);
                Socket connection = serverListener.Accept();
                ClientInterface client = new ClientConnection(connection);
                if (gameLobby.getRoomSize() >= gameLobby.MaxRoomSize)
                {
                    client.sendTooManyPlayers();
                }
                else
                {
                    string name = client.getName();
                    gameLobby.addPlayer(new Player(name, 0, client));
                }
            }
        }
    }
}