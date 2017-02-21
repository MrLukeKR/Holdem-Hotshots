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
            if (commandManager == null)
            {
                commandManager = new ServerCommandManager(connection, player);
            }

            return commandManager;
        }

        public void runCommand(string command)
        {
            String[] args = command.Split(':');

            switch (args[0])
            {
                case "RAISE":
                    if (args.Length == 2)
                        Raise(uint.Parse(args[1]));
                    else
                        Console.WriteLine("Insufficient arguments for command 'Raise'");
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

        }

        private void Call()
        {

        }

        private void Fold()
        {

        }

        private void AllIn()
        {

        }

        private void Check()
        {

        }

        private void Pong()
        {

        }

        private void Ping()
        {

        }

        private void Disconnect()
        {

        }
    }
}
