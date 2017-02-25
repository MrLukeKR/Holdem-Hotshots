using System;
using System.Collections.Generic;

namespace HoldemHotshots{
  static class CardRanker{
    public enum Hand{
      ROYAL_FLUSH = 10,
      STRAIGHT_FLUSH = 9,
      FOUR_OF_A_KIND = 8,
      FULL_HOUSE = 7,
      FLUSH = 6,
      STRAIGHT = 5,
      THREE_OF_A_KIND = 4,
      TWO_PAIRS = 3,
      PAIR = 2,
      HIGH_CARD = 1
    };

    public static ServerPlayer evaluateGame(Table table, List<ServerPlayer> players){
            Hand highestRank = 0, currentRank = 0;
            int draws = 0;
      ServerPlayer highestPlayer = null;
      List<Card> allCards = new List<Card>();
      ServerPlayer currentPlayer;
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].hasFolded())
                {
                    currentPlayer = players[i];
                    allCards.Clear();
                    allCards.AddRange(table.hand);
                    allCards.AddRange(currentPlayer.getCards());
                    currentRank = rankCards(allCards);
                    if (currentRank > highestRank)
                    {
                        highestRank = currentRank;
                        highestPlayer = currentPlayer;
                    }
                    else if (currentRank == highestRank) draws++;
                }
            }
      return highestPlayer;
    }
    
    public static Hand rankCards(List<Card> cards){
            bool flush = false,  straight = false, straightFlush = false, royalFlush = false;
            bool four = false, three = false, twoPair = false, pair = false, fullHouse = false;

            flush                           = isFlush(cards);
            straight                        = isStraight(cards);
            straightFlush                   = flush && straight;
            if (straightFlush) royalFlush   = isRoyalFlush(cards);

            var ofAKind                     = isOfAKind(cards);

            four                            = ofAKind == 4;
            three                           = ofAKind == 3;

            var pairs                       = anyPairs(cards);
            twoPair                         = pairs == 2;
            pair                            = pairs == 1;

            fullHouse                       = three && pair;

            if (royalFlush) { Console.WriteLine("Detected Royal Flush"); return Hand.ROYAL_FLUSH; }
            if (straightFlush) { Console.WriteLine("Detected Straight Flush"); return Hand.STRAIGHT_FLUSH; }
            if (four) { Console.WriteLine("Detected Four of a Kind"); return Hand.FOUR_OF_A_KIND; }
            if (fullHouse) { Console.WriteLine("Detected Full House"); return Hand.FULL_HOUSE; }
            if (flush) { Console.WriteLine("Detected Flush"); return Hand.FLUSH; }
            if (straight) { Console.WriteLine("Detected Straight"); return Hand.STRAIGHT; }
            if (three) { Console.WriteLine("Detected Three of a Kind"); return Hand.THREE_OF_A_KIND; }
            if (twoPair) { Console.WriteLine("Detected Two Pair"); return Hand.TWO_PAIRS; }
            if (pair) { Console.WriteLine("Detected Pair"); return Hand.PAIR; }

            Console.WriteLine("Detected High Card");

            return Hand.HIGH_CARD;
    }

    private static bool isRoyalFlush(List<Card> cards){
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

                    if (royalty == 5) return true;
                }
            return false;
    }

    private static int isOfAKind(List<Card> cards){
            cards.Sort((x, y) => x.rank.CompareTo(y.rank));

            uint count = 0;
            int ofAKind = 0;
            int i = 0;

            while (count < 3 && i < 6)
            {
                if (cards[i].rank == cards[i + 1].rank) count++;
                else count = 0;

                if (count == 2) ofAKind = 3;
                if (count == 3) ofAKind = 4;
                i++;
            }
            return ofAKind;
        }

    private static bool isFlush(List<Card> cards){
            cards.Sort((x, y) => (x.suit.CompareTo(y.suit)));

            uint count = 0;
            int i = 0;

            while (count < 4 && i < 6)
            {
                if (cards[i].suit == cards[i + 1].suit) count++;
                else count = 0;
                i++;
            }

            return (count == 5);
        }

    private static bool isStraight(List<Card> cards){
            cards.Sort((x, y) => x.rank.CompareTo(y.rank));

            uint count = 0;
            int i = 0;

            while (count < 5 && i < 6)
            {
                if (cards[i].rank == Card.Rank.ACE && cards[i + 1].rank == Card.Rank.TEN) count++;
                else if ((int)cards[i].rank == (int)cards[i + 1].rank - 1) count++;
                else count = 0;
                i++;
            }

            return (count == 5);
    }
        
    private static int anyPairs(List<Card> cards){
            cards.Sort((x, y) => x.rank.CompareTo(y.rank));
            
            int i = 0;
            int pairs = 0;

            while (pairs < 2 && i < 6)
            {
                if (cards[i].rank == cards[i + 1].rank) pairs++;
                i++;
            }

            return pairs;
        }
  }
}