using HoldemHotshots.GameLogic.Player;
using HoldemHotshots.Utilities;
using System.Collections.Generic;

namespace HoldemHotshots.GameLogic
{
    //This class uses getter and setter functions, where as other parts of the
    //code base use the C# style of writing the getter and setter into the
    //property declaration - need discussion on consistant style.
    
    public class Room
    {
        public List<ServerPlayer> players = new List<ServerPlayer>();
        public const int MAX_ROOM_SIZE = 6;
        
        public Room()
        {

        }

        public int GetRemainingPlayers()
        {
            int remaining = 0;

            foreach (ServerPlayer player in players)
                if (!player.folded)
                    remaining++;

            return remaining;
        }

        public void CheckConnections()
        {
            foreach (ServerPlayer player in players)
                if (!player.IsConnected()) { player.Fold(); }
        }

        public void Cleanup()
        {
            List<ServerPlayer> toRemove = new List<ServerPlayer>();

            foreach (ServerPlayer player in players)
                if (!player.IsConnected()) { toRemove.Add(player); }

            foreach (ServerPlayer player in toRemove) players.Remove(player);

            toRemove.Clear();

            UIUtils.UpdatePlayerList(this);
        }
    }
}
