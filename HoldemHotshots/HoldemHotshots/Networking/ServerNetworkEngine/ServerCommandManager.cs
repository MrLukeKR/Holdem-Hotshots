using System;
using System.Collections.Generic;
using System.Text;

namespace HoldemHotshots
{
    class ServerCommandManager
    {
        private static ServerCommandManager commandManager;
        private ClientConnection connection;
        private ServerPlayer player;

        private ServerCommandManager(ClientConnection connection, ServerPlayer player)
        {
            this.connection = connection;
            this.player = player;
        }

        public static ServerCommandManager getInstance(ClientConnection connection, ServerPlayer player)
        {
            if (commandManager == null) commandManager = new ServerCommandManager(connection, player);

            return commandManager;
        }

        public void runCommand(string command)
        {
            String[] args = command.Split(':');

            switch (args[0])
            {
                case "RAISE":
                    if (args.Length == 2) Raise(uint.Parse(args[1]));
                    else Console.WriteLine("Insufficient arguments for command 'Raise'");
                    break;
                case "CALL":
                    Call();
                    break;
                case "FOLD":
                    Fold();
                    break;
                case "ALL_IN":
                    AllIn();
                    break;
                case "CHECK":
                    Check();
                    break;
                case "DISCONNECT":
                    Disconnect();
                    break;
                case "PING":
                    Pong();
                    break;
                default:
                    Console.WriteLine("Command not found");
                    break;

            }
        }

        private void Raise(uint amount)
        {
            player.takeChips(0 + amount); //TODOL get latest bet
        }

        private void Call()
        {
            player.takeChips(0); //TODO: get latest bet
        }

        private void Fold()
        {
            player.hasFolded();
        }

        private void AllIn()
        {
            player.takeChips(player.getChips());
        }

        private void Check()
        {
            //TODO: notify players that player has checked (Do nothing else)
        }

        private void Pong()
        {
            connection.sendCommand("PONG");
        }

        private void Ping()
        {
            connection.sendCommand("PING");
        }

        private void Disconnect()
        {

        }
    }
}
