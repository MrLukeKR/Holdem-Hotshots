using System.Net;
using System.Net.Sockets;
using System.Text;

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
            return networkEngine;
        }
        public void init()
        {
            Room gameLobby = new Room();
            ListenerThread listener = new ListenerThread(gameLobby);
            listener.Start();

        }

    }
}