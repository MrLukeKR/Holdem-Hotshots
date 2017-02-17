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

        private void sendCommand(String command)
        {
            Console.WriteLine("command: '" + command + "' sent");
            byte[] messageBuffer = Encoding.ASCII.GetBytes(command);
            connection.Send(messageBuffer);
        }

        private String getResponse()
        {
            Byte[] Buffer;
            Buffer = new Byte[255];
            int messageSize = connection.Receive(Buffer, 0, Buffer.Length, 0);
            Array.Resize(ref Buffer, messageSize);
            String response = Encoding.Default.GetString(Buffer);
            Console.WriteLine("Response: '" + response + "' recieved");
            return response;
        }

        //Commands
        public String getName()
        {
            this.sendCommand("GET_PLAYER_NAME");
            return this.getResponse();
        }

        public void sendTooManyPlayers()
        {
            this.sendCommand("MAX_PLAYERS_ERROR");
        }

        public void sendPlayerKicked()
        {
            this.sendCommand("PLAYER_KICKED");
        }

        public String getPlayerAction()
        {
            this.sendCommand("GET_PLAYER_ACTION");
            return this.getResponse();
        }

        public int getBet()
        {
            this.sendCommand("GET_PLAYER_BET");
            return Int32.Parse(this.getResponse());
        }

        public void animateCard(int cardvalue)
        {
            this.sendCommand("ANIMATE_CARD");
            this.sendCommand(cardvalue.ToString());
        }

        public void giveCard(int suit, int rank)
        {
            this.sendCommand("GIVE_CARD");
            this.sendCommand(suit.ToString());
            this.sendCommand(rank.ToString());
        }

        public string takeTurn()
        {
            sendCommand("TAKE_TURN");
            return getResponse();
        }

        public void sendBuyIn(int buyin)
        {
            sendCommand("SEND_BUY_IN");
            sendCommand(buyin.ToString());
        }

        public void sendKicked()
        {
            sendCommand("PLAYER_KICKED");
        }

        public void sendCurrentState(string state)
        {
            sendCommand("CURRENT_STATE");
            sendCommand(state);
        }

        public void giveChips(uint chips)
        {
            sendCommand("GIVE_CHIPS");
            sendCommand(chips.ToString());
        }

        public void startGame()
        {
            sendCommand("START_GAME");
        }

        public void returnToLobby()
        {
            sendCommand("RETURN_TO_LOBBY");
        }

        public void takeChips(uint chips)
        {
            sendCommand("TAKE_CHIPS");
            sendCommand(chips.ToString());
        }





    }
}