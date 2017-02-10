using System;
using System.Collections.Generic;

namespace HoldemHotshots{
  //This class uses getter and setter functions, where as other parts of the
  //code base use the C# style of writing the getter and setter into the
  //property declaration - need discussion on consistant style.
  
 public class Room{
    private List<ServerPlayer> players = new List<ServerPlayer>();
        public int MaxRoomSize { get; set; } = 6;
    public Room() { }
    public void addPlayer(ServerPlayer player) { players.Add(player); }
    public void removePlayer(int index) { players.RemoveAt(index); }
    public void removePlayer(ServerPlayer player) { players.Remove(player); }
    public int getRoomSize() { return players.Count; }
    internal ServerPlayer getPlayer(int i) { return players[i]; }
    public override String ToString(){
      String playerList = "PLAYERS IN ROOM:\n\n";
      for(int i = 0; i < players.Count; i ++){
        playerList += players[i].ToString() + "\n";
      }
      return playerList;
    }
  }
}
