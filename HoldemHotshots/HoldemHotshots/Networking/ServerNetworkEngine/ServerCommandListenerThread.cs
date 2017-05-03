using HoldemHotshots.GameLogic.Player;
using System.Threading;

namespace HoldemHotshots.Networking.ServerNetworkEngine
{
    /// <summary>
    /// Responsible for receiving commands from the client
    /// </summary>
    class ServerCommandListenerThread
    {
        private readonly ServerCommandManager commandmanager;
        private readonly ClientConnection clientConnection;

        public ServerCommandListenerThread(ClientConnection connection, ServerPlayer player)
        {
            commandmanager = ServerCommandManager.GetInstance(connection, player);
            clientConnection = connection;
        }

        /// <summary>
        /// Starts the listener thread
        /// </summary>
        public void Start()
        {
            new Thread(ListenForCommands).Start();
        }

        /// <summary>
        /// Listens for commands in  a loop until the client is disconnected
        /// </summary>
        private void ListenForCommands()
        {
            while (clientConnection.connection.Connected)
            {
                string command = clientConnection.GetCommand();

                if (command.Length > 0)
                {
                    clientConnection.monitorThread.ResetCommandTimer();
                    commandmanager.RunCommand(command);
                }
            }
        }
    }
}