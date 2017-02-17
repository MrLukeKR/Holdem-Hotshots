using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HoldemHotshots.Networking.ServerNetworkEngine
{
    class ServerCommandListenerThread
    {

        private ServerCommandManager commandmanager;
        private ClientConnection connection;

        public ServerCommandListenerThread(ClientConnection connection, ServerPlayer player)
        {
            this.commandmanager = ServerCommandManager.getInstance(connection, player);
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
