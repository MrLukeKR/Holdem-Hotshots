
using System;

namespace PokerLogic
{
    class Game
    {
        private Table pokerTable = new Table();
        private bool gameOver = false;
        private int round = 0;

        public Game()
        {

        }

        public void start()
        {
            Console.WriteLine("Starting Game...");

            pokerTable.populate();

            run();
        }

        public void run()
        {
            Console.WriteLine("Running Game...");

            pokerTable.init(1000, 10, 25);

            while (!gameOver)
            {
                Console.WriteLine("\nROUND " + round + ":\n");

                if (round == 0)
                {
                    pokerTable.dealBlinds();
                    pokerTable.dealToPlayers();
                }

                if (round == 1) pokerTable.flop();
                if (round > 1) pokerTable.dealToTable();
                
                pokerTable.printHand();
                pokerTable.placeBets();
                
                round++;

                if (pokerTable.getRemainingPlayers() == 1 || round == 4)
                    showdown();
            }
        }


        
        public void showdown()
        {
            Console.WriteLine("\nSHOWDOWN:\n");
            //TODO: DO CARD RANKING
            end();
        }

        public void end()
        {
            gameOver = true;
        }
    }
}
