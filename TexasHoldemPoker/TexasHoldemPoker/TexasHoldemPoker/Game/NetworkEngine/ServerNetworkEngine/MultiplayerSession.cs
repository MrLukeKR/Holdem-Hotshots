using System.Net;
using System.Net.Sockets;
using System.Text;
using TexasHoldemPoker;
using TexasHoldemPoker.Game.NetworkEngine.ServerNetworkEngine;

namespace TexasHoldemPoker.Game.NetworkEngine.AndroidNetworkEngine{
  class MultiplayerSession : NetworkEngineInterface{

    private static MultiplayerSession networkEngine;

    private MultiplayerSession()
        {
            //Leave blank for singleton design pattern
        }
    
    public static ServerNetworkEngine getinstance()
        {
            return networkEngine;
        }

    public void init(){

          gameLobby = new Room();

          listenerThread listener = new listenerThread(gameLobby);
          listener.Start();
      
    }

   
  }
}
