using System.Net;
using System.Net.Sockets;
using System.Text;
using TexasHoldemPoker;
using TexasHoldemPoker.Game.NetworkEngine.ServerNetworkEngine;
namespace TexasHoldemPoker.Game.NetworkEngine.AndroidNetworkEngine
{
    class Session : SessionInterface
    {
        private static Session networkEngine;
        private Session()
        {
            //Leave blank for singleton design pattern
        }

        public static Session getinstance()
        {
            if(this.networkEngine == null)
            {
                this.networkEngine = new Session();
            }

            return networkEngine;
        }
        public void init()
        {
            gameLobby = new Room();
            ListenerThread listener = new ListenerThread(gameLobby);
            listener.Start();

        }

    }
}