using HoldemHotshots.Utilities;
using System.Threading;

namespace HoldemHotshots.GameLogic
{
    /// <summary>
    /// Handles the game loop
    /// </summary>
  class PokerGame
    {
        private Table pokerTable;

        public PokerGame(Room room)
        {
            pokerTable = new Table(room);
        }
        
        /// <summary>
        /// Runs the game loop
        /// </summary>
        public void Start()
        {
            pokerTable.ApplyBlinds();

            for (int i = 0; i < 2; i++)
                pokerTable.DealToPlayers();
            
            pokerTable.PlaceBets();
            
            pokerTable.Flop();
            pokerTable.PlaceBets();

            for (int i = 0; i < 2; i++)
            {
                pokerTable.DealToTable(3 + i);
                pokerTable.PlaceBets();
            }

            pokerTable.Showdown();

            Thread.Sleep(3000);
            UIUtils.ShowRestartOptions();
        }
    }
}
