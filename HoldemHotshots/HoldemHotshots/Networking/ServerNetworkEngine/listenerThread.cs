using System;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using Urho;

namespace HoldemHotshots
{
    class ListenerThread
    {
        private Socket serverListener;
        private int listenerPortNumber = 56789; //Using 0 allows C# to assign a free port itself
        private IPEndPoint listenerEndpoint;
        private bool shutdown;

        public ListenerThread()
        {

        }
        public void Start()
        {
            Console.WriteLine("Listener Starting");

            shutdown = false;
            setupSockets();
            listenForConnections();
        }
        private void setupSockets()
        {
            Console.WriteLine("Setup Sockets Starting");
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            listenerEndpoint = new IPEndPoint(ipAddress, listenerPortNumber);
            
            serverListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
           
            if(!serverListener.IsBound) serverListener.Bind(listenerEndpoint);

            Console.WriteLine("NET ADDRESS: " + listenerEndpoint.Address.ToString());

            Application.InvokeOnMain(new Action(() => UIManager.GenerateQRCode(listenerEndpoint.Address.ToString() + ":" + listenerEndpoint.Port.ToString())));
        }

        private void listenForConnections()
        {
            Console.WriteLine("Listening for Connections...");

            try //TODO: See if we can get rid of TRY/CATCH (Currently used to handle exceptions when closing the listener when still listening - See if we can force-stop listening?)
            {
                while (!shutdown)
                {
                    serverListener.Listen(0);
                    Console.WriteLine("Listening for connections");
                    Socket connection = serverListener.Accept();
                    Console.WriteLine("Connection accepted");
                    ClientInterface client = new ClientConnection(connection);
                    if (Session.Lobby.getRoomSize() >= Session.Lobby.MaxRoomSize)
                    {
                        client.sendTooManyPlayers();
                    }
                    else
                    {
                        Console.WriteLine("Getting name...");
                        string name = client.getName();
                        Session.Lobby.addPlayer(new Player(name, 0, client));
                        Console.WriteLine("RECEIVED NAME: " + name);
                        UIUtils.UpdatePlayerList(Session.Lobby);
                    }
                }
            }
            catch (SocketException)
            {

            }
        }

        internal void ShutdownSocket()
        {
            shutdown = true;
            
            if (serverListener.Connected)
            {
                serverListener.Shutdown(SocketShutdown.Both);
                Console.WriteLine("Shutdown Socket");

                serverListener.Disconnect(false);
                Console.WriteLine("Disconnected Socket");
            }

            serverListener.Close();

            Console.WriteLine("Closed Socket");
        }
    }
}