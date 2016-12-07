using System;
using System.Collections.Generic;

namespace PokerLogic
{
    class Deck
    {
        List<Card> deck = new List<Card>();

        public Deck()
        {
            init();
            shuffle();
        }

        private void init()
        {
            for (int s = 0; s < 4; s++)
                for (int r = 1; r <= 13; r++)
                    deck.Add(new Card((Card.Suit)s, (Card.Rank)r));
        }

        public void shuffle()
        {
            
        }

        public Card takeCard()
        {
            Card card = deck[0];
            deck.RemoveAt(0);
            return card;
        }

        public override String ToString()
        {
            String sDeck = "";

            for (int i = 0; i < 52; i++)
                sDeck += deck[i].ToString() + "\n";

            return sDeck;
        }
    }
}