using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TexasHoldemPoker.Game.NetworkEngine.ServerNetworkEngine
{
    class broadcastThread : Thread
    {

        private Socket broadcaster;
        private int broadcastPortNumber = 8742;
        private Room gameLobby;

        public broadcastThread(Room gameLobby)
        {
            this.gameLobby = gameLobby;
        }

        public void Start()
        {
            broadcastGameInfo();
        }

        private void broadcastGameInfo()
        {
            this.broadcaster = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.broadcaster.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            IPEndPoint broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, broadcastPortNumber);
            while (true)
            {
                sendBroadcast((listenerEndpoint.ToString() + gameLobby.getRoomSize().ToString()));
                //TODO add delay between broadcasts
            }
        }

        private void sendBroadcast(string message)
        {
            byte[] messageBuffer = Encoding.ASCII.GetBytes(message);
            broadcaster.Send(messageBuffer);
        }


    }
}
