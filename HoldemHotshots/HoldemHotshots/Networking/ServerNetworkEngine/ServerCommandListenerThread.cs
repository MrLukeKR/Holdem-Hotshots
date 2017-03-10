using HoldemHotshots.GameLogic.Player;
using System.Threading;

namespace HoldemHotshots
{
    class ServerCommandListenerThread
    {
        private readonly ServerCommandManager commandmanager;
        private readonly ClientConnection clientConnection;

        public ServerCommandListenerThread(ClientConnection connection, ServerPlayer player)
        {
            commandmanager = ServerCommandManager.getInstance(connection, player);
            clientConnection = connection;
        }

        public void Start()
        {
            new Thread(ListenForCommands).Start();
        }

        private void ListenForCommands()
        {
            while (clientConnection.connection.Connected)
            {
                string command = clientConnection.GetCommand();

                if (command.Length > 0)
                {
                    clientConnection.monitorThread.ResetCommandTimer();
                    commandmanager.runCommand(command);
                }
            }
        }
    }
}