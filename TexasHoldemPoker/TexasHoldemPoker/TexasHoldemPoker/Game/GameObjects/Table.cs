using System;
using TexasHoldemPoker.Game.GameObjects;
using Urho;
using Urho.Audio;

namespace TexasHoldemPoker.Game.PokerObjects
{
    class Table : GameEntity
    {
        Card[] cards;
        uint currentCard = 0;
        Scene tableScene;

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

        

        public void DropChip1()
        {
            //Generate a $1 chip object and drop it on the table
        }

        public void DropChip2_50()
        {
            //Generate a $2.50 chip object and drop it on the table
        }

        public void DropChip5()
        {
            //Generate a $5 chip object and drop it on the table
        }

        public void DropChip10()
        {
            //Generate a $10 chip object and drop it on the table
        }

        public void DropChip25()
        {
            //Generate a $25 chip object and drop it on the table
        }

        public void DropChip100()
        {
            //Generate a $100 chip object and drop it on the table
        }
    }
}
