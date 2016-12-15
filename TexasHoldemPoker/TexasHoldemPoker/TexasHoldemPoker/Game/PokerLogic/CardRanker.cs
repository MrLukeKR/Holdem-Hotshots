using PokerLogic;
using System.Collections.Generic;
using System;

namespace PokerLogic
{
    static class CardRanker
    {
        public enum Hand
        {
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

        public static Player evaluateGame(Table table, List<Player> players)
        {
            Hand highestRank = Hand.HIGH_CARD, currentRank;
            Player highestPlayer = null;
            List<Card> allCards = new List<Card>();

            Player currentPlayer;

            for (int i = 0; i < players.Count; i++)
            {
                currentPlayer = players[i];

                allCards.Clear();
                allCards.AddRange(table.getCards());
                allCards.AddRange(currentPlayer.getCards());

                currentRank = rankCards(allCards);
                if (currentRank > highestRank)
                {
                    highestRank = currentRank;
                    highestPlayer = currentPlayer;
                }
            }

            return highestPlayer;
        }

        public static Hand rankCards(List<Card> cards)
        {
            Hand rank = Hand.HIGH_CARD;
            int rank_number = 0, rest_card1 = 0, rest_card2 = 0, rest_card3 = 0, rest_card4 = 0;
            int[] values = new int[13];
            int[] suits = new int[4];


            //ANALYSIS STAGE

            if (isRoyalFlush(cards))
            {
                rank = Hand.ROYAL_FLUSH;
            }
            else if (isStraightFlush(cards))
            {
                rank = Hand.STRAIGHT_FLUSH;
                int[] value = new int[7];
                int[] suit = new int[7];
                int[] temp_array = new int[13];
                int suit_straight = 0;

                for (int i = 0; i < 7; i++)
                    value[i] = cards[i].getValue();
                for (int i = 0; i < 7; i++)
                    suit[i] = cards[i].getSuit();

                for (int i = 0; i < 4; i++)
                    if (suits[i] >= 5)
                        suit_straight = i;

                for (int i = 0; i < 7; i++)
                    if (suits[i] == suit_straight)
                        temp_array[value[i]]++;

                for (int i = 0; i < 8; i++)
                    if (temp_array[i] > 0 && temp_array[i + 1] > 0 && temp_array[i + 2] > 0 && temp_array[i + 3] > 0 && temp_array[i + 4] > 0)
                        rank_number = i + 4;

            } else if (isFour(cards)) {
                rank = Hand.FOUR_OF_A_KIND;
                int highest1 = 0, highest2 = 0, temp = 0;
                for (int i = 0; i < 13; i++) {
                    if (values[i] == 4)
                        rank_number = i;
                    highest1 = i;
                }
                for (int i = 0; i < 7; i++) {
                    temp = values[cards[i].getValue()];
                    if (temp > highest1 && temp != rank_number) {
                        highest2 = temp; }
                }
                rest_card1 = highest2;
            }
            else if(isFullHouse(cards)){
			rank = Hand.FULL_HOUSE;
			for(int i = 0; i < 13 ; i++){
				if(values[i] == 3)
					rank_number = i;
				if(values[i]== 2)
					rest_card1 = i;
			}
		}
		else if(isFlush(cards)){
			rank = Hand.FLUSH;
			int suit = 0;
			int[,] card = new int[7,2];
			
			for(int i = 0; i < 7; i++)
				card[i,0] = cards[i].getValue();
			for(int i = 0; i < 7; i++)
				card[i,1] = cards[i].getSuit();
			
			for(int i = 0; i < 7; i++) {
				if(suits[i] >= 5) {
					suit = i;
				}
			}
				for(int i = 0; i < 7; i++) {
				if(card[i,1] == suit){
					rank_number = card[i,1];
				}
			}
			for(int i = 0; i < 7; i++) {
				if(card[i,1] == suit && card[i,1] != rank_number){
					rest_card1 = card[i,1];
				}
			}
			for(int i = 0; i < 7; i++) {
				if(card[i,1] == suit && card[i,1] != rank_number && card[i,1] != rest_card1){
					rest_card2 = card[i,1];
				}
			}
			for(int i = 0; i < 7; i++) {
				if(card[i,1] == suit && card[i,1] != rank_number && card[i,1] != rest_card1 && card[i,1] != rest_card2){
					rest_card3 = card[i,1];
				}
			}
			for(int i = 0; i < 7; i++) {
				if(card[i,1] == suit && card[i,1] != rank_number && card[i,1] != rest_card1 && card[i,1] != rest_card2 && card[i,1] != rest_card3){
					rest_card3 = card[i,1];
				}
			}
		}

            return rank;
        }

        private static bool isFour(List<Card> cards)
        {
            throw new NotImplementedException();
        }

        private static bool isFlush(List<Card> cards)
        {
            throw new NotImplementedException();
        }

        private static bool isFullHouse(List<Card> cards)
        {
            throw new NotImplementedException();
        }

        private static bool isStraightFlush(List<Card> cards)
        {
            throw new NotImplementedException();
        }

        private static bool isRoyalFlush(List<Card> cards)
        {
            throw new NotImplementedException();
        }
    }
}
