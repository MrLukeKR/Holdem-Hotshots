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
        private  int  lastCommandCountdown    = 5;
        private  int  timeoutCountdown        = 5;

        private readonly Thread timeoutTimer;
        private readonly Socket connectionSocket;
        
        public ClientConnectionMonitorThread(Socket connectionSocket)
        {
            this.connectionSocket = connectionSocket;
            timeoutTimer = new Thread(StartTimeout);
        }

        public void Start()
        {
            new Thread(MonitorConnection).Start();
            timeoutTimer.Start();
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
                /*
                if (!receivedCommandRecently) {
                    //TODO: CALL PING HERE!
                    timeoutCountdown--;
                }
                */

                Thread.Sleep(1000);

                if(timeoutCountdown < 5 && timeoutCountdown > 0) Console.WriteLine("Timing out in " + timeoutCountdown);
            }

            Console.WriteLine("Timed out!");
            HandleDisconnect();
        }

        private bool AttemptReconnect(EndPoint connectionPoint)
        {
            if(ListenForReconnect() == true)
            {
                Console.WriteLine("Reconnect successful");
            }
            else
            {
                Console.WriteLine("Failed to reconnect");
                connectionSocket.Disconnect(true);
            }

           
            //TODO: Reconnection code

            return connectionSocket.Connected;
        }

        private void HandleDisconnect()
        {
            if(connectionSocket.Connected) connectionSocket.Disconnect(false);
            Session.Lobby.CheckConnections();
        }

        private bool ListenForReconnect()
        {

            //Returns true if client recconnects

            int countdown = 5;

            while(countdown != 0)
            {
                if (this.connectionSocket.Connected)
                    return true;

                countdown--;
                Thread.Sleep(1000);

            }

            return false;


        }
    }
}