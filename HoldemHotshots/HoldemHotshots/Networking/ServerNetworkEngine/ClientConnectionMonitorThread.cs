using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HoldemHotshots.Networking.ServerNetworkEngine
{
    class ClientConnectionMonitorThread
    {
        private  bool receivedCommandRecently = true;
        private  int  lastCommandCountdown = 5;
        private  int  timeoutCountdown = 5;

        private readonly Thread timeoutTimer;
        private readonly Socket connectionSocket;
        private readonly byte[] messageBuffer = Encoding.ASCII.GetBytes("PING");
        private readonly byte[] prefix = new byte[4];
        
        public ClientConnectionMonitorThread(Socket connectionSocket)
        {
            this.connectionSocket = connectionSocket;
            timeoutTimer = new Thread(StartTimeout);
            prefix = BitConverter.GetBytes(messageBuffer.Length);
        }

        public void Start()
        {
            new Thread(MonitorConnection).Start();
            timeoutTimer.Start();
        }

        private void Ping()
        {
            try
            {
                connectionSocket.Send(prefix);          //send prefix
                connectionSocket.Send(messageBuffer);   //send actual message
            }
            catch
            {

            }
        }

        public void ResetCommandTimer()
        {
            receivedCommandRecently = true;
            lastCommandCountdown = 5;
            timeoutCountdown = 5;
        }

        private void StartTimeout()
        {
            while (true)
            {
                while (lastCommandCountdown-- > 0)
                    Thread.Sleep(1000);

                receivedCommandRecently = false;
            }
        }
        
        private void MonitorConnection()
        {
            EndPoint reconnectEP = connectionSocket.RemoteEndPoint;

            while (timeoutCountdown > 0)
            {
                if (!receivedCommandRecently) {
                    Ping();
                    timeoutCountdown--;
                }

                Thread.Sleep(1000);

                if(timeoutCountdown < 5 && timeoutCountdown > 0) Console.WriteLine("Timing out in " + timeoutCountdown);
            }

            Console.WriteLine("Timed out!");
            HandleDisconnect();
        }

        private bool AttemptReconnect(EndPoint connectionPoint)
        {
            Console.WriteLine("Attemping to reconnect...");
            try
            {
                //TODO: Reconnect code (Listener?)
            }
            catch
            {
                Console.WriteLine("Failed to reconnect");
            }
            //TODO: Reconnection code

            return connectionSocket.Connected;
        }

        private void HandleDisconnect()
        {
            //TODO: Handle disconnect
            if(connectionSocket.Connected) connectionSocket.Disconnect(false);
            Session.Lobby.CheckConnections();
        }
    }
}