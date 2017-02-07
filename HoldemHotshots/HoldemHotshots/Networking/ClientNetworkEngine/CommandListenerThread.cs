using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace HoldemHotshots.Networking.ClientNetworkEngine
{
    class CommandListenerThread
    {
        private CommandManager commandmanager;

        public CommandListenerThread(ServerConnection connection)
        {
            this.commandmanager = commandmanager.getInstance(connection);
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
