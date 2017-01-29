using System;
using System.Collections.Generic;

namespace HoldemHotshots{
  class Deck{
    List<Card> deck = new List<Card>();

    public Deck() { init(); }
    private void init() {
      for (int s = 0; s < 4; s++)
        for (int r = 1; r <= 13; r++)
          deck.Add(new Card((Card.Suit)s, (Card.Rank)r));
    }
        
    public void shuffle(){
      var rnd = new Random();
      var shuffledDeck = new List<Card>();
      int index;
      while(shuffledDeck.Count < 52){
        index = rnd.Next(0, deck.Count);
        shuffledDeck.Add(deck[index]);
        deck.RemoveAt(index);
      }
      deck = shuffledDeck;
    }
    internal void dealTo(List<Card> hand){ hand.Add(takeCard()); }
    private Card takeCard(){
      Card card = deck[0];
      deck.RemoveAt(0);
      return card;
    }
    public override String ToString(){
      String sDeck = "";
      for (int i = 0; i < 52; i++)
        sDeck += deck[i].ToString() + "\n";
      return sDeck;
    }
  }
}
