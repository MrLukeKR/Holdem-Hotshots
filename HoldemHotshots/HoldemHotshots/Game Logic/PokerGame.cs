using Urho;
using Urho.Gui;
using Urho.Resources;

namespace HoldemHotshots{
  class PokerGame{
    private Table pokerTable;
    public PokerGame(Room room, uint buyIn){
      pokerTable = new Table(room, buyIn);
    }

        public void Start()
        {
            //Runs separately to the UI
            Run();
        }

        public async void Run()
        {

            for (int i = 0; i < 2; i++)
                pokerTable.dealToPlayers();
                
            await pokerTable.placeBets();

            pokerTable.flop();
            
            for (int i = 0; i < 2; i++)
            {
                pokerTable.dealToTable(3+i);
                await pokerTable.placeBets();
            }

            pokerTable.showdown();
        }
    }
}
