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
    	int rank_number = 0, rest_card1 = 0, rest_card2 = 0, rest_card3 = 0, rest_card4 = 0, rest_card5 =0, rest_card6 = 0;
    	int[] values = new int[13];
    	int[] suits = new int[4];
   	 	//COUNTING STAGE
    	for(int i = 0; i < 7; i++) values[(int)cards[i].rank]++;
    	for(int i = 0; i < 7; i++) suits[(int)cards[i].rank]++;
			//ANALYSIS STAGE
			if (isRoyalFlush(cards))
			{
				rank = Hand.ROYAL_FLUSH;
			}
			else if (isStraightFlush(cards))
			{
				rank = Hand.STRAIGHT_FLUSH;
			}
			else if (isFour(cards))
			{
				rank = Hand.FOUR_OF_A_KIND;
			}
			else if (isFullHouse(cards))
			{
				rank = Hand.FULL_HOUSE;
				int[] value7 = new int[7];
				int[] fh1 = new int[2];
				int[] fh2 = new int[2];
				int tempfh = 0, tempfh1 = 0, tempfh2 = 0;
				for (int i = 0; i < 13; i++) {
					if (values[i] == 3) tempfh++; fh1[tempfh-1] = values[i];
					if (values[i] == 2) tempfh1++; fh2[tempfh1-1] = values[i];
				}
				if (tempfh == 2) {
					if (fh1[0] > fh1[1]) { rank_number = fh1[0]; rest_card1 = fh1[1]; }
					else { rank_number = fh1[1]; rest_card1 = fh1[0]; }
					for (int i = 0; i < 7; i++) {
						value7[i] = (int)cards[i].rank;
						if (value7[i] != fh1[0] && value7[i] != fh1[1]) tempfh2 = value7[i];
					}
					if (tempfh2 > rest_card1) { rest_card2 = tempfh2; rest_card3 = rest_card1; }
					else {rest_card2 = rest_card1; rest_card2 = tempfh2; }
				}
				else {
					rank_number = fh1[0];
					if (tempfh1 == 2) {
						if (fh2[0] > fh2[1]) { rest_card1 = fh2[0]; rest_card2 = fh2[1]; rest_card3 = fh2[1]; }
						else { rest_card1 = fh2[1]; rest_card2 = fh2[0]; rest_card3 = fh2[0]; }
					}
					else {
						rest_card1 = fh2[0];
						for (int i = 0; i < 7; i++) {
							value7[i] = (int)cards[i].rank;
							if (value7[i] >= rest_card2 && value7[i] != rank_number && value7[i] != rest_card1) rest_card2 = value7[i];
						}
						for (int i = 0; i < 7; i++) {
							value7[i] = (int)cards[i].rank;
							if (value7[i] >= rest_card3 && value7[i] != rank_number && value7[i] != rest_card1 && value7[i] != rest_card2) rest_card3 = value7[i];
						}
					}
				}
			}
			else if (isFlush(cards))
			{
				rank = Hand.FLUSH;
			}
			else if (isStraight(cards))
			{
				rank = Hand.STRAIGHT;
				if (values[0] > 0 && values[1] > 0 &&
				values[2] > 0 && values[3] > 0 &&
				values[12] > 0)
				{
					rank_number = 3;
				}
				else {
					for (int i = 0; i < 8; i++)
					{
						if (values[i] > 0 &&
						values[i + 1] > 0 &&
						values[i + 2] > 0 &&
						values[i + 3] > 0 &&
						values[i + 4] > 0) rank_number = i + 4;
					}
				}
				//
			}
			else if (isThree(cards)) {
				rank = Hand.THREE_OF_A_KIND;
				int[] value4 = new int[7];
				int tempthree = 0;
				for (int i = 0; i < 13; i++) if (value4[i] == 3) rank_number = i;
				for (int i = 0; i < 7; i++) {
					value4[i] = (int)cards[i].rank;
					tempthree = value4[i];
					if (tempthree >= rest_card1 && tempthree != rank_number) rest_card1 = tempthree; }
				for (int i = 0; i < 7; i++) {
					value4[i] = (int)cards[i].rank;
					tempthree = value4[i];
					if (tempthree >= rest_card2 && tempthree != rank_number && tempthree != rest_card1) rest_card2 = tempthree; }
				for (int i = 0; i < 7; i++) {
					value4[i] = (int)cards[i].rank;
					tempthree = value4[i];
					if (tempthree >= rest_card3 && tempthree != rank_number && tempthree != rest_card1 && tempthree != rest_card2) rest_card3 = tempthree; }
				for (int i = 0; i < 7; i++) {
					value4[i] = (int)cards[i].rank;
					tempthree = value4[i];
					if (tempthree >= rest_card4 && tempthree != rank_number && tempthree != rest_card1 && tempthree != rest_card2 && tempthree != rest_card3) rest_card4 = tempthree; }
			}
			else if (isTwoPairs(cards)) {
				rank = Hand.TWO_PAIRS;
				int[] twopair = new int[3];
				int[] value3 = new int[7];
				int temppair = 0, temppair1 = 0, temppair2 = 0, temppair3 = 0, temppair4 = 0, temppair5 = 0, temppair6 = 0;
				for (int i = 0; i < 13; i++) {
					if (values[i] == 2) temppair++;
					twopair[temppair-1] = values[i];
				}
				if(temppair == 2) {
					if (twopair[0] > twopair[1]) { rank_number = twopair[0]; rest_card1 = twopair[1]; }
					else { rank_number = twopair[1]; rest_card1 = twopair[0]; }
					for (int i = 0; i < 7; i++) { value3[i] = (int)cards[i].rank; temppair3 = value3[i];
						if (temppair3 >= rest_card2 && temppair3 != rank_number && temppair3 != rest_card1) rest_card2 = temppair3; }
					for (int i = 0; i < 7; i++) { value3[i] = (int)cards[i].rank; temppair3 = value3[i];
						if (temppair3 >= rest_card3 && temppair3 != rank_number && temppair3 != rest_card1 && temppair3 != rest_card2) rest_card3 = temppair3; }
					for (int i = 0; i < 7; i++) { value3[i] = (int)cards[i].rank; temppair3 = value3[i];
						if (temppair3 >= rest_card4 && temppair3 != rank_number && temppair3 != rest_card1 && temppair3 != rest_card2 && temppair3 != rest_card3) rest_card4 = temppair3; }
				} else {
					for (int i = 0; i < 3; i++) {
						if (twopair[i] >= temppair1) temppair1 = twopair[i];
					}
					for (int i = 0; i < 3; i++) {
						if (twopair[i] >= temppair2 && twopair[i] != temppair1) temppair2 = twopair[i]; 
					}
					for (int i = 0; i < 3; i++) {
						if (twopair[i] != temppair1 && twopair[i] != temppair2) temppair6 = twopair[i]; 
					}
					rank_number = temppair1; rest_card1 = temppair2;
					for (int i = 0; i < 7; i++) { value3[i] = (int)cards[i].rank; temppair4 = value3[i];
						if (temppair4 != twopair[0] && temppair4 != twopair[1] && temppair4 != twopair[2]) temppair5 = temppair4;
					}
					if (temppair5 > temppair6) { rest_card2 = temppair5; rest_card3 = temppair6; rest_card4 = temppair6;}
					else { rest_card2 = temppair6; rest_card3 = temppair6; rest_card4 = temppair5;}
				}
			}
			else if (isPair(cards)) {
				rank = Hand.PAIR;
				int[] value2 = new int[7];
				int temppair = 0;
				for (int i = 0; i < 13; i++) if (value2[i] == 2) rank_number = i;
				for (int i = 0; i < 7; i++) { value2[i] = (int)cards[i].rank; temppair = value2[i];
					if (temppair >= rest_card1 && temppair != rank_number) rest_card1 = temppair; }
				for (int i = 0; i < 7; i++) { value2[i] = (int)cards[i].rank; temppair = value2[i];
					if (temppair >= rest_card2 && temppair != rank_number && temppair != rest_card1) rest_card2 = temppair; }
				for (int i = 0; i < 7; i++) { value2[i] = (int)cards[i].rank; temppair = value2[i];
					if (temppair >= rest_card3 && temppair != rank_number && temppair != rest_card1 && temppair != rest_card2) rest_card3 = temppair; }
				for (int i = 0; i < 7; i++) { value2[i] = (int)cards[i].rank; temppair = value2[i];
					if (temppair >= rest_card4 && temppair != rank_number && temppair != rest_card1 && temppair != rest_card2 && temppair != rest_card3) rest_card4 = temppair; }
				for (int i = 0; i < 7; i++) { value2[i] = (int)cards[i].rank; temppair = value2[i];
					if (temppair >= rest_card5 && temppair != rank_number && temppair != rest_card1 && temppair != rest_card2 && temppair != rest_card3 && temppair != rest_card4) rest_card5 = temppair; }
			}
			else {
				rank = Hand.HIGH_CARD;
				int[] value1 = new int[7];
				int temphigh = 0;
				for (int i = 0; i < 7; i++) { value1[i] = (int)cards[i].rank; temphigh = value1[i];
					if (temphigh >= rest_card1 && temphigh != rank_number) rest_card1 = temphigh; }
				for (int i = 0; i < 7; i++) { value1[i] = (int)cards[i].rank; temphigh = value1[i];
					if (temphigh >= rest_card2 && temphigh != rank_number && temphigh != rest_card1) rest_card2 = temphigh; }
				for (int i = 0; i < 7; i++) { value1[i] = (int)cards[i].rank; temphigh = value1[i];
					if (temphigh >= rest_card3 && temphigh != rank_number && temphigh != rest_card1 && temphigh != rest_card2) rest_card3 = temphigh; }
				for (int i = 0; i < 7; i++) { value1[i] = (int)cards[i].rank; temphigh = value1[i];
					if (temphigh >= rest_card4 && temphigh != rank_number && temphigh != rest_card1 && temphigh != rest_card2 && temphigh != rest_card3) rest_card4 = temphigh; }
				for (int i = 0; i < 7; i++) { value1[i] = (int)cards[i].rank; temphigh = value1[i];
					if (temphigh >= rest_card5 && temphigh != rank_number && temphigh != rest_card1 && temphigh != rest_card2 && temphigh != rest_card3 && temphigh != rest_card4) rest_card5 = temphigh; }
				for (int i = 0; i < 7; i++) { value1[i] = (int)cards[i].rank; temphigh = value1[i];
					if (temphigh >= rest_card6 && temphigh != rank_number && temphigh != rest_card1 && temphigh != rest_card2 && temphigh != rest_card3 && temphigh != rest_card4 && temphigh != rest_card5) rest_card6 = temphigh; }
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
      int temp1 = 0,temp2 = 0;
      for (int i = 0; i < 7; i++) values[(int)cards[i].rank]++;
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
      int temp = 0;
      for (int i = 0; i < 7; i++) values[(int)cards[i].rank]++;
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