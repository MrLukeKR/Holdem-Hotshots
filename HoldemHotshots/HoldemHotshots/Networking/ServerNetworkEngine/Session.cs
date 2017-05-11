using HoldemHotshots.GameLogic;
using System.Collections.Generic;
using System.Threading;

namespace HoldemHotshots.Networking.ServerNetworkEngine
{
    /// <summary>
    /// Responsible for managing the server connection
    /// </summary>
    class Session
    {
        private static Session networkEngine;
        private static List<ListenerThread> listenerThreads = new List<ListenerThread>();
        public  static Room Lobby;

        private Session() { }

        /// <summary>
        /// Returns the current instance
        /// </summary>
        /// <returns>Current instance of Session</returns>
        public static Session Getinstance()
        {
            if(networkEngine == null)
                networkEngine = new Session();
           
            return networkEngine;
        }

        /// <summary>
        /// Initialises the server, room and connection listener thread
        /// </summary>
        public void Init()
        {
            Lobby = new Room();
            ListenerThread listener = new ListenerThread();
            
            listenerThreads.Add(listener);

            Thread listenThread = new Thread(listener.Start);
            listenThread.Start();
        }

        /// <summary>
        /// Disconnects all connections
        /// </summary>
        public static void DisposeOfSockets()
        {
            foreach(ListenerThread lThread in listenerThreads) lThread.ShutdownSocket();
            listenerThreads.Clear();
        }

        /// <summary>
        /// Returns the server lobby
        /// </summary>
        /// <returns>Server lobby</returns>
        internal Room getRoom()
        {
            return Lobby;
        }
    }
}