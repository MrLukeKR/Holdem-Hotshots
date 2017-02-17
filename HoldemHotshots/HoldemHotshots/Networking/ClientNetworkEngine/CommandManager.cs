using System;
using HoldemHotshots;

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
                commandManager = new CommandManager(connection,player);
            }

            return commandManager;
        }

        public void runCommand(String command)
        {
            switch (command)
            {
                case "MAX_PLAYERS_ERROR":
                    //TODO: Call Max Players Method
                    break;
                case "GET_PLAYER_NAME":
                    sendPlayerName();
                    break;
                case "GET_PLAYER_ACTION":
                    sendPlayerAction();
                    break;
                case "ANIMATE_CARD":
                    animateCard();
                    break;
                case "GIVE_CARD":
                    giveCard();
                    break;
                case "TAKE_TURN":
                    takeTurn();
                    break;
                case "SEND_BUY_IN":
                    sendBuyIn();
                    break;
                case "PLAYER_KICKED":
                    playerKicked();
                    break;
                case "CURRENT_STATE":
                    sentCurrentState();
                    break;
                case "GIVE_CHIPS":
                    giveChips();
                    break;
                case "START_GAME":
                    startGame();
                    break;
                case "RETURN_TO_LOBBY":
                    returnToLobby();
                    break;
                case "TAKE_CHIPS":
                    takeChips();
                default:
                    Console.WriteLine("Client recieved a message from server that was not found");
                    break;

            }


        }

        private void sendPlayerAction()
        {
            //TODO: switch(player.takeTurn());
        }

        private void sendPlayerName()
        {
            Console.WriteLine("Sending name...");
            this.connection.sendMessage(UIUtils.GetPlayerName());
            Console.WriteLine("Name sent");
        }

        private void animateCard()
        {
            int cardindex = int.Parse(connection.getResponse());

            //TODO : Fix the issue with animate card (void to int error) 
            player.animateCard(cardindex);
        }

        private void giveCard()
        {
            //TODO: implement giveCard()

            int suit = int.Parse(connection.getResponse());
            int rank = int.Parse(connection.getResponse());

            player.giveCard(suit,rank);
        }

        private void takeTurn()
        {
            string playeraction = player.takeTurn();
            connection.sendMessage(playeraction);
        }

        private void sendBuyIn()
        {
            //TODO: implement send buyin
            int buyin = int.Parse(connection.getResponse());
            player.sendbuyin(buyin);
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

        private void giveChips()
        {
            string chips = connection.getResponse();
            uint chipnumber = uint.Parse(chips);

            player.giveChips(chipnumber);
        }

        private void startGame()
        {
           //TODO: call start game method on correct object
        }

        private void returnToLobby()
        {
          //TODO: call return to lobby method on correct object
        }

        private void takeChips()
        {
            uint chips = uint.Parse(connection.getResponse());

            player.takeChips(chips);

        }

    }
}