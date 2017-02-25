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
            Hand rank = Hand.HIGH_CARD;
            bool flush = false,  straight = false, straightFlush = false, royalFlush = false;
            bool four = false, three = false, two = false, pair = false, fullHouse = false;

            flush                           = isFlush(cards);
            straight                        = isStraight(cards);
            straightFlush                   = flush && straight;
            if (straightFlush) royalFlush   = isRoyalFlush(cards);

            four                            = isFour(cards);
            three                           = isThree(cards);
            two                             = isTwoPairs(cards);
            pair                            = isPair(cards);
            fullHouse                       = three && pair;

    	return rank;
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

    private static bool isFour(List<Card> cards){
      int[] values = new int[13];
      int temp = 0;
      for (int i = 0; i < 7; i++) values[(int)cards[i].rank]++;
      for (int i = 0; i < 13 ; i++) if(values[i] == 4) temp++;
      if (temp > 0) return true;
      else return false;
    }

    private static bool isFlush(List<Card> cards){
      int[] suits = new int[4];
      for (int i = 0; i < 7; i++) suits[(int)cards[i].suit]++;
      if (suits[0] >= 5 ||
          suits[1] >= 5 ||
          suits[2] >= 5 ||
          suits[3] >= 5) return true;
      else return false;
    }

    private static bool isStraight(List<Card> cards){
            //TODO: Sort by rank
            uint count = 0;
            int i = 0;
            while (count < 4 && i < 6)
            {
                if ((int)cards[i + 1].rank == (int)cards[i].rank + 1) count++;
                else count = 0;
                i++;
            }

            return (count == 4);
    }

    private static bool isThree(List<Card> cards){
      int[] values = new int[13];
      int temp = 0;
      for (int i = 0; i < 7; i++) values[(int)cards[i].rank]++;
      for (int i = 0; i < 13 ; i++) if(values[i] == 3) temp++;
      if (temp > 0) return true;
      else return false;
    }

    private static bool isTwoPairs(List<Card> cards){
      int[] values = new int[13];
      int temp = 0;
      for (int i = 0; i < 7; i++) values[(int)cards[i].rank]++;
      for (int i = 0; i < 13 ; i++) if(values[i] == 2) temp++;
      if (temp > 1) return true;
      else return false;
    }

    public static bool isPair(List<Card> cards){
      int[] values = new int[13];
      int temp = 0;
      for (int i = 0; i < 7; i++) values[(int)cards[i].rank]++;
      for (int i = 0; i < 13 ; i++) if(values[i] == 2) temp = 1;
      if (temp == 1) return true;
      else return false;
	}
  }
}