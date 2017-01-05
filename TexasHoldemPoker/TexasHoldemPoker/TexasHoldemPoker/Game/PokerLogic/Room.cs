using System;
using System.Collections.Generic;

namespace TexasHoldemPoker{
  //This class uses getter and setter functions, where as other parts of the
  //code base use the C# style of writing the getter and setter into the
  //property declaration - need discussion on consistant style.
  
  class Room{
    private List<Player> players = new List<Player>();
    int MaxRoomSize { get; set; }
    public Room() { }
    public void addPlayer(Player player) { players.Add(player); }
    public void removePlayer(int index) { players.RemoveAt(index); }
    public void removePlayer(Player player) { players.Remove(player); }
    public int getRoomSize() { return players.Count; }
    internal Player getPlayer(int i) { return players[i]; }
    public override String ToString(){
      String playerList = "PLAYERS IN ROOM:\n\n";
      for(int i = 0; i < players.Count; i ++){
        playerList += players[i].ToString() + "\n";
      }
      return playerList;
    }
  }
}
