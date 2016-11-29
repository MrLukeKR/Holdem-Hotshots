using System;
using TexasHoldemPoker.Game.GameObjects;

namespace TexasHoldemPoker.Game.PokerObjects
{
    class Table : GameEntity
    {
        Card[] cards;
        uint currentCard = 0;

        public Table()
        {
            cards = new Card[5];
        }

        public void dealCard(Card card)
        {
            cards[currentCard++] = card;
        }

        public void giveChips(uint amount)
        {
            throw new NotImplementedException();
        }

        public void returnCardToDeck(Card card)
        {
            throw new NotImplementedException();
        }

        public void takeChips(uint amount)
        {
            throw new NotImplementedException();
        }

        public void transferChips(GameEntity recipient)
        {
            throw new NotImplementedException();
        }
    }
}
