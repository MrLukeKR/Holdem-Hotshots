using System;
using System.Collections.Generic;
using System.Text;

namespace HoldemHotshots.Networking.ServerNetworkEngine
{
    class ServerCommandManager
    {
        private static ServerCommandManager commandManager;
        private ClientConnection connection;
        private ServerPlayer player;

        private ServerCommandManager(ClientConnection connection, ServerPlayer player)
        {
            this.connection = connection;
            this.player = player;
        }

        public static ServerCommandManager getInstance(ClientConnection connection, ServerPlayer player)
        {
            if (commandManager == null)
            {
                commandManager = new ServerCommandManager(connection, player);
            }

            return commandManager;
        }

        public void runCommand(string command)
        {
            switch (command)
            {
                case "SEND_ACTION":
                    sentAction();
                    break;
                default:
                    System.Console.WriteLine("Command not found");
                    break;

            }

        }

        private void sentAction()
        {
            //TODO: implement sendAction()
        }

    }
}
