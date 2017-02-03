using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace HoldemHotshots
{
    class ListenerThread : Thread
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
            setupSockets();
            listenForConnections();
        }
        private void setupSockets()
        {
            serverListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenerEndpoint = new IPEndPoint(0, listenerPortNumber);
            serverListener.Bind(listenerEndpoint);
            UIManager.GenerateQRCode(listenerEndpoint.Address.ToString());
        }
        private void listenForConnections()
        {

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