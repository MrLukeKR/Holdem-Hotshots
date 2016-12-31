using System;
using System.Threading.Tasks;
using Urho;

namespace TexasHoldemPoker{
  class PokerGame{
    private Table pokerTable;
    public PokerGame(Room room, Scene tableScene, uint buyIn){
      pokerTable = new Table(room, tableScene, buyIn);
    }

        public void start()
        {

            //TODO: Separate thread from UI
            Task.Run(() =>run());
        }

        public async void run()
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
