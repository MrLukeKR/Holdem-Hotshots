using PokerLogic;
﻿import java.util.ArrayList

namespace TexasHoldemPoker.Game.PokerObjects
{
    static class CardRanker
    {
        public static enum Hand{
		ROYAL_FLUSH,
		STRAIGHT_FLUSH,
		FOUR_OF_A_KIND,
		FULL_HOUSE,
		FLUSH,
		STRAIGHT,
		THREE_OF_A_KIND,
		TWO_PAIRS,
		PAIR,
		HIGH_CARD
	};							//TODO: Consider using this enum instead of the list of constants
	
	public static final int ROYAL_FLUSH 	 = 10;
	public static final int STRAIGHT_FLUSH 	 = 9;
	public static final int FOUR_OF_A_KIND 	 = 8;
	public static final int FULL_HOUSE       = 7;
	public static final int FLUSH            = 6;
	public static final int STRAIGHT         = 5;
	public static final int THREE_OF_A_KIND  = 4;
	public static final int TWO_PAIR         = 3;
	public static final int PAIR             = 2;
	public static final int HIGH_CARD        = 1;
	

    }
    public static Player evaluateGame(Table table, ArrayList<Player> players){
		int highestRank = 0, currentRank = 0;
		Player highestPlayer = players.get(0);
		ArrayList<Card> allCards = new ArrayList<Card>();
		
		for(Player currentPlayer : players){
			allCards.clear();
			allCards.addAll(table.getCards());
			allCards.addAll(currentPlayer.getCards());
			
			currentRank = rankCards(allCards);
			if(currentRank > highestRank){
				PrintUtils.cardRankPrint(currentRank + " is higher than " + highestRank);
				highestRank = currentRank;
				highestPlayer = currentPlayer;
			}
		}
				
		return highestPlayer;
	}
	
	public static int rankCards(ArrayList<Card> cards){
		int rank = 0, rank_number = 0, rest_card1 = 0, rest_card2 = 0, rest_card3 = 0, rest_card4 = 0;
		int[] values = new int[13];
		int[] suits = new int[4];
		
}

		/** Collections.sort(cards, new Comparator<Card>(){

			@Override
			public int compare(Card o1, Card o2) {
				
				if(o1.getValue() < o2.getValue())
					return -1;
				else if (o1.getValue() > o2.getValue())
					return 1;
				else
					return 0;
			}
			
		});
		
		PrintUtils.cardRankPrint(cards.toString()); */
		
