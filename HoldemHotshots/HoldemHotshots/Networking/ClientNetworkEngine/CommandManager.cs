using System;
using System.Collections.Generic;
using System.Text;

namespace HoldemHotshots.Networking.ClientNetworkEngine
{
    private static CommandManager commandManager;
    private ServerConnection connection;

    class CommandManager
    {
        private CommandManager(ServerConnection connection)
        {
            this.connection = connection;
        }

        public static CommandManager getInstance(ServerConnection connection)
        {
            if (commandManager == null)
            {
                commandManager = new CommandManager(connection);
            }

            return commandManager;
        }

        public void runCommand(String command)
        {
            switch (command)
            {
                case "MAX_PLAYERS_ERROR":
                    //Call Max Players Method
                    break;
                case "GET_PLAYER_NAME":
                //Call player name method
                default:
                    Console.Write("Client recieved a message from server that was not found");
                    break;

            }


        }
    }
}
