using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace TexasHoldemPoker.Game.NetworkEngine.ServerNetworkEngine
{
    /*
     * This class is a wrapper for the client socket and contains commands that can be sent to the client
     */

    class ClientConnection
    {
        Socket connection;

        public ClientConnection(Socket connection)
        {
            this.connection = connection;
        }

        private void sendCommand(String command){

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

        public void getPlayerAction()
        {
            this.sendCommand("GET_PLAYER_ACTION");

            return this.getResponse;

        }

        public int getBet()
        {
            this.sendCommand("GET_PLAYER_BET");
            return Int32.Parse(this.getResponse);

        }

    }
}
