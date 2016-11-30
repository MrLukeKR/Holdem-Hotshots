using System;
using System.Collections.Generic;
using System.Text;
using TexasHoldemPoker.Game.PokerObjects;

namespace TexasHoldemPoker.Game.GameObjects
{
    interface GameEntity
    {
        void dealCard(Card card);
        void returnCardToDeck(Card card);

        void giveChips(uint amount);
        void takeChips(uint amount);
        void transferChips(GameEntity recipient);
    }
}
