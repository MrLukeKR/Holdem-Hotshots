using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace HoldemHotshots.Networking.ClientNetworkEngine
{
    class ServerConnection : ServerInterface
    {

        private Socket connection;
        private ServerConnectionMonitorThread monitorThread;

        public ServerConnection(Socket connection)
        {
            this.connection = connection;
            this.monitorThread = new ServerConnectionMonitorThread(connection);
            monitorThread.Start();
        }

        public void SendMessage(String command)
        {
            bool sent = false;
            while (!sent)
                try
                {
                    byte[] messageBuffer = Encoding.ASCII.GetBytes(command);
                    byte[] prefix = new byte[4];

                    //send prefix
                    prefix = BitConverter.GetBytes(messageBuffer.Length);
                    connection.Send(prefix);

                    //send actual message
                    connection.Send(messageBuffer);
                    sent = true;
                    Console.WriteLine("Sending message '" + command + "' succeeded!");
                }
                catch
                {   
                    Console.WriteLine("Sending message '" + command + "' failed!");
                    Thread.Sleep(1000);
                    //TODO: Resend any information if the connection is re-established
                }
        }

        internal string GetCommand()
        {
            string response = "";
            byte[] prefix = new byte[4];

            connection.Receive(prefix, 0, 4, 0);
            int messagelength = BitConverter.ToInt32(prefix, 0);

            if (messagelength > 0)
            {
                byte[] buffer = new byte[messagelength];
                connection.Receive(buffer, 0, messagelength, 0);
                response = Encoding.Default.GetString(buffer);
            }

            return response;
        }

        public void SendFold()
        {
            SendMessage("FOLD");
        }

        public void SendRaise(uint amount)
        {
            SendMessage("RAISE:" + amount);
        }

        public void SendCheck()
        {
            SendMessage("CHECK");
        }

        public void SendAllIn()
        {
            SendMessage("ALL_IN");
        }

        public void SendCall()
        {
            SendMessage("CALL");
        }

        public void SendPlayerBid(uint bid)
        {
            SendMessage("PLAYER_BID:" + bid);
        }
    }
}
