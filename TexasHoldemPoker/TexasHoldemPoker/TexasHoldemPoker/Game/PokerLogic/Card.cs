using System;

namespace PokerLogic
{
    class Card
    {
        public enum Rank { ACE = 1, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING }
        public enum Suit { CLUBS, SPADES, DIAMONDS, HEART }

        Suit suit;
        Rank rank;

        public Card(Suit suit, Rank rank)
        {
            this.suit = suit;
            this.rank = rank;
        }

        public override String ToString()
        {
            String sSuit = "", sRank = "";

            switch (suit)
            {
                case Suit.CLUBS: sSuit = "Clubs"; break;
                case Suit.DIAMONDS: sSuit = "Diamonds"; break;
                case Suit.HEART: sSuit = "Hearts"; break;
                case Suit.SPADES: sSuit = "Spades"; break;
            }

            switch (rank)
            {
                case Rank.ACE:sRank = "Ace"; break;
                case Rank.TWO: sRank = "Two"; break;
                case Rank.THREE: sRank = "Three"; break;
                case Rank.FOUR: sRank = "Four"; break;
                case Rank.FIVE: sRank = "Five"; break;
                case Rank.SIX: sRank = "Six"; break;
                case Rank.SEVEN: sRank = "Seven"; break;
                case Rank.EIGHT: sRank = "Eight"; break;
                case Rank.NINE: sRank = "Nine"; break;
                case Rank.TEN: sRank = "Ten"; break;
                case Rank.JACK: sRank = "Jack"; break;
                case Rank.QUEEN: sRank = "Queen"; break;
                case Rank.KING: sRank = "King"; break;
            }

            return sRank + " of " + sSuit;
        }
    }
}
