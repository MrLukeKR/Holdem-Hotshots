using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace HoldemHotshots.Networking.ClientNetworkEngine
{
    class ServerConnectionMonitorThread
    {
        private readonly Socket connectionSocket;

        public ServerConnectionMonitorThread(Socket connectionSocket)
        {
            this.connectionSocket = connectionSocket;
        }

        public void Start()
        {
            new Thread(MonitorConnection).Start();
        }

        private void MonitorConnection()
        {
            int  timeLeft = 10;
            bool timedOut = false;

            while (!timedOut)
            {
                if (timeLeft != 10 && connectionSocket.Connected)
                    timeLeft  = 10;

                Thread.Sleep(5000);

                while (!connectionSocket.Connected && timeLeft > 0)
                {
                    Console.WriteLine("Timing out in: " + timeLeft--);
                    Thread.Sleep(1000);
                }

                if (timeLeft == 0)
                    timedOut = true;
            }

            Console.WriteLine("Timed out!");
            HandleDisconnect();
        }

        private void HandleDisconnect()
        {
            EndPoint serverEndpoint = connectionSocket.RemoteEndPoint;

            int timeoutCountdown = 5;

            Console.WriteLine("Trying to reconnect to server ...");

            while(timeoutCountdown-- > 0)
            {
                if (!connectionSocket.Connected)
                    connectionSocket.Connect(serverEndpoint);   
                
                Thread.Sleep(1000);
            }

            if (connectionSocket.Connected)
            {
                connectionSocket.Disconnect(false);
            }
            else
            {
                connectionSocket.Disconnect(true);
            }

        }
    }
}