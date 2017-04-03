using HoldemHotshots.GameLogic.Player;
using HoldemHotshots.Utilities;
using System;

namespace HoldemHotshots.Networking.ClientNetworkEngine
{
    class CommandManager
    {
        private static CommandManager commandManager;
        private readonly ServerConnection connection;
        private readonly ClientPlayer player;

        private CommandManager(ServerConnection connection, ClientPlayer player)
        {
            this.connection = connection;
            this.player = player;
        }

        public static CommandManager getInstance(ServerConnection connection, ClientPlayer player)
        {
            if (commandManager == null)
                commandManager = new CommandManager(connection, player);

            return commandManager;
        }

        public void RrunCommand(string command)
        {
            string[] args = command.Split(':');
            
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
                        player.highestBid = uint.Parse(args[1]);
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

        private void ResetInterface()
        {
            player.ResetInterface();
        }

        private void Ping()
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