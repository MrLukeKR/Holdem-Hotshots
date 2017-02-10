using System;
using System.Threading;
using HoldemHotshots.ClientPlayer;

namespace HoldemHotshots
{
    class CommandListenerThread
    {
        private CommandManager commandmanager;
        private ServerConnection connection;

        public CommandListenerThread(ServerConnection connection,ClientPlayer player)
        {
            this.commandmanager = CommandManager.getInstance(connection,player);
            this.connection = connection;
        }

        public void Start()
        {
            var listener = new Thread(listenForCommands);

            listener.Start();
        }

        private void listenForCommands()
        {
            while (true)
            {
                String command = connection.getResponse();
                commandmanager.runCommand(command);
            }
        }

    }
}
