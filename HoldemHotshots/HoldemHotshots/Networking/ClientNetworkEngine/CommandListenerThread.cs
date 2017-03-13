using HoldemHotshots.GameLogic.Player;
using System.Threading;

namespace HoldemHotshots.Networking.ClientNetworkEngine
{
    class CommandListenerThread
    {
        private readonly CommandManager commandmanager;
        private readonly ServerConnection connection;

        public CommandListenerThread(ServerConnection connection, ClientPlayer player)
        {
            commandmanager  = CommandManager.getInstance(connection, player);
            this.connection = connection;
        }

        public void Start()
        {
            new Thread(ListenForCommands).Start();
        }

        private void ListenForCommands()
        {
            while (true)
                commandmanager.runCommand(connection.GetCommand());
        }
    }
}