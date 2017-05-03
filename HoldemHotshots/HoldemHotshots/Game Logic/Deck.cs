using System;
using System.Collections.Generic;

namespace HoldemHotshots.GameLogic
{
    /// <summary>
    /// Stores the array of 52 cards for use in the game
    /// </summary>
    class Deck
    {
        private List<Card> deck = new List<Card>(52);
        
        public Deck()
        {
            for (int s = 0; s < 4; s++)
                for (int r = 1; r <= 13; r++)
                    deck.Add(new Card((Card.Suit)s, (Card.Rank)r));
        }
        
        /// <summary>
        /// Shuffles the cards into a random order
        /// </summary>
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
        
        /// <summary>
        /// Removes a card from the deck
        /// </summary>
        /// <returns>The removed card</returns>
        public Card TakeCard()
        {
            Card card = deck[0];

            deck.RemoveAt(0);

            return card;
        }
    }
}