using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HoldemHotshots
{
    class ClientConnectionMonitorThread
    {
        private Socket connectionSocket;

        public ClientConnectionMonitorThread(Socket connectionSocket)
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
            while (true)
            {
                if (connectionSocket.Connected)
                {
                    System.Threading.Thread.Sleep(5000);
                }
                else
                {
                    //TODO :Disconnect handling here

                    Console.WriteLine("Connection dropped");
                    handleDisconnect();

                }
            }

        }

        private void handleDisconnect()
        {
              
        }
    }
}
