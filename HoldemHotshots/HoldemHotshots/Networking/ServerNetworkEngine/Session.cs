using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HoldemHotshots
{
    class Session
    {
        private static Session networkEngine;
        private Session()
        {
            //Leave blank for singleton design pattern
        }

        public static Session getinstance()
        {
            Console.WriteLine("Getting Instance...");
            if(networkEngine == null)
            {
                Console.WriteLine("Creating Instance...");
                networkEngine = new Session();
            }
            Console.WriteLine("Created instance!");
            return networkEngine;
        }
        public void init()
        {
            Console.WriteLine("Creating New Room...");
            Room gameLobby = new Room();
            Console.WriteLine("Created Room!");
            Console.WriteLine("Creating Listener thread...");
            ListenerThread listener = new ListenerThread(gameLobby);
            Console.WriteLine("Created Listener thread...");
            Console.WriteLine("Starting new Thread...");
            Thread listenThread = new Thread(new ThreadStart(listener.Start));
            listenThread.Start();
            Console.WriteLine("Thread Started...");
        }

    }
}