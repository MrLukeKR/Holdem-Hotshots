using System;
using System.Collections.Generic;
using System.Timers;

namespace HoldemHotshots
{
    class ServerCommandManager
    {
        private static List<ServerCommandManager> commandManagers = new List<ServerCommandManager>();
        private static Pot pot;
        private ClientConnection connection;
        private ServerPlayer player;
        
        private ServerCommandManager(ClientConnection connection, ServerPlayer player)
        {
            this.connection = connection;
            this.player = player;
        }

        public static ServerCommandManager getInstance(ClientConnection connection, ServerPlayer player)
        {
			if (getPlayerInstance(player) == null) commandManagers.Add (new ServerCommandManager(connection, player));

			return getPlayerInstance(player);
        }

        public static void SetPot(Pot chipPot)
        {
            pot = chipPot;
        }

		public ServerPlayer getPlayer()
		{
			return player;
		}

		public static ServerCommandManager getPlayerInstance(ServerPlayer player)
		{
			foreach (ServerCommandManager cm in commandManagers)
				if (cm.getPlayer() == player) return cm;
			return null;
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
                case "SET_NAME":
                    if (args.Length == 2) SetName(args[1]);
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
                case "PONG":
                    Console.WriteLine("Ping was successful");
                    break;
                default:
                    Console.WriteLine("Command not found");
                    break;

            }
        }

        private void SetName(string name)
        {
            player.name = name;
        }

        private void Raise(uint amount)
        {
            pot.payIn(player.TakeChips(pot.GetLatestBet() + amount));
            player.hasTakenTurn = true;
        }

        private void Call()
        {
            pot.payIn(player.TakeChips(pot.GetLatestBet()));
            player.hasTakenTurn = true;
        }

        private void Fold()
        {
            player.Fold();
            player.hasTakenTurn = true;
        }

        private void AllIn()
        {
            pot.payIn(player.TakeChips(player.chips));
            player.hasTakenTurn = true;
        }

        private void Check()
        {
            //TODO: notify players that player has checked (Do nothing else)
            player.hasTakenTurn = true;
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
