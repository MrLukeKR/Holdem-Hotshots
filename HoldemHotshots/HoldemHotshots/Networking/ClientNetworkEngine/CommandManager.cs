using System;

namespace HoldemHotshots
{

    class CommandManager
    {
        private static CommandManager commandManager;
        private ServerConnection connection;
        private ClientPlayer player;

        private CommandManager(ServerConnection connection, ClientPlayer player)
        {
            this.connection = connection;
            this.player = player;
        }

        public static CommandManager getInstance(ServerConnection connection,ClientPlayer player)
        {
            if (commandManager == null)
            {
                commandManager = new CommandManager(connection, player);
            }

            return commandManager;
        }

        public void runCommand(String command)
        {
            String[] args = command.Split(':');

            Console.WriteLine("Command Split to:");
            foreach (String arg in args) Console.WriteLine(arg);

            Console.WriteLine("Client '" + player.getName() + "' received command '" + command + "'");

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
                    else Console.WriteLine("Insufficient arguments for command 'Raise'");
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
                case "CURRENT_STATE":
                    sentCurrentState();
                    break;
                case "START_GAME":
                    startGame();
                    break;
                case "RETURN_TO_LOBBY":
                    returnToLobby();
                    break;
                case "SET_CHIPS":
                    if (args.Length == 2) setChips(uint.Parse(args[1]));
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
            connection.sendMessage("PING");
        }

        private void Pong()
        {
            connection.sendMessage("PONG");
        }

        private void sendPlayerName()
        {
            Console.WriteLine("Sending name...");
            connection.sendMessage(UIUtils.GetPlayerName());
            Console.WriteLine("Name sent");
        }

        private void giveCard(int suit, int rank)
        {
            player.giveCard(suit,rank);
        }

        private void takeTurn()
        {
            player.takeTurn();
        }

        private void sendBuyIn()
        {
            //TODO: implement send buyin
            int buyin = int.Parse(connection.getResponse());
            player.setBuyIn(buyin);
        }

        private void playerKicked()
        {
            //TODO: call player.kicked
        }

        private void sentCurrentState()
        {
            string state = connection.getResponse();
            //TODO: pass state to method object that needs it
        }

        private void startGame()
        {
           //TODO: call start game method on correct object
        }

        private void returnToLobby()
        {
          //TODO: call return to lobby method on correct object
        }

        private void setChips(uint amount)
        {
            player.setChips(amount);

        }

        private void DisplayMessage(String message)
        {
            UIUtils.DisplayPlayerMessage(message);
        }
    }
}