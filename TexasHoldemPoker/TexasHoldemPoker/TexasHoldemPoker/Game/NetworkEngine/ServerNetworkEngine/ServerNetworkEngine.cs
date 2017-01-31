using System.Net;
using System.Net.Sockets;
using System.Text;
using TexasHoldemPoker;
using TexasHoldemPoker.Game.NetworkEngine.ServerNetworkEngine;

namespace TexasHoldemPoker.Game.NetworkEngine.AndroidNetworkEngine{
  class ServerNetworkEngine : NetworkEngineInterface{

    private static ServerNetworkEngine networkEngine;

    private ServerNetworkEngine()
        {
            //Leave blank for singleton design pattern
        }
    
    public static ServerNetworkEngine getinstance()
        {
            return networkEngine;
        }

    public void init(){

          gameLobby = new Room();

          broadcastThread broadcaster = new broadcastThread(gameLobby);
          listenerThread listener = new listenerThread(gameLobby);

          broadcaster.Start();
          listener.Start();

          Thread.join(broadcaster);
          Thread.join(listener);       
    }

   
  }
}
