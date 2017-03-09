using System;
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
            while (true)
            {
                try
                {
                    //ServerCommandManager.getInstance(connection, player).ping(); //TODO: Jack, can you make it so that the player and ServerConnection/ClientConnection can be accessed from this class
                }
                catch
                {
                    //TODO :Disconnect handling here

                    Console.WriteLine("Connection dropped");
                }
                finally
                {
                    Thread.Sleep(5000);
                }
            }
            
        }

    }
}
