using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using HoldemHotshots.Utilities;

namespace HoldemHotshots.Networking.ServerNetworkEngine
{
    /*
     * This class is a wrapper for the client socket and contains commands that can be sent to the client
     */

    class ClientConnection : ClientInterface
    {
        public Socket connection { get; private set; }
        public ClientConnectionMonitorThread monitorThread { get; private set; }
        public Encryptor encryptionCipher;
        readonly Boolean isencrypted = false;

        public ClientConnection(Socket connection,Encryptor encryptionCipher)
        {
            this.connection = connection;
            this.encryptionCipher = encryptionCipher;
            monitorThread = new ClientConnectionMonitorThread(connection);
            monitorThread.Start();

        }

        public void SendCommand(string command)
        {
            bool sent = false;
            int timeout = 5;

            while (!sent && timeout-- > 0)
            {
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
                }
                catch
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public string GetCommand()
        {
            string response = "";
            try
            {
                byte[] prefix = new byte[4];

                //read prefix
                connection.Receive(prefix, 0, 4, 0);
                int messagelength = BitConverter.ToInt32(prefix, 0);

                if (messagelength > 0)
                {
                    //read actual message
                    byte[] Buffer = new byte[messagelength];
                    connection.Receive(Buffer, 0, messagelength, 0);
                    response = Encoding.Default.GetString(Buffer);

                    #if (isencrypted)
                    response = encryptionCipher.DecryptString(response);
                    #endif
                }
            }
            catch
            {

            }
            
            return response;
        }

        //Commands
        public void GetName()
        {
            SendCommand("GET_PLAYER_NAME");
        }

        public void SendTooManyPlayers()
        {
            SendCommand("MAX_PLAYERS_ERROR");
        }

        public void SendPlayerKicked()
        {
            SendCommand("PLAYER_KICKED");
        }

        public void AnimateCard(int cardValue)
        {
            SendCommand("ANIMATE_CARD:" + cardValue);
        }

        public void GiveCard(int suit, int rank)
        {
            SendCommand("GIVE_CARD:" + suit + ":" + rank);
        }

        public void TakeTurn()
        {
            SendCommand("TAKE_TURN");
        }
        
        public void SetChips(uint amount)
        {
            SendCommand("SET_CHIPS:" + amount);
        }

        public void SendKicked()
        {
            SendCommand("PLAYER_KICKED");
        }

        public void SendCurrentState(string state)
        {
            SendCommand("CURRENT_STATE:" + state);
        }

        public void SetPlayerBid(uint bid)
        {
            SendCommand("PLAYER_BID:" + bid);
        }

        public void SetHighestBid(uint bid)
        {
            SendCommand("HIGHEST_BID:" + bid);
        }

        public void StartGame()
        {
            SendCommand("START_GAME");
        }
        
        public void DisplayMessage(string message)
        {
            SendCommand("DISPLAY_MESSAGE:" + message);
        }

        public void ResetInterface()
        {
            SendCommand("RESET_INTERFACE");
        }

        public bool IsConnected()
        {
            return connection.Connected;
        }
    }
}