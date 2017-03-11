using System;
using System.Collections.Generic;

namespace HoldemHotshots.GameLogic
{
    class Deck
    {
        private List<Card> deck = new List<Card>(52);
        
        public Deck()
        {
            for (int s = 0; s < 4; s++)
                for (int r = 1; r <= 13; r++)
                    deck.Add(new Card((Card.Suit)s, (Card.Rank)r));
        }
        
        public void Shuffle()
        {
            var rnd = new Random();
            var shuffledDeck = new List<Card>();
            int index;
            
            while (shuffledDeck.Count < 52)
            {
                index = rnd.Next(0, deck.Count);
                shuffledDeck.Add(deck[index]);
                deck.RemoveAt(index);
            }

            deck = shuffledDeck;
        }
        
        public Card TakeCard()
        {
            Card card = deck[0];

            deck.RemoveAt(0);

            return card;
        }
    }
}