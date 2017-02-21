using System;
using System.Text;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace HoldemHotshots
{
    /*
     * This class is a wrapper for the client socket and contains commands that can be sent to the client
     */

    class ClientConnection : ClientInterface
    {
        private Socket connection;
        private ClientConnectionMonitorThread monitorThread;

        public ClientConnection(Socket connection)
        {
            this.connection = connection;
            this.monitorThread = new ClientConnectionMonitorThread(connection);
            monitorThread.start();

        }

        public void sendCommand(String command)
        {
          
            byte[] messageBuffer = Encoding.ASCII.GetBytes(command);
            byte[] prefix = new byte[4];

            //send prefix
            prefix = BitConverter.GetBytes(messageBuffer.Length);
            connection.Send(prefix);

            //send actual message
            connection.Send(messageBuffer);

            Console.WriteLine("command: '" + command + "' sent");
        }

        public String getResponse()
        {

            byte[] prefix = new byte[4];

            //read prefix
            connection.Receive(prefix, 0, 4, 0);
            int messagelength = int.Parse(prefix.ToString());

            //read actual message
            Byte[] Buffer = new byte[messagelength];
            connection.Receive(Buffer, 0, messagelength, 0);
            String response = Encoding.Default.GetString(Buffer);
        
            Console.WriteLine("Response: '" + response + "' recieved");
            return response;
        }

        //Commands
        public String getName()
        {
            sendCommand("GET_PLAYER_NAME");
            return getResponse();
        }

        public void sendTooManyPlayers()
        {
            sendCommand("MAX_PLAYERS_ERROR");
        }

        public void sendPlayerKicked()
        {
            sendCommand("PLAYER_KICKED");
        }

        public void animateCard(int cardValue)
        {
            sendCommand("ANIMATE_CARD:" + cardValue);
        }

        public void giveCard(int suit, int rank)
        {
            sendCommand("GIVE_CARD:" + suit + ":" + rank);
        }

        public string takeTurn()
        {
            sendCommand("TAKE_TURN");
            return getResponse();
        }

        public void sendBuyIn(int buyIn)
        {
            sendCommand("SEND_BUY_IN:" + buyIn);
        }

        public void sendKicked()
        {
            sendCommand("PLAYER_KICKED");
        }

        public void sendCurrentState(string state)
        {
            sendCommand("CURRENT_STATE:" + state);
        }

        public void giveChips(uint chips)
        {
            sendCommand("GIVE_CHIPS:" + chips);
        }

        public void takeChips(uint chips)
        {
            sendCommand("TAKE_CHIPS:" + chips);
        }

        public void startGame()
        {
            sendCommand("START_GAME");
        }

        public void returnToLobby()
        {
            sendCommand("RETURN_TO_LOBBY");
        }
    }
}