using System;
using Urho;

namespace TexasHoldemPoker{
  class PokerGame{
    private Table pokerTable;
    public PokerGame(Room room, Scene tableScene, uint buyIn){
      pokerTable = new Table(room, tableScene, buyIn);
    }
    public void run(){
      pokerTable.flop();
      pokerTable.placeBets();
      for (int i = 0; i < 2; i++){
        pokerTable.dealCards();
        pokerTable.placeBets();
      }
      pokerTable.showdown();
    }
  }
}
