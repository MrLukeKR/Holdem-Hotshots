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
            //TODO: Get this to send the CommandManager ping()
            
            Byte[] ping = Encoding.ASCII.GetBytes("PING");
            byte[] prefix = new byte[4];
            prefix = BitConverter.GetBytes(ping.Length);

            while (true)
            {
                try     { connectionSocket.Send(prefix); connectionSocket.Send(ping); }
                catch   { }
                
                if (connectionSocket.Connected)
                {
                    System.Threading.Thread.Sleep(5000);
                }
                else
                {
                    //TODO :Disconnect handling here

                    Console.WriteLine("Connection dropped");

                }
            }

        }
    }
}
