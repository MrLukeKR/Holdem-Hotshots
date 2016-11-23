using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldemPoker.Game.PokerObjects;

namespace MixedRealityPoker.Game.PokerObjects
{
    class Card
    {
        private int suit, rank; //Change this from int to whatever type you use (try an enum)?

        //TODO: Provide handling of models/textures for the card
        public Card(int suit, int rank) //Change param types from int to whatever type you use (try an enum)?
        {

        }
        public int GetSuit()//Change return type from int to whatever type you use (try an enum)?
        {
            return suit;
        }

        public int GetRank()//Change return type from int to whatever type you use (try an enum)?
        {
            return rank;
        }
     
    }
}
