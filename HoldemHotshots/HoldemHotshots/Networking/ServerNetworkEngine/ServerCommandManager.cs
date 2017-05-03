using HoldemHotshots.GameLogic;
using HoldemHotshots.GameLogic.Player;
using HoldemHotshots.Managers;
using HoldemHotshots.Utilities;
using System;
using System.Collections.Generic;
using Urho;

namespace HoldemHotshots.Networking.ServerNetworkEngine
{
    /// <summary>
    /// Responsible for running the commands on the server
    /// </summary>
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

        /// <summary>
        /// Returns the command manager linked to a given player
        /// </summary>
        /// <param name="connection">Client's connection</param>
        /// <param name="player">Player to find the Command Manager of</param>
        /// <returns></returns>
        public static ServerCommandManager GetInstance(ClientConnection connection, ServerPlayer player)
        {
			if (GetPlayerInstance(player) == null) commandManagers.Add (new ServerCommandManager(connection, player));

			return GetPlayerInstance(player);
        }

        /// <summary>
        /// Sets the pot reference
        /// </summary>
        /// <param name="chipPot">Pot object</param>
        public static void SetPot(Pot chipPot)
        {
            pot = chipPot;
        }

        /// <summary>
        /// Returns the player associated with this command manager
        /// </summary>
        /// <returns>Server-side Player</returns>
		public ServerPlayer GetPlayer()
		{
			return player;
		}

        /// <summary>
        /// Returns the command manager of a specific player
        /// </summary>
        /// <param name="player">Server-side player</param>
        /// <returns>Command manager</returns>
		public static ServerCommandManager GetPlayerInstance(ServerPlayer player)
		{
			foreach (ServerCommandManager cm in commandManagers)
				if (cm.GetPlayer() == player) return cm;
			return null;
		}

        /// <summary>
        /// Runs a given command on the server
        /// </summary>
        /// <param name="command">Command to run</param>
        public void RunCommand(string command)
        {
            string[] args = command.Split(':');

            switch (args[0])
            {
                case "RAISE":
                    if (args.Length == 2) Raise(uint.Parse(args[1]));
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
                    break;
                default:
                    Console.WriteLine("Command not found");
                    break;
            }
        }

        /// <summary>
        /// Sets the player's name on the server side based on client-side input
        /// </summary>
        /// <param name="name">New player name</param>
        private void SetName(string name)
        {
            player.name = name;
        }

        /// <summary>
        /// Raises the current stake by a given amount
        /// </summary>
        /// <param name="amount">Amount to raise by</param>
        private void Raise(uint amount)
        {
            SpeechManager.Speak(player.name + " raises " + amount + " " + AppValuesManager.CURRENCY);

            Application.InvokeOnMain(new Action(() =>
            SceneUtils.UpdatePlayerInformation(player.name, "Raised " + AppValuesManager.CURRENCY_SYMBOL + amount)));

            pot.PayIn(player.TakeChips((pot.stake - player.currentStake) + amount), player.currentStake);
            player.hasTakenTurn = true;
        }

        /// <summary>
        /// Calls the current stake
        /// </summary>
        private void Call()
        {
            SpeechManager.Speak(player.name + " calls");
                
            Application.InvokeOnMain(new Action(() => SceneUtils.UpdatePlayerInformation(player.name, "Called ")));

            pot.PayIn(player.TakeChips(pot.stake - player.currentStake), player.currentStake);
            player.hasTakenTurn = true;
        }

        /// <summary>
        /// Folds the current player
        /// </summary>
        private void Fold()
        {
            SpeechManager.Speak(player.name + " folds");
            
            Application.InvokeOnMain(new Action(() =>
            SceneUtils.UpdatePlayerInformation(player.name, "Folded")));

            player.Fold();
            player.hasTakenTurn = true;
        }

        /// <summary>
        /// Sends all of the player's chips into the pot
        /// </summary>
        private void AllIn()
        {
            var chips = player.chips; //Have to store the chips before calling out-of-thread code for concurrency purposes

            SpeechManager.Speak(player.name + " goes All-In with " + player.chips + " " + AppValuesManager.CURRENCY);
            
            Application.InvokeOnMain(new Action(() =>
            SceneUtils.UpdatePlayerInformation(player.name, "All~In " + AppValuesManager.CURRENCY_SYMBOL + chips)));

            pot.PayIn(player.TakeChips(player.chips), player.currentStake);
            player.hasTakenTurn = true;
        }

        /// <summary>
        /// Performs a check operation (do nothing - i.e. zero call/raise)
        /// </summary>
        private void Check()
        {
            SpeechManager.Speak(player.name + " checks");

            Application.InvokeOnMain(new Action(() =>
            SceneUtils.UpdatePlayerInformation(player.name, "Checks")));

            player.hasTakenTurn = true;
        }

        private void Pong()
        {
            connection.SendCommand("PONG");
        }

        private void Ping()
        {
            connection.SendCommand("PING");
        }

        /// <summary>
        /// Disconnecs the client connection
        /// </summary>
        private void Disconnect()
        {
            connection.connection.Disconnect(true);
        }
    }
}