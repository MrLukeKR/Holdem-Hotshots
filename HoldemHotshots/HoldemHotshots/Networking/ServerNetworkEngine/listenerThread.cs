using HoldemHotshots.GameLogic;
using HoldemHotshots.GameLogic.Player;
using HoldemHotshots.Managers;
using HoldemHotshots.Utilities;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security;
using System.Threading;

namespace HoldemHotshots.Networking.ServerNetworkEngine
{
    /// <summary>
    /// Responsible for accepting connections from clients to the server
    /// </summary>
    class ListenerThread
    {
        private Socket serverListener;
        private int listenerPortNumber = 56789;
        private IPEndPoint listenerEndpoint;
        private bool shutdown;
        private Encryptor encryptionCipher; // Encyptor to be passed to clientConnections

        public ListenerThread(){ }

        /// <summary>
        /// Begins the listening process
        /// </summary>
        [SecurityCritical]
        public void Start()
        {
            shutdown = false;
            SetupSockets();
            ListenForConnections();
        }

        /// <summary>
        /// Sets up the 2-way socket connections after they have been initialised from the listener thread
        /// </summary>
        [SecurityCritical]
        private void SetupSockets()
        {
            IPAddress ipAddress = null;

            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                ipAddress = ipHostInfo.AddressList[0];
            }
            catch
            {
                //The following code was written by Mike Bluestein in the Xamarin forums
                //It's used to manually get an IP Address where the DNS Host cannot be resolved
                foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
                    if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                        foreach (var addressInfo in netInterface.GetIPProperties().UnicastAddresses)
                            if (addressInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                                ipAddress = addressInfo.Address;
                //End of 3rd party code
            }
            
            listenerEndpoint = new IPEndPoint(ipAddress, listenerPortNumber);
            serverListener   = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
           
            if(!serverListener.IsBound)
                serverListener.Bind(listenerEndpoint);

            encryptionCipher = new Encryptor(); //Create encyptor with random key and initalization vector
            
            string key = Convert.ToBase64String(encryptionCipher.getKey());
            string iv  = Convert.ToBase64String(encryptionCipher.getIV());

            UIUtils.GenerateQRCode(listenerEndpoint.Address.ToString() + ":" + listenerEndpoint.Port.ToString() +":" + key + ":" + iv, true);
        }

        /// <summary>
        /// Listens for client connections until a shutdown command is sent
        /// </summary>
        private void ListenForConnections()
        {
            try
            {
                while (!shutdown)
                {
                    serverListener.Listen(0);
                    Socket connection = serverListener.Accept();
                    ClientConnection client = new ClientConnection(connection,encryptionCipher);
                    if (Session.Lobby.players.Count >= Room.MAX_ROOM_SIZE)
                    {
                        client.SendTooManyPlayers();
                    }
                    else
                    {
                        ServerPlayer newPlayer = new ServerPlayer(client);
                        ServerCommandListenerThread lt = new ServerCommandListenerThread(client, newPlayer);

                        new Thread(lt.Start).Start();

                        while (newPlayer.name == null && connection.Connected) { client.GetName(); Thread.Sleep(1000); }

                        int similarCount = 1;

                        newPlayer.originalName = newPlayer.name;

                        foreach(ServerPlayer player in Session.Lobby.players)
                            if (player.originalName == newPlayer.originalName)
                                player.name = player.originalName + " " + similarCount++;

                        if (similarCount > 1)
                            newPlayer.name = newPlayer.originalName + " " + similarCount;

                        Session.Lobby.players.Add(newPlayer);
                        SpeechManager.Speak(newPlayer.name + " has joined the room");
                        UIUtils.ValidateStartGame();
                        UIUtils.UpdatePlayerList(Session.Lobby);
                    }
                }
            }
            catch (SocketException)
            {

            }
        }

        /// <summary>
        /// Shuts down the sockets
        /// </summary>
        internal void ShutdownSocket()
        {
            shutdown = true;
            
            serverListener.Close();
        }
    }
}