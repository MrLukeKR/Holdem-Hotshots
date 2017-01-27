using System.Net;
using System.Net.Sockets;
using System.Text;
using TexasHoldemPoker;

namespace TexasHoldemPoker.Game.NetworkEngine.AndroidNetworkEngine{
  class ServerNetworkEngine : NetworkEngineInterface{
    private Socket serverListener;
    private Socket broadcaster;
    private Room gameLobby;
    private int listenerPortNumber = 8741;
    private int broadcastPortNumber = 8742;
    private IPEndPoint listenerEndpoint;

    public void init(){

      gameLobby = new Room();
      //TODO : Create udp broadcast function and add it here
      //TODO : implement multithreading
      listenForConnections();
    }


    private void listenForConnections() {
      serverListener = new Socket(AddressFamily.InterNetwork ,SocketType.Stream,ProtocolType.Tcp);
      listenerEndpoint = new IPEndPoint(0, listenerPortNumber);
      serverListener.Bind(listenerEndpoint);
      while (true){
        serverListener.Listen(0);
        Socket connection = serverListener.Accept();
        
        if(gameLobby.getRoomSize() >= gameLobby.getMaxRoomSize()){

            
            
        }
         //TODO : Call function to add PlayerConnection to lobby

         //Get player info

         string message = "GET_PLAYER_NAME";
         byte[] messageBuffer = Encoding.ASCII.GetBytes(message);
         connection.Receive; 

         gameLobby.addPlayer(new Player();  

      }
    }
    private void broadcastGameInfo(){
      this.broadcaster = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
      this.broadcaster.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
      IPEndPoint broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, broadcastPortNumber);
      while (true){
        sendBroadcast((listenerEndpoint.ToString() +  gameLobby.getRoomSize().ToString()));
        //TODO add delay between broadcasts
      }
    }
    private void sendBroadcast(string message){
      byte[] messageBuffer = Encoding.ASCII.GetBytes(message);
      broadcaster.Send(messageBuffer);
    }
  }
}
