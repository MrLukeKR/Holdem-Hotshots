using System;
using System.Threading;
using System.Threading.Tasks;
using Urho;
using Urho.Gui;
using Urho.Resources;

namespace HoldemHotshots{
  class PokerGame{
    private Table pokerTable;
    public PokerGame(Room room, Scene tableScene, UI ui, ResourceCache cache, uint buyIn){
      pokerTable = new Table(room, tableScene, ui, cache, buyIn);
    }

        public void start()
        {
            //Runs separately to the UI
            Task.Run(() =>run());
        }

        public async void run()
        {

            for (int i = 0; i < 2; i++)
                pokerTable.dealToPlayers();
                
            await pokerTable.placeBets();

            Thread.Sleep(2000);//Debugging

            pokerTable.flop();

            Thread.Sleep(2000);//Debugging

            for (int i = 0; i < 2; i++)
            {
                pokerTable.dealToTable(3+i);
                await pokerTable.placeBets();
                Thread.Sleep(2000); //Debugging
            }

            pokerTable.showdown();
        }
    }
}
