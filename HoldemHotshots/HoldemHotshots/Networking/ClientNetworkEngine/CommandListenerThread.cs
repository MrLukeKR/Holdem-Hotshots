using HoldemHotshots.GameLogic.Player;
using System.Threading;

namespace HoldemHotshots.Networking.ClientNetworkEngine
{
    /// <summary>
    /// Thread that listens for commands from the server and then executes them
    /// </summary>
    class CommandListenerThread
    {
        private readonly CommandManager commandmanager;
        private readonly ServerConnection connection;


        /// <summary>
        /// Constructor for CommandListenerThread
        /// </summary>
        /// <param name="connection"> Connection to the server </param>
        /// <param name="player"> Client-side version of the player object </param>
        public CommandListenerThread(ServerConnection connection, ClientPlayer player)
        {
            commandmanager  = CommandManager.getInstance(connection, player);
            this.connection = connection;
        }

        /// <summary>
        /// Runs the thread
        /// </summary>
        public void Start()
        {
            new Thread(ListenForCommands).Start();
        }
   
        private void ListenForCommands()
        {
            while (true)
                commandmanager.RrunCommand(connection.GetCommand());
        }
    }
}