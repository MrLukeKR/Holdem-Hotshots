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
}
