using HoldemHotshots.Utilities;
using System.Threading;

namespace HoldemHotshots.GameLogic
{
  class PokerGame
    {
        private Table pokerTable;

        public PokerGame(Room room)
        {
            pokerTable = new Table(room);
        }
        
        public void Start()
        {
            pokerTable.assignBlinds();

            for (int i = 0; i < 2; i++)
                pokerTable.dealToPlayers();
            
            pokerTable.placeBets();
            
            pokerTable.Flop();
            pokerTable.placeBets();

            for (int i = 0; i < 2; i++)
            {
                pokerTable.dealToTable(3 + i);
                pokerTable.placeBets();
            }

            pokerTable.showdown();

            Thread.Sleep(3000);
            UIUtils.ShowRestartOptions();
        }
    }
}
