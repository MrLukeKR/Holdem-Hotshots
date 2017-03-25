using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicUnitTesting
{
    [TestClass]
    public class Card
    {
        [TestMethod]
        public void dealToPlayers(){
            ServerPlayer currPlayer = null;
            for (int i = 0; i < room.players.Count; i++)
            {
                currPlayer = room.players[i];
                Console.WriteLine("Dealing card to " + currPlayer.name);
                currPlayer.GiveCard(deck.TakeCard());
            }
        }
        
        public void placeBets() {
            Console.WriteLine("Room size is " + room.players.Count);
            ServerPlayer currentPlayer = null;
            for (int i = 0; i < room.players.Count; i++)
            {
                if (room.GetRemainingPlayers() > 1)
                {
                    currentPlayer = room.players[i];
                    currentPlayer.TakeTurn();
                    
                    while (!currentPlayer.hasTakenTurn && !currentPlayer.folded) { Thread.Sleep(1000); }
                    currentPlayer.hasTakenTurn = false;
                }
            }
        }
    }
}
