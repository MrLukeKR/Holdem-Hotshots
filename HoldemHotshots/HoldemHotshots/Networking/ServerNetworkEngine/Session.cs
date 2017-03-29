using HoldemHotshots.GameLogic;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HoldemHotshots.Networking.ServerNetworkEngine
{
    class Session
    {
        private static Session networkEngine;
        private static List<ListenerThread> listenerThreads = new List<ListenerThread>();
        public static Room Lobby;

        private Session()
        {
            //Leave blank for singleton design pattern
        }

        public static Session getinstance()
        {
            if(networkEngine == null)
                networkEngine = new Session();
           
            return networkEngine;
        }

        public void init()
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