using HoldemHotshots.GameLogic.Player;
using System.Collections.Generic;

namespace HoldemHotshots.GameLogic
{
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
        
        public static List<ServerPlayer> evaluateGame(Table table, List<ServerPlayer> players)
        {
            Hand highestRank = 0, currentRank = 0;
            List<ServerPlayer> drawingPlayers = new List<ServerPlayer>();
            ServerPlayer highestPlayer = null;
            List<Card> allCards = new List<Card>();

            foreach(ServerPlayer player in players)
            {
                if (!player.folded)
                {
                    allCards.Clear();
                    allCards.AddRange(table.hand);
                    allCards.AddRange(player.hand);
                    currentRank = rankCards(allCards);

                    if (currentRank > highestRank)
                    {
                        highestRank = currentRank;
                        highestPlayer = player;
                        drawingPlayers.Clear();
                    }
                    else if (currentRank == highestRank)
                        drawingPlayers.Add(player);
                }
            }

            if(drawingPlayers.Count > 0)
            {
                switch (highestRank)
                {
                    case Hand.ROYAL_FLUSH:
                        //TODO: Share pot
                        break;
                    case Hand.STRAIGHT_FLUSH:
                        //TODO: Highest card
                        break;
                    case Hand.FULL_HOUSE:
                        //TODO: Hightest card
                        break;
                    case Hand.FLUSH:
                        //TODO: Highest card
                        break;
                    case Hand.STRAIGHT:
                        //TODO: Highest card
                        break;
                    case Hand.THREE_OF_A_KIND:
                    case Hand.FOUR_OF_A_KIND:
                        var highest = 0;
                        var current = 0;

                        foreach (ServerPlayer player in drawingPlayers)
                        {
                            current = IsOfAKind(player.hand, true);
                            if (current > highest)
                            {
                                highest = current;
                                highestPlayer = player;
                            }
                        }
                        break;
                    case Hand.TWO_PAIRS:
                        //TODO: Highest card
                        break;
                    case Hand.PAIR:
                        //TODO: Highest card
                        break;
                    case Hand.HIGH_CARD:
                        //TODO: Highest card
                        break;
                }
            }

            return new List<ServerPlayer>() { highestPlayer };
        }
        
        public static Hand rankCards(List<Card> cards)
        {
            bool flush          = isFlush(cards),
                 straight       = isStraight(cards),
                 straightFlush  = flush && straight,
                 royalFlush     = false;

            if (straightFlush)
                royalFlush = IsRoyalFlush(cards);

            var ofAKind = IsOfAKind(cards, false);
            var pairs = anyPairs(cards);

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

        private static int anyPairs(List<Card> cards)
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