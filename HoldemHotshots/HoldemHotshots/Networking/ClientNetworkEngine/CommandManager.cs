using HoldemHotshots.GameLogic.Player;
using HoldemHotshots.Managers;
using HoldemHotshots.Utilities;
using System;

namespace HoldemHotshots.Networking.ClientNetworkEngine
{

    /// <summary>
    /// Manager for translating commands for the server and then executes them
    /// </summary>
    class CommandManager
    {
        private static CommandManager commandManager;
        private readonly ServerConnection connection;
        private readonly ClientPlayer player;


        /// <summary>
        /// Constructor for CommandManager
        /// </summary>
        /// <param name="connection">Connection to server</param>
        /// <param name="player">Client-side version of the player game object</param>
        private CommandManager(ServerConnection connection, ClientPlayer player)
        {
            this.connection = connection;
            this.player = player;
        }
        /// <summary>
        /// Returns object reference to singleton CommandManager
        /// </summary>
        /// <param name="connection">Connection to server</param>
        /// <param name="player">Client-side version of the player game object</param>
        /// <returns>Object reference to CommandManager</returns>
        public static CommandManager getInstance(ServerConnection connection, ClientPlayer player)
        {
            if (commandManager == null)
                commandManager = new CommandManager(connection, player);

            return commandManager;
        }

        /// <summary>
        /// Takes a command and executes it
        /// </summary>
        /// <param name="command">Command to be exceuted</param>
        public void RunCommand(string command)
        {
            string[] args = command.Split(':');

            if (command.Length == 0)
                return;

            foreach (string arg in args) Console.WriteLine(arg);
            
            switch (args[0])
            {
                case "MAX_PLAYERS_ERROR":
                    //TODO: Call Max Players Method
                    break;
                case "GET_PLAYER_NAME":
                    sendPlayerName();
                    break;
                case "GIVE_CARD":
                    if (args.Length == 3) giveCard(int.Parse(args[1]), int.Parse(args[2]));
                    else Console.WriteLine("Insufficient arguments for command 'GIVE_CARD'");
                    break;
                case "TAKE_TURN":
                    takeTurn();
                    break;
                case "SEND_BUY_IN":
                    if(args.Length == 2) sendBuyIn();
                    break;
                case "PLAYER_KICKED":
                    playerKicked();
                    break;
                case "PLAYER_BID":
                    if (args.Length == 2)
                       player.playerBid = uint.Parse(args[1]);
                    break;
                case "HIGHEST_BID":
                    if (args.Length == 2)
                        ClientManager.highestBid = uint.Parse(args[1]);
                    break;
                 case "RESET_STAKES":
                    player.ResetStakes();
                    break;
                case "SET_CHIPS":
                    if (args.Length == 2) SetChips(uint.Parse(args[1]));
                    break;
                case "PING":
                    Pong();
                    break;
                case "DISPLAY_MESSAGE":
                    if (args.Length == 2) DisplayMessage(args[1]);
                    break;
                case "RESET_INTERFACE":
                    ResetInterface();
                    break;
                default:
                    Console.WriteLine("Client recieved a message from server that was not found");
                    break;
            }
        }

        //Private methods that deal with exceution of commands

        private void ResetInterface()
        {
            player.ResetInterface();
        }

        public void Ping()
        {
            connection.SendMessage("PING");
        }

        private void Pong()
        {
            connection.SendMessage("PONG");
        }

        private void sendPlayerName()
        {
            connection.SendMessage("SET_NAME:" + UIUtils.GetPlayerName());
        }

        private void giveCard(int suit, int rank)
        {
            player.GiveCard(suit,rank);
        }

        private void takeTurn()
        {
            player.TakeTurn();
        }

        private void sendBuyIn()
        {
            //TODO: implement send buyin
            int buyin = 0;
            player.SetBuyIn(buyin);
        }

        private void playerKicked()
        {
            //TODO: call player.kick()
            //player.kick();
        }

        private void SetChips(uint amount)
        {
            player.SetChips(amount);
        }

        private void DisplayMessage(string message)
        {
            UIUtils.DisplayPlayerMessage(message);
        }
    }
}