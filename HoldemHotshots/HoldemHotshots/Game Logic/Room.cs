using HoldemHotshots.GameLogic.Player;
using HoldemHotshots.Managers;
using HoldemHotshots.Utilities;
using System.Collections.Generic;

namespace HoldemHotshots.GameLogic
{
    /// <summary>
    /// Stores a group of players
    /// </summary>
    public class Room
    {
        public List<ServerPlayer> players = new List<ServerPlayer>();
        public const int MAX_ROOM_SIZE = 6;
        
        public Room(){}

        /// <summary>
        /// Counts the amount of players left in the game that haven't folded
        /// </summary>
        /// <returns>Amount of unfolded players</returns>
        public int GetRemainingPlayers()
        {
            int remaining = 0;

            foreach (ServerPlayer player in players)
                if (!player.folded)
                    remaining++;

            return remaining;
        }

        /// <summary>
        /// Checks if players are still connected - if not, it removes them from the round
        /// </summary>
        public void CheckConnections()
        {
            foreach (ServerPlayer player in players)
                if (!player.IsConnected())
                {
                    if (!player.folded)
                    {
                        player.Fold();
                        SpeechManager.Speak(player.name + " has disconnected");
                    }
                }
        }

        /// <summary>
        /// If players are still disconnected at the end of a game, they're removed from the lobby
        /// </summary>
        public void Cleanup()
        {
            List<ServerPlayer> toRemove = new List<ServerPlayer>();

            foreach (ServerPlayer player in players)
                if (!player.IsConnected()) { toRemove.Add(player); }

            foreach (ServerPlayer player in toRemove)
            {
                SpeechManager.Speak(player.name + " has left the room");
                player.Kick();
                players.Remove(player);
            }

            toRemove.Clear();

            UIUtils.UpdatePlayerList(this);
        }
    }
}
