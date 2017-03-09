using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HoldemHotshots
{
    class ServerCommandListenerThread
    {
        private ServerCommandManager commandmanager;
        private ClientConnection clientConnection;

        public ServerCommandListenerThread(ClientConnection connection, ServerPlayer player)
        {
            commandmanager = ServerCommandManager.getInstance(connection, player);
            clientConnection = connection;
            
        }

        public void Start()
        {
            var listener = new Thread(listenForCommands);

            listener.Start();
        }

        private void listenForCommands()
        {
            while (clientConnection.connection.Connected)
            {
                String command = clientConnection.GetCommand();
                if (command.Length > 0)
                {
                    ClientConnectionMonitorThread.ResetCommandTimer();
                    Console.WriteLine("Received command '" + command + "'");
                    commandmanager.runCommand(command);
                }
            }
        }

    }
}
