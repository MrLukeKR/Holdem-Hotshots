using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using TexasHoldemPoker.Game.PokerObjects;


namespace MixedRealityPoker.Game.PokerObjects
{
    class Player
    {
        uint id;
        String name;
        String ip;
        uint chips;
        Card[] hand;
        Socket connection;

        public Player(Socket connection)
        {
            //Just going to do some player init stuff here

            this.connection = connection;

        }


        /*
         * Method used to send messages to the player's client device
         */ 
        private void sendMessage(String message)
        {

            Byte[] messageBuffer = new Byte[255];
            messageBuffer = Encoding.ASCII.GetBytes(message);

            connection.Send(messageBuffer);
        
        }
    }
}
