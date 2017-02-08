using System;

namespace HoldemHotshots
{

    class CommandManager
    {
        private static CommandManager commandManager;
        private ServerConnection connection;

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
                    sendPlayerName();
                    break;
                default:
                    Console.WriteLine("Client recieved a message from server that was not found");
                    break;

            }


        }

        private void sendPlayerName()
        {
            Console.WriteLine("Sending name...");
            this.connection.sendMessage(UIUtils.GetPlayerName());
            Console.WriteLine("Name sent");
            
        }
    }
}
