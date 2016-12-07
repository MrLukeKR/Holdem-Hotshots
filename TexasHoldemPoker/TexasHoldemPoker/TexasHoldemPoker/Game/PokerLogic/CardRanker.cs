using PokerLogic;

namespace TexasHoldemPoker.Game.PokerObjects
{
    static class CardRanker
    {

        public static Player GetWinner(Player[] players)
        {
            // Go through all the players cards, call RankCards(players[1...2...3...n].GetCards()], Table.GetCards()) {Or something like that} and determine/return the winner

            return null; // Replace this with the winning player
        }

        public static void RankCards(Card[] cards)  //SHOULDN'T BE OF TYPE VOID, BUT CHANGE THIS TO WHATEVER YOU DECIDE TO BE APPROPRIATE FOR RANKING (Perhaps an enum type?)
        {

        }
    }
}
