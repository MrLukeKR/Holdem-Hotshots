using System;
using System.Threading.Tasks;
using Urho;
using Urho.Actions;

namespace PokerLogic
{
    class Card : Component
    {
        public enum Rank { ACE = 1, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING }
        public enum Suit { CLUBS, SPADES, DIAMONDS, HEART }

        Suit suit;
        Rank rank;

        Node node = new Node();

        public Card(Suit suit, Rank rank)
        {
            this.suit = suit;
            this.rank = rank;

           init();
        }

        public override String ToString()
        {
            String sSuit = "", sRank = "";

            switch (suit)
            {
                case Suit.CLUBS: sSuit = "Clubs"; break;
                case Suit.DIAMONDS: sSuit = "Diamonds"; break;
                case Suit.HEART: sSuit = "Hearts"; break;
                case Suit.SPADES: sSuit = "Spades"; break;
            }

            switch (rank)
            {
                case Rank.ACE:sRank = "Ace"; break;
                case Rank.TWO: sRank = "Two"; break;
                case Rank.THREE: sRank = "Three"; break;
                case Rank.FOUR: sRank = "Four"; break;
                case Rank.FIVE: sRank = "Five"; break;
                case Rank.SIX: sRank = "Six"; break;
                case Rank.SEVEN: sRank = "Seven"; break;
                case Rank.EIGHT: sRank = "Eight"; break;
                case Rank.NINE: sRank = "Nine"; break;
                case Rank.TEN: sRank = "Ten"; break;
                case Rank.JACK: sRank = "Jack"; break;
                case Rank.QUEEN: sRank = "Queen"; break;
                case Rank.KING: sRank = "King"; break;
            }

            return sRank + " of " + sSuit;
        }

        public void init()
        {
            var cache = Application.ResourceCache;

            var model = node.CreateComponent<StaticModel>();

            model.Model = cache.GetModel("Models/Card.mdl");

            var material = cache.GetMaterial("Materials/White.xml");

            model.SetMaterial(material);

            node.Scale = new Vector3(1.4f, 1.0f, 1.0f);

        }

        internal Node getNode()
        {
            return node;
        }

        public async void fullView()
        {
            var moveAction = new MoveTo(1, new Vector3(0f,0f,0f));
            await node.RunActionsAsync(moveAction, moveAction.Reverse());
        }
    }
}
