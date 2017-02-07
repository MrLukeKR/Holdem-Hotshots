using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace HoldemHotshots
{
    class CommandListenerThread
    {
        private CommandManager commandmanager;
        private ServerConnection connection;

        public CommandListenerThread(ServerConnection connection)
        {
            this.commandmanager = CommandManager.getInstance(connection);
            this.connection = connection;
        }

        public void Start()
        {
            listenForCommands();
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
