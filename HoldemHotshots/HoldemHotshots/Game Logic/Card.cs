using System;
using Urho;

namespace HoldemHotshots.GameLogic
{
    public class Card
    {
        public static readonly Vector3 CARD_DEALING_POSITION        = new Vector3(0, 20, 0);

        //Card1 positions
        public static readonly Vector3 CARD_1_HOLDING_POSITION      = new Vector3(2.65f, -3.1f, 0.1f);
        public static readonly Vector3 CARD_1_VIEWING_POSITION      = new Vector3(-1.1f,  2.0f, 0.0f);

        //Card2 positions
        public static readonly Vector3 CARD_2_HOLDING_POSITION      = new Vector3(3.1f, -3.05f, 0.0f);
        public static readonly Vector3 CARD_2_VIEWING_POSITION      = new Vector3(1.1f,  2.00f, 0.0f);

        //Table card placement positions
        public static readonly Vector3 CARD_TABLE_DEALING_POSITION  = new Vector3(10.0f, 0.0f, 0.4f);
        public static readonly Vector3[] CARD_TABLE_POSITIONS = {
            new Vector3(0.0f, 4.4f, 3.4f),
            new Vector3(0.0f, 2.2f, 3.4f),
            new Vector3(0.0f, 0.0f, 3.4f),
            new Vector3(0.0f, -2.2f, 3.4f),
            new Vector3(0.0f, -4.4f, 3.4f)
        };

        public enum Rank { ACE = 1, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING }
    	public enum Suit { CLUBS, SPADES, DIAMONDS, HEARTS }

        public readonly Suit suit;
        public readonly Rank rank;

        public Node node { get; private set; }

    	public Card(Suit suit, Rank rank)
        {
      		this.suit = suit;
      		this.rank = rank;
            
            Init();
    	}
        
        private void Init()
        {
            var cache = Application.Current.ResourceCache;
            string filename = "";

            node = new Node();

            StaticModel model = node.CreateComponent<StaticModel>();
            
            Application.InvokeOnMain(new Action(() => model.Model = cache.GetModel("Models/Box.mdl")));
            node.Scale = new Vector3(1.0f, 1.4f, 0f) * 2;

            switch (suit)
            {
                case Suit.CLUBS:    filename = "C"; break;
                case Suit.DIAMONDS: filename = "D"; break;
                case Suit.SPADES:   filename = "S"; break;
                case Suit.HEARTS:   filename = "H"; break;
            }

            filename += (int)rank + ".xml";

            Application.InvokeOnMain(new Action(() => 
            { 
                Material material = cache.GetMaterial("Materials/Cards/" + filename);
                model.SetMaterial(material);
            }));
        }
    }
}