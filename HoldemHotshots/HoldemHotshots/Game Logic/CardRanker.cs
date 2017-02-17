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
      int highestRank, currentRank = 0;
      ServerPlayer highestPlayer = null;
      List<Card> allCards = new List<Card>();
      ServerPlayer currentPlayer;
      for (int i = 0; i < players.Count; i++){
        currentPlayer = players[i];
        allCards.Clear();
        allCards.AddRange(table.hand);
        allCards.AddRange(currentPlayer.getCards());
        currentRank = rankCards(allCards);
        if (currentRank > highestRank){
          highestRank = currentRank;
          highestPlayer = currentPlayer;
        }
      }
      return highestPlayer;
    }

public static Hand rankCards(List<Card> cards){
    Hand rank = Hand.HIGH_CARD;
    int rank_number = 0, rest_card1 = 0, rest_card2 = 0, rest_card3 = 0;
    int[] values = new int[13];
    int[] suits = new int[4];
    //COUNTING STAGE
    for(int i = 0; i < 7; i++) values[(int)cards[i].rank]++;
    for(int i = 0; i < 7; i++) suits[(int)cards[i].rank]++;
    //ANALYSIS STAGE
    if (isRoyalFlush(cards)){
        rank = Hand.ROYAL_FLUSH;
        //
    } else if (isStraightFlush(cards)){
        rank = Hand.STRAIGHT_FLUSH;
        int[] value = new int[7];
        int[] suit = new int[7];
        int[] temp_array = new int[13];
        int suit_straight = 0;
        for (int i = 0; i < 7; i++) value[i] = (int)cards[i].rank;
        for (int i = 0; i < 7; i++) suit[i] = (int)cards[i].suit;
        for (int i = 0; i < 4; i++) if (suits[i] >= 5) suit_straight = i;
        for (int i = 0; i < 7; i++)
        if (suits[i] == suit_straight) temp_array[value[i]]++;
        for (int i = 0; i < 8; i++)
        if (temp_array[i] > 0 &&
        temp_array[i + 1] > 0 &&
        temp_array[i + 2] > 0 &&
        temp_array[i + 3] > 0 &&
        temp_array[i + 4] > 0) rank_number = i + 4;
        //
    } else if (isFour(cards)) {
        rank = Hand.FOUR_OF_A_KIND;
        int highest1 = 0, highest2 = 0, temp = 0;
        for (int i = 0; i < 13; i++){
            if (values[i] == 4) rank_number = i;
            highest1 = i;
        }
        for (int i = 0; i < 7; i++){
            temp = values[(int)cards[i].rank];
            if (temp > highest1 && temp != rank_number) { highest2 = temp; }
        }
        rest_card1 = highest2;
        //
    } else if (isFullHouse(cards)) {
        rank = Hand.FULL_HOUSE;
        for (int i = 0; i < 13; i++){
            if (values[i] == 3) rank_number = i;
            if (values[i] == 2) rest_card1 = i;
        }
        //
    } else if (isFlush(cards)){
        rank = Hand.FLUSH;
        int suit = 0;
        int[,] card = new int[7, 2];
        for (int i = 0; i < 7; i++) card[i, 0] = (int)cards[i].rank;
        for (int i = 0; i < 7; i++) card[i, 1] = (int)cards[i].suit;
        for (int i = 0; i < 7; i++) if (suits[i] >= 5) suit = i;
        for (int i = 0; i < 7; i++) {
            if (card[i, 1] == suit) rank_number = card[i, 1];
        }
        for (int i = 0; i < 7; i++) {
            if (card[i, 1] == suit && card[i, 1] != rank_number)
            rest_card1 = card[i, 1];
        }
        for (int i = 0; i < 7; i++){
            if (card[i, 1] == suit &&
            card[i, 1] != rank_number &&
            card[i, 1] != rest_card1) rest_card2 = card[i, 1];
        }
        for (int i = 0; i < 7; i++){
            if (card[i, 1] == suit &&
            card[i, 1] != rank_number &&
            card[i, 1] != rest_card1 &&
            card[i, 1] != rest_card2) rest_card3 = card[i, 1];
        }
        for (int i = 0; i < 7; i++) {
            if (card[i, 1] == suit &&
            card[i, 1] != rank_number &&
            card[i, 1] != rest_card1 &&
            card[i, 1] != rest_card2 &&
            card[i, 1] != rest_card3) rest_card3 = card[i, 1];
        }
        //
    } else if (isStraight(cards)){
        rank = Hand.STRAIGHT;
        if (values[0] > 0 && values[1] > 0 &&
        values[2] > 0 && values[3] > 0 &&
        values[12] > 0){
            rank_number = 3;
        } else {
            for (int i = 0; i < 8; i++) {
                if (values[i] > 0 &&
                values[i + 1] > 0 &&
                values[i + 2] > 0 &&
                values[i + 3] > 0 &&
                values[i + 4] > 0) rank_number = i + 4;
            }
        }
        //
    } else if (isThree(cards)){
        rank = Hand.THREE_OF_A_KIND;
        for (int i = 0; i < 13; i++) if (values[i] == 3) rank_number = i;
    }
    return rank;
}

    private static bool isRoyalFlush(List<Card> cards){
      int[] suits = new int[4];
      int[,] card = new int[7, 2];
      int suit = 0, temp = 0;
      for (int i = 0; i < 7; i++) suits[(int)cards[i].suit]++;
      for (int i = 0; i < 7; i++) card[i,0] = (int)cards[i].rank;
      for (int i = 0; i < 7; i++) card[i,1] = (int)cards[i].suit;
      for (int i = 0; i < 4; i++) if (suits[i] >= 5) suit = i;
      for (int i = 8; i < 13; i++){
        for (int j = 0; j < 7; j++){
          if (card[j, 0] == i && card[j, 1] == suit) temp++;
        }
      }
      if (temp == 5) return true;
      else return false;
    }

    private static bool isStraightFlush(List<Card> cards){
			int[] suits = new int[4];
			int[,] card = new int[7, 2];
			int suit = 0, temp1 = 0, temp2 = 0;
			for (int i = 0; i < 7; i++) suits[(int)cards[i].suit]++;
			for (int i = 0; i < 7; i++) card[i, 0] = (int)cards[i].rank;
			for (int i = 0; i < 7; i++) card[i, 1] = (int)cards[i].suit;
			for (int i = 0; i < 4; i++) if (suits[i] >= 5) suit = i;
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 7; j++)
				{
					int temp3 = 0;
					if (card[j, 0] == i && card[j, 1] == suit) temp3++;
					if (card[j, 0] == i+1 && card[j, 1] == suit) temp3++;
					if (card[j, 0] == i+2 && card[j, 1] == suit) temp3++;
					if (card[j, 0] == i+3 && card[j, 1] == suit) temp3++;
					if (card[j, 0] == i+4 && card[j, 1] == suit) temp3++;
					temp1 = temp3;
				}
				if (temp1 == 5) temp2++;
			}

			for (int j = 0; j < 7; j++)
			{
				int temp4 = 0;
				if (card[j, 0] == 0 && card[j, 1] == suit) temp4++;
				if (card[j, 0] == 1 && card[j, 1] == suit) temp4++;
				if (card[j, 0] == 2 && card[j, 1] == suit) temp4++;
				if (card[j, 0] == 3 && card[j, 1] == suit) temp4++;
				if (card[j, 0] == 12 && card[j, 1] == suit) temp4++;
				temp1 = temp4;
			}
			if (temp1 == 5) temp2++;

			if (temp2 > 0) return true;
			else return false;
    }

    private static bool isFour(List<Card> cards){
      int[] values = new int[13];
      int temp = 0;
      for (int i = 0; i < 7; i++) values[(int)cards[i].rank]++;
      for (int i = 0; i < 13 ; i++) if(values[i] == 4) temp++;
      if (temp > 0) return true;
      else return false;
    }

    private static bool isFullHouse(List<Card> cards){
      int[] values = new int[13];
      int[] suits = new int[4];
      int temp1 = 0,temp2 = 0;
      for (int i = 0; i < 7; i++) values[(int)cards[i].rank]++;
      for (int i = 0; i < 7; i++) suits[(int)cards[i].suit]++;
      for (int i = 0; i < 13 ; i++){
          if (values[i] == 3) temp1++;
          if (values[i]== 2) temp2++;
      }
      if (temp1 > 0 && temp2 > 0) return true;
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
      int[] values = new int[13];
      int[] suits = new int[4];
      int temp = 0;
      for (int i = 0; i < 7; i++) values[(int)cards[i].rank]++;
      for (int i = 0; i < 7; i++) suits[(int)cards[i].suit]++;
      for (int i = 0; i < 9; i++){
        if (values[i] > 0  &&
           values[i+1] > 0 &&
           values[i+2] > 0 &&
           values[i+3] > 0 &&
           values[i+4] > 0) temp++;
      }
      if (values[0] > 0 &&
          values[1] > 0 &&
          values[2] > 0 &&
          values[3] > 0 &&
          values[12] > 0) temp++;
      if (temp > 0) return true;
      else return false;
    }

    private static bool isThree(List<Card> cards){
      int[] values = new int[13];
      int[] suits = new int[4];
      int temp = 0;
      for (int i = 0; i < 7; i++) values[(int)cards[i].rank]++;
      for (int i = 0; i < 7; i++) suits[(int)cards[i].suit]++;
      for (int i = 0; i < 13 ; i++) if(values[i] == 3) temp++;
      if (temp > 0) return true;
      else return false;
    }

    private static bool isTwoPairs(List<Card> cards){
      int[] values = new int[13];
      int[] suits = new int[4];
      int temp = 0;
      for (int i = 0; i < 7; i++) values[(int)cards[i].rank]++;
      for (int i = 0; i < 7; i++) suits[(int)cards[i].suit]++;
      for (int i = 0; i < 13 ; i++) if(values[i] == 2) temp++;
      if (temp > 1) return true;
      else return false;
    }

    public static bool isPair(List<Card> cards){
      int[] values = new int[13];
      int[] suits = new int[4];
      int temp = 0;
      for (int i = 0; i < 7; i++) values[(int)cards[i].rank]++;
      for (int i = 0; i < 7; i++) suits[(int)cards[i].suit]++;
      for (int i = 0; i < 13 ; i++) if(values[i] == 2) temp = 1;
      if (temp == 1) return true;
      else return false;
    }
  }
}
