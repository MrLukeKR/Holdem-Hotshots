using HoldemHotshots.GameLogic.Player;
using System.Net;
using System.Net.Sockets;
using HoldemHotshots.Utilities;
using System.Text;
using System;

namespace HoldemHotshots.Networking.ClientNetworkEngine
{
    /// <summary>
    /// Version of the multiplayer session that exists on the client
    /// </summary>

    class ClientSession
    {
        private readonly Socket connectionSocket;
        private readonly IPEndPoint endpoint;
        public readonly ClientPlayer player;

        /// <summary>
        /// Constructor for Client Session
        /// </summary>
        /// <param name="address">IP address of host device</param>
        /// <param name="portNumber">Port number on host device</param>
        /// <param name="player">Client-side version of the player game object </param>

        public ClientSession(string address, int portNumber, ClientPlayer player,string key,string iv)
        {
            this.player = player;
            connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            endpoint = new IPEndPoint(IPAddress.Parse(address), portNumber);

            byte[] base64Key = Convert.FromBase64String(key);
            byte[] base64IV  = Convert.FromBase64String(iv);

            Encryptor encryptionCipher = new Encryptor(base64Key, base64IV);

            player.connection = new ServerConnection(connectionSocket,encryptionCipher);
        }

        /// <summary>
        /// Initalizes the Client Session
        /// </summary>
        public void Init()
        {
            new CommandListenerThread((ServerConnection)player.connection, player).Start();
        }

        /// <summary>
        /// Returns True if the Client Session is connected to it's Server session
        /// </summary>
        /// <returns>True if is connected</returns>
        public bool Connect()
        {
            if (!connectionSocket.Connected)
                connectionSocket.Connect(endpoint);
            else
                return false;

            return true;
        }

        public void Disconnect()
        {
            connectionSocket.Disconnect(true);
        }
    }
}
