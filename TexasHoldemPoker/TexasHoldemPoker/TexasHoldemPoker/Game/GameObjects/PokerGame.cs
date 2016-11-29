namespace TexasHoldemPoker.Game.PokerObjects
{
    class currentGame
    {
        Room room = new Room();

        public void init(Lobby lobby)
        {
            room.populateRoom(lobby);
        }
    }
}
