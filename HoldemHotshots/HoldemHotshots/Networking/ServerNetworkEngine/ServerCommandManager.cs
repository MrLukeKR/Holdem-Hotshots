using HoldemHotshots.GameLogic;
using HoldemHotshots.GameLogic.Player;
using HoldemHotshots.Managers;
using System;
using System.Collections.Generic;

namespace HoldemHotshots.Networking.ServerNetworkEngine
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
            SpeechManager.Speak(player.name + " raises by " + amount);
            pot.PayIn(player.TakeChips(pot.latestBet + amount));
            player.hasTakenTurn = true;
        }

        private void Call()
        {
            SpeechManager.Speak(player.name + " calls " + pot.latestBet);
            pot.PayIn(player.TakeChips(pot.latestBet));
            player.hasTakenTurn = true;
        }

        private void Fold()
        {
            SpeechManager.Speak(player.name + " folds");
            player.Fold();
            player.hasTakenTurn = true;
        }

        private void AllIn()
        {
            SpeechManager.Speak(player.name + " goes All-In with " + player.chips);
            pot.PayIn(player.TakeChips(player.chips));
            player.hasTakenTurn = true;
        }

        private void Check()
        {
            SpeechManager.Speak(player.name + " checks");
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
