namespace HoldemHotshots.GameLogic
{
  class PokerGame
    {
        private Table pokerTable;

        bool exit = false;

        public PokerGame(Room room)
        {
            pokerTable = new Table(room);
        }
        
        public void Start()
        {
            while (!exit)
            {
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

                exit = true; //FOR DEBUGGING
            }
        }

        public void ResetGame()
        {
            pokerTable.ResetTable();
        }
    }
}
