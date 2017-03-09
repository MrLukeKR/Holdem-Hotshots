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
        private CommandManager cm;

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
            //TODO: Get this to send the CommandManager ping()

            while (true)
            {
                try
                {
                    //CommandManager.getInstance(connection, player).ping(); //TODO: Jack, can you make it so that the player and ServerConnection/ClientConnection can be accessed from this class
                }
                catch
                {
                    Console.WriteLine("Connection dropped");
                    handleDisconnect();
                }
                finally
                {
                    Thread.Sleep(5000);
                }
            }

        }

        private void handleDisconnect()
        {
              
        }
    }
}
