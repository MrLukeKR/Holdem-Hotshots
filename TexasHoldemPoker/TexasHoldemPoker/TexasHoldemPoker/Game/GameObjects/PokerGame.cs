namespace TexasHoldemPoker.Game.PokerObjects
{
    class PokerGame
    {
        enum State { INIT, RUNNING, SHOWDOWN, GAME_OVER };
        State state;
        Room room = new Room();
        Table table = new Table();
        uint round = 0;
        uint smallBlind, bigBlind;
        
        public PokerGame(uint smallBlind, uint bigBlind)
        {
            this.smallBlind = smallBlind;
            this.bigBlind = bigBlind;
        }

        public void init(Lobby lobby)
        {
            room.populateRoom(lobby);
        }

        public void start()
        {
            if(state != State.RUNNING)
                run();
        }

        private void run()
        {
            state = State.RUNNING;
            while (state == State.RUNNING)
            {
                //Play poker game
                for (int i = 0; i < room.getNumberOfPlayers(); i++)
                    processPlayer();
            }
        }

        private void processPlayer()
        {
            //Dummy implementation placeholder
        }


    }
}