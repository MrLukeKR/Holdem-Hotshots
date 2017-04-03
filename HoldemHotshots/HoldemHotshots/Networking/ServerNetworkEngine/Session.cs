using HoldemHotshots.GameLogic;
using System.Collections.Generic;
using System.Threading;

namespace HoldemHotshots.Networking.ServerNetworkEngine
{
    class Session
    {
        private static Session networkEngine;
        private static List<ListenerThread> listenerThreads = new List<ListenerThread>();
        public  static Room Lobby;

        private Session() { }

        public static Session Getinstance()
        {
            if(networkEngine == null)
                networkEngine = new Session();
           
            return networkEngine;
        }

        public void Init()
        {
            Lobby = new Room();
            ListenerThread listener = new ListenerThread();
            
            listenerThreads.Add(listener);

            Thread listenThread = new Thread(listener.Start);
            listenThread.Start();
        }

        public static void DisposeOfSockets()
        {
            foreach(ListenerThread lThread in listenerThreads) lThread.ShutdownSocket();
            listenerThreads.Clear();
        }

        internal Room getRoom()
        {
            return Lobby;
        }
    }
}