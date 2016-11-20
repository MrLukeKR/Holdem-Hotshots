using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TexasHoldemPoker.Game.NetworkEngine
{
    /*
     * This class handles all outgoing communication from the
     * server to the player devices. The functions in this class
     * will handle this communication
     * 
     */


    class PlayerConnection
    {

        Socket connection;

        public PlayerConnection(Socket connection)
        {
            this.connection = connection;



        }




    }
}
