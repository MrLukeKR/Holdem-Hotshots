﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HoldemHotshots
{
    class ServerConnectionMonitorThread
    {
        private Socket connectionSocket;

        public ServerConnectionMonitorThread(Socket connectionSocket)
        {
            this.connectionSocket = connectionSocket;
        }

        public void start()
        {
            var monitor = new Thread(monitorConnection);

            monitor.Start();

        }

        private void monitorConnection()
        {
            int timeLeft = 10;
            bool timedOut = false;
            byte[] ping = new byte[0];

            while (!timedOut)
            {
                if (timeLeft != 10 && connectionSocket.Connected) timeLeft = 10;

                Thread.Sleep(5000);

                while (!connectionSocket.Connected && timeLeft > 0) {
                    Console.WriteLine("Timing out in: " + timeLeft--);
                    Thread.Sleep(1000);
                }

                if (timeLeft == 0) timedOut = true;
            }

            Console.WriteLine("Timed out!");
            handleDisconnect();
        }
        private void handleDisconnect()
        {
            connectionSocket.Shutdown(SocketShutdown.Both);
        }
    }
}
