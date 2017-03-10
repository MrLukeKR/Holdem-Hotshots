using HoldemHotshots.GameLogic.Player;
using System;
using System.Threading;

namespace HoldemHotshots
{
    class CommandListenerThread
    {
        private CommandManager commandmanager;
        private ServerConnection connection;

        public CommandListenerThread(ServerConnection connection, ClientPlayer player)
        {
            this.commandmanager = CommandManager.getInstance(connection, player);
            this.connection = connection;
        }

        public void Start()
        {
            var listener = new Thread(listenForCommands);

            listener.Start();
        }

        private void listenForCommands()
        {
            Console.WriteLine("Client is listening for commands");
            while (true)
            {
                String command = connection.GetCommand();
                Console.WriteLine("Received command '" + command + "'");
                commandmanager.runCommand(command);
            }
        }

    }
}
