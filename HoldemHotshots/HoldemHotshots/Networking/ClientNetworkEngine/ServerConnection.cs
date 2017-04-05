using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using HoldemHotshots.Utilities;

namespace HoldemHotshots.Networking.ClientNetworkEngine
{
    /// <summary>
    /// Wrapper class for connection to server
    /// </summary>
    class ServerConnection : ServerInterface
    {
        private Socket connection;
        private ServerConnectionMonitorThread monitorThread;
        private Encryptor encryptionCipher;
        readonly Boolean isencrypted = false;

        /// <summary>
        /// Constructor for ServerConnection
        /// </summary>
        /// <param name="newConnection">Socket to server</param>
        public ServerConnection(Socket newConnection,Encryptor encryptionCipher)
        {
            connection = newConnection;
            this.encryptionCipher = encryptionCipher;
            monitorThread = new ServerConnectionMonitorThread(connection);
            monitorThread.Start();
        }

        /// <summary>
        /// Sends a generic command to the Server
        /// </summary>
        /// <param name="command">The command sent to the server</param>
        public void SendMessage(string command)
        {
            bool sent = false;

            while (!sent)
                try
                {
                    #if (isencrypted)
                    command = encryptionCipher.EncyptString(command);
                    #endif

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

                #if (isencrypted)
                response = encryptionCipher.DecryptString(response);
                #endif
            }

            return response;
        }

        /// <summary>
        /// Sends a fold command to the server
        /// </summary>
        public void SendFold()
        {
            SendMessage("FOLD");
        }

        /// <summary>
        /// Sends a raise command to the server
        /// </summary>
        /// <param name="amount">The amount to raise</param>
        public void SendRaise(uint amount)
        {
            SendMessage("RAISE:" + amount);
        }

        /// <summary>
        /// Sends a check command to the server
        /// </summary>
        public void SendCheck()
        {
            SendMessage("CHECK");
        }

        /// <summary>
        /// Sends a all in command to the server
        /// </summary>
        public void SendAllIn()
        {
            SendMessage("ALL_IN");
        }

        /// <summary>
        /// Sends a call command to the server
        /// </summary>
        public void SendCall()
        {
            SendMessage("CALL");
        }

        /// <summary>
        /// Sends a bid command to the server
        /// </summary>
        /// <param name="bid">The bid amount</param>
        public void SendPlayerBid(uint bid)
        {
            SendMessage("PLAYER_BID:" + bid);
        }
    }
}