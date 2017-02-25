using System.Collections.Generic;

namespace HoldemHotshots{
  static class CardRanker{
    public enum Hand{
      ROYAL_FLUSH = 10,
      STRAIGHT_FLUSH = 9,
      FOUR_OF_A_KIND = 8,
      FULL_HOUSE = 7,
      FLUSH = 5,
      STRAIGHT = 4,
      THREE_OF_A_KIND = 3,
      TWO_PAIRS = 2,
      PAIR = 1,
      HIGH_CARD = 0
    };

    public static ServerPlayer evaluateGame(Table table, List<ServerPlayer> players){
      int highestRank = 0, currentRank = 0;
      ServerPlayer highestPlayer = null;
      List<Card> allCards = new List<Card>();
      ServerPlayer currentPlayer;
      for (int i = 0; i < players.Count; i++){
        currentPlayer = players[i];
        allCards.Clear();
        allCards.AddRange(table.hand);
        allCards.AddRange(currentPlayer.getCards());
       // currentRank = rankCards(allCards); //TODO: Fix these
        if (currentRank > highestRank){
          highestRank = currentRank;
          highestPlayer = currentPlayer;
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

            if (royalFlush)     return Hand.ROYAL_FLUSH;
            if (straightFlush)  return Hand.STRAIGHT_FLUSH;
            if (four)           return Hand.FOUR_OF_A_KIND;
            if (fullHouse)      return Hand.FULL_HOUSE;
            if (flush)          return Hand.FLUSH;
            if (straight)       return Hand.STRAIGHT;
            if (three)          return Hand.THREE_OF_A_KIND;
            if (twoPair)        return Hand.TWO_PAIRS;
            if (pair)           return Hand.PAIR;

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
            cards.Sort((x, y) => (int)x.rank.CompareTo((int)y.rank));

            uint count = 0;
            int ofAKind = 0;
            int i = 0;

            while (count < 4 && i < 6)
            {
                if ((int)cards[i].rank == (int)cards[i + 1].rank) count++;
                else count = 0;
                i++;

                if (count == 3) ofAKind = 3;
                if (count == 4) ofAKind = 4;
            }
            return ofAKind;
        }

    private static bool isFlush(List<Card> cards){
            cards.Sort((x, y) => (int)x.suit.CompareTo((int)y.suit));

            uint count = 0;
            int i = 0;

            while (count < 5 && i < 6)
            {
                if ((int)cards[i].suit == (int)cards[i + 1].suit) count++;
                else count = 0;
                i++;
            }

            return (count == 5);
        }

    private static bool isStraight(List<Card> cards){
            cards.Sort((x, y) => (int)x.rank.CompareTo((int)y.rank));

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
            cards.Sort((x, y) => (int)x.rank.CompareTo((int)y.rank));

            uint count = 0;
            int i = 0;
            int pairs = 0;

            while (count < 4 && i < 6)
            {
                if ((int)cards[i].rank == (int)cards[i + 1].rank) count++;
                else count = 0;
                i++;

                if (count == 2)
                {
                    count = 0;
                    pairs++;
                }
            }

            return pairs;
        }
  }
}