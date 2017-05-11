using HoldemHotshots.GameLogic.Player;
using HoldemHotshots.Utilities;
using System.Collections.Generic;

namespace HoldemHotshots.GameLogic
{
    /// <summary>
    /// Manages the card ranking aspect of the game (determines hand values)
    /// </summary>
    static class CardRanker
    {
        public enum Hand
        {
            ROYAL_FLUSH     = 10,
            STRAIGHT_FLUSH  = 9,
            FOUR_OF_A_KIND  = 8,
            FULL_HOUSE      = 7,
            FLUSH           = 6,
            STRAIGHT        = 5,
            THREE_OF_A_KIND = 4,
            TWO_PAIRS       = 3,
            PAIR            = 2,
            HIGH_CARD       = 1
        };
        
        /// <summary>
        /// Finds the winning player by evaluating every eligible player's hands (ignoring folded players)
        /// </summary>
        /// <param name="table">The table containing the table's cards</param>
        /// <param name="players">The list of players containing each player's cards</param>
        /// <returns>A list of winner(s) (Ties result in splitting the pot in this implementation)</returns>
        public static List<ServerPlayer> EvaluateGame(Table table, List<ServerPlayer> players)
        {
            Hand highestRank = 0, currentRank = 0;
            List<ServerPlayer> drawingPlayers = new List<ServerPlayer>();
            ServerPlayer highestPlayer = null;
            List<Card> allCards = new List<Card>();
            ServerPlayer previousPlayer = null;

            int i = 0;
            foreach(ServerPlayer player in players)
            {
                if (!player.folded)
                {
                    allCards.Clear();
                    allCards.AddRange(table.hand);
                    allCards.AddRange(player.hand);
                    currentRank = RankCards(allCards);

                    if (currentRank > highestRank)
                    {
                        highestRank = currentRank;
                        highestPlayer = player;
                        drawingPlayers.Clear();
                    }
                    else if (currentRank == highestRank)
                    {
                        if (!drawingPlayers.Contains(previousPlayer))
                            drawingPlayers.Add(previousPlayer);
                        if (!drawingPlayers.Contains(player))
                            drawingPlayers.Add(player);
                    }
                    previousPlayer = player;
                }

                SceneUtils.ShowPlayerCards(i++, player.name, ToString(currentRank), player.hand[0], player.hand[1], player.folded);
            }

            if(drawingPlayers.Count >= 2)
            {
                SceneUtils.DisplayWinners(drawingPlayers, highestRank);
                return drawingPlayers;
            }

            SceneUtils.DisplayWinner(highestPlayer, highestRank);
            return new List<ServerPlayer>() { highestPlayer };
        }

        /// <summary>
        /// Converts a hand's rank to text for use in debugging and TTS on the table
        /// </summary>
        /// <param name="highestRank">The hand to convert to text</param>
        /// <returns>A string containing the text version of the hand</returns>
        public static string ToString(Hand highestRank)
        {
            switch (highestRank)
            {
                case Hand.HIGH_CARD:
                    return "High Card";
                case Hand.PAIR:
                    return "Pair";
                case Hand.TWO_PAIRS:
                    return "Two Pair";
                case Hand.THREE_OF_A_KIND:
                    return "Three of a Kind";
                case Hand.FOUR_OF_A_KIND:
                    return "Four of a Kind";
                case Hand.STRAIGHT:
                    return "Straight";
                case Hand.FLUSH:
                    return "Flush";
                case Hand.FULL_HOUSE:
                    return "Full House";
                case Hand.STRAIGHT_FLUSH:
                    return "Straight Flush";
                case Hand.ROYAL_FLUSH:
                    return "Royal Flush";
                default:
                    return "Unknown Hand";
            }
        }

        /// <summary>
        /// Determines the rank of the hand by running through a set of rules for each possible rank, from the highest to lowest values
        /// </summary>
        /// <param name="cards">Cards to evaluate</param>
        /// <returns>The rank of the hand</returns>
        public static Hand RankCards(List<Card> cards)
        {
            bool flush          = isFlush(cards),
                 straight       = isStraight(cards),
                 straightFlush  = flush && straight,
                 royalFlush     = false;

            if (straightFlush)
                royalFlush = IsRoyalFlush(cards);

            var ofAKind = IsOfAKind(cards, false);
            var pairs = AnyPairs(cards);

            bool 
                four            = ofAKind   == 4, 
                three           = ofAKind   == 3, 
                twoPair         = pairs     == 2, 
                pair            = pairs     == 1, 
                fullHouse       = three && pair;

            if (royalFlush)
                return Hand.ROYAL_FLUSH;
            if (straightFlush)
                return Hand.STRAIGHT_FLUSH;
            if (four)
                return Hand.FOUR_OF_A_KIND;
            if (fullHouse)
                return Hand.FULL_HOUSE;
            if (flush)
                return Hand.FLUSH;
            if (straight)
                return Hand.STRAIGHT;
            if (three)
                return Hand.THREE_OF_A_KIND;
            if (twoPair)
                return Hand.TWO_PAIRS;
            if (pair)
                return Hand.PAIR;

            return Hand.HIGH_CARD;
        }
        
        /// <summary>
        /// Determines if the hand is a Royal Flush
        /// </summary>
        /// <param name="cards">Cards to evaluate</param>
        /// <returns>True/False of if the hand is a Royal Flush</returns>
        private static bool IsRoyalFlush(List<Card> cards)
        {
            uint royalty = 0;

            foreach (Card card in cards)
            {
                switch (card.rank)
                {
                    case Card.Rank.ACE:
                    case Card.Rank.TEN:
                    case Card.Rank.JACK:
                    case Card.Rank.QUEEN:
                    case Card.Rank.KING:
                        royalty++;
                        break;
                }
                
                if (royalty == 5)
                    return true;
            }
            
            return false;
        }

        /// <summary>
        /// Determines if the hand has cards "Of A Kind"
        /// </summary>
        /// <param name="cards">Cards to evaluate</param>
        /// <param name="returnHighCard">(Experimental) Makes the funciton return the highest card of the hand</param>
        /// <returns>Number "of a kind" cards (Or [unused/experimental] returns the highest card of the hand)</returns>
        private static int IsOfAKind(List<Card> cards, bool returnHighCard)
        {
            int highCard = 0,
                ofAKind = 0,
                i = 0;

            uint count = 0;

            cards.Sort((x, y) => x.rank.CompareTo(y.rank));

            do
            {
                if (cards[i].rank == cards[i + 1].rank)
                    count++;
                else
                    count = 0;

                if (count == 2)
                {
                    ofAKind = 3;
                    highCard = (int)cards[i + 1].rank;
                }

                if (count == 3)
                {
                    ofAKind = 4;
                    highCard = (int)cards[i + 1].rank;
                }
            }
            while (count < 3 && ++i < 6);

            if (returnHighCard)
                return highCard;
            else
                return ofAKind;
        }

        /// <summary>
        /// Determines if the hand contains a flush
        /// </summary>
        /// <param name="cards">Cards to evaluate</param>
        /// <returns>True/False of if the hand contains a flush</returns>
        private static bool isFlush(List<Card> cards)
        {
            uint count = 0;
            int i = 0;

            cards.Sort((x, y) => (x.suit.CompareTo(y.suit)));

            do
            {
                if (cards[i].suit == cards[i + 1].suit)
                    count++;
                else
                    count = 0;
            }
            while (count < 4 && ++i < 6);

            return (count == 5);
        }

        /// <summary>
        /// Determines if the hand contains a straight
        /// </summary>
        /// <param name="cards">Cards to evaluate</param>
        /// <returns>True/False of if the hand contains a straight</returns>
        private static bool isStraight(List<Card> cards)
        {
            uint count = 0;
            int i = 0;

            cards.Sort((x, y) => x.rank.CompareTo(y.rank));

            do
            {
                if (cards[i].rank == Card.Rank.ACE && cards[i + 1].rank == Card.Rank.TEN)
                    count++;
                else if ((int)cards[i].rank == (int)cards[i + 1].rank - 1)
                    count++;
                else
                    count = 0;

            }
            while (count < 5 && ++i < 6);

            return (count == 5);
        }

        /// <summary>
        /// Determines if the hand contains any pairs
        /// </summary>
        /// <param name="cards">Cards to evaluate</param>
        /// <returns>Number of pairs</returns>
        private static int AnyPairs(List<Card> cards)
        {
            cards.Sort((x, y) => x.rank.CompareTo(y.rank));
            
            int i = 0;
            int pairs = 0;

            do
                if (cards[i].rank == cards[i + 1].rank)
                    pairs++;
            while (pairs < 2 && ++i < 6);

            return pairs;
        }
    }
}