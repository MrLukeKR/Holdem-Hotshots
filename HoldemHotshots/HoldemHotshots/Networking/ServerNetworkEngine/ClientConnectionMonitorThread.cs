using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HoldemHotshots.Networking.ServerNetworkEngine
{
    /// <summary>
    /// Monitors the connection between the client and server
    /// </summary>
    class ClientConnectionMonitorThread
    {
        private  bool receivedCommandRecently = true;
        private  int  lastCommandCountdown    = 5;
        private  int  timeoutCountdown        = 5;

        private readonly Thread timeoutTimer;
        private readonly Socket connectionSocket;
        private readonly byte[] messageBuffer = Encoding.ASCII.GetBytes("PING");
        private readonly byte[] prefix = new byte[4];

        public ClientConnectionMonitorThread(Socket connectionSocket)
        {
            this.connectionSocket = connectionSocket;
            timeoutTimer = new Thread(StartTimeout);
        }

        /// <summary>
        /// Starts the monitor thread
        /// </summary>
        public void Start()
        {
            new Thread(MonitorConnection).Start();
            timeoutTimer.Start();
        }
        
        /// <summary>
        /// Resets all countdown timers that signal a disconnection
        /// </summary>
        public void ResetCommandTimer()
        {
            receivedCommandRecently = true;
            lastCommandCountdown = 5;
            timeoutCountdown = 5;
        }

        /// <summary>
        /// Sends a ping command to the client
        /// </summary>
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

        /// <summary>
        /// Starts the timeout countdown
        /// </summary>
        private void StartTimeout()
        {
            while (true)
            {
                while (lastCommandCountdown-- > 0)
                    Thread.Sleep(1000);

                receivedCommandRecently = false;
            }
        }
        
        /// <summary>
        /// Detects when a disconnection has occurred
        /// </summary>
        private void MonitorConnection()
        {
            EndPoint reconnectEP = connectionSocket.RemoteEndPoint;

            while (timeoutCountdown > 0)
            {
              
                /*  
                if (!receivedCommandRecently) {
                    Ping();
                    timeoutCountdown--;
                }
                */

                Thread.Sleep(1000);

                if(timeoutCountdown < 5 && timeoutCountdown > 0) Console.WriteLine("Timing out in " + timeoutCountdown);
            }

            Console.WriteLine("Timed out!");
            HandleDisconnect();
        }

        /// <summary>
        /// Attempts to reconnect the disconnected client
        /// </summary>
        /// <param name="connectionPoint">Connection endpoint to reconnect</param>
        /// <returns>Connection status</returns>
        private bool AttemptReconnect(EndPoint connectionPoint)
        {
            if(ListenForReconnect() == true)
            {
                Console.WriteLine("Reconnect successful");
            }
            else
            {
                Console.WriteLine("Failed to reconnect");
                connectionSocket.Close();
            }
            
            return connectionSocket.Connected;
        }

        /// <summary>
        /// Disconnects the client(s) from the server
        /// </summary>
        private void HandleDisconnect()
        {
            if(connectionSocket.Connected) connectionSocket.Disconnect(true);
            Session.Lobby.CheckConnections();
        }

        /// <summary>
        /// Waits for the client to reconnect
        /// </summary>
        /// <returns>Connection status</returns>
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