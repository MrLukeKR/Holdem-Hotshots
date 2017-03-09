﻿using System;
using System.Text;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace HoldemHotshots
{
    /*
     * This class is a wrapper for the client socket and contains commands that can be sent to the client
     */

    class ClientConnection : ClientInterface
    {
        public Socket connection { get; private set; }
        private ClientConnectionMonitorThread monitorThread;

        public ClientConnection(Socket connection)
        {
            this.connection = connection;
            this.monitorThread = new ClientConnectionMonitorThread(connection);
            monitorThread.start();

        }

        public void sendCommand(String command)
        {
            bool sent = false;
            while (!sent)
            {
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
                    Console.WriteLine("command: '" + command + "' sent");
                }
                catch
                {
                    Console.WriteLine("Command '" + command + "' could not be sent!");
                    Thread.Sleep(1000);
                    //TODO: Resend any information if the connection is re-established
                }
            }
        }

        public String GetCommand()
        {
            String response = "";
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

                    Console.WriteLine("Response: '" + response + "' recieved");
                }

            }
            catch
            {

            }
            
            return response;
        }

        //Commands
        public void getName()
        {
            sendCommand("GET_PLAYER_NAME");
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

        public void takeTurn()
        {
            sendCommand("TAKE_TURN");
        }
        
        public void setChips(uint amount)
        {
            sendCommand("SET_CHIPS:" + amount);
        }

        public void sendKicked()
        {
            sendCommand("PLAYER_KICKED");
        }

        public void sendCurrentState(string state)
        {
            sendCommand("CURRENT_STATE:" + state);
        }

        public void startGame()
        {
            sendCommand("START_GAME");
        }

        public void returnToLobby()
        {
            sendCommand("RETURN_TO_LOBBY");
        }

        public void DisplayMessage(String message)
        {
            sendCommand("DISPLAY_MESSAGE:" + message);
        }

        public void ResetInterface()
        {
            sendCommand("RESET_INTERFACE");
        }

        public bool IsConnected()
        {
            return connection.Connected;
        }
    }
}