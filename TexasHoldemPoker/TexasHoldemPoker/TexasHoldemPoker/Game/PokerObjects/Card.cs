using MixedRealityPoker.Game.RenderEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldemPoker.Game.Interfaces;
using TexasHoldemPoker.Game.PokerObjects;

namespace MixedRealityPoker.Game.PokerObjects
{
    class Card : Transferrable, Drawable
    {
        private Model cardModel;

        public Card(int suit, int value) //Provide handling of models/textures for the card
        {

        }

        public void draw()
        {
            throw new NotImplementedException();
        }

        public void transfer(GameEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
