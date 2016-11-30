namespace TexasHoldemPoker.Game.PokerObjects
{
    class Card
    {
        private int suit, rank; //Change this from int to whatever type you use (try an enum)?

        //TODO: Provide handling of models/textures for the card
        public Card(int suit, int rank) //Change param types from int to whatever type you use (try an enum)?
        {
            this.suit = suit;
            this.rank = rank;
        }
        public int GetSuit()//Change return type from int to whatever type you use (try an enum)?
        {
            return suit;
        }

        public int GetRank()//Change return type from int to whatever type you use (try an enum)?
        {
            return rank;
        }
     
    }
}
