using System;

namespace TexasHoldemPoker
{
    class PokerGame
    {
        private Table pokerTable = new Table();
        private Room room;
        
        public PokerGame(Room room)
        {
            this.room = room;
        }

        public void start()
        {
            Console.WriteLine("Starting Game...");
            pokerTable.setRoom(room);
            run();
        }

        public void run()
        {
            Console.WriteLine("Running Game...");

            pokerTable.flop();
        }
    }
}
