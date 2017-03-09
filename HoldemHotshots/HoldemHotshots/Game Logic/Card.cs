using System;
using Urho;

namespace HoldemHotshots
{
    public class Card
    {
        public static readonly Vector3 cardDealingPos = new Vector3(0, 20, 0);

        //Card1 positions
        public static readonly Vector3 card1HoldingPos = new Vector3(2.65f, -3.1f, 0.1f);
        public static readonly Vector3 card1ViewingPos = new Vector3(-1.1f,  2.0f, 0.0f);

        //Card2 positions
        public static readonly Vector3 card2HoldingPos = new Vector3(3.1f, -3.05f, 0.0f);
        public static readonly Vector3 card2ViewingPos = new Vector3(1.1f,  2.00f, 0.0f);

        //Table card placement positions
        public static readonly Vector3 cardTableDealingPos = new Vector3(10.0f, 0.0f, 0.4f);
        public static readonly Vector3[] cardTablePositions = {
            new Vector3(0.0f, 4.4f, 3.4f),
            new Vector3(0.0f, 2.2f, 3.4f),
            new Vector3(0.0f, 0.0f, 3.4f),
            new Vector3(0.0f, -2.2f, 3.4f),
            new Vector3(0.0f, -4.4f, 3.4f)
        };
        
        public enum Rank { ACE = 1, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING }
    	public enum Suit { CLUBS, SPADES, DIAMONDS, HEARTS }

   	 	public Suit suit { get; set; }
   		public Rank rank { get; set; }

        public Node node { get; private set; }

    	public Card(Suit suit, Rank rank)
        {
      		this.suit = suit;
      		this.rank = rank;
            node = new Node();
            Init();
    	}
        
        private void Init()
        {
            var cache = Application.Current.ResourceCache;
            string filename = "";
            StaticModel model = node.CreateComponent<StaticModel>();
            
            Application.InvokeOnMain(new Action(() => model.Model = cache.GetModel("Models/Box.mdl")));
            
            switch (suit)
            {
                case Suit.CLUBS: filename = "C"; break;
                case Suit.DIAMONDS: filename = "D"; break;
                case Suit.SPADES: filename = "S"; break;
                case Suit.HEARTS: filename = "H"; break;
            }

            filename += (int)rank + ".xml";
            Application.InvokeOnMain(new Action(() => 
            { 
                Material material = null;
                material = cache.GetMaterial("Materials/Cards/" + filename);
                model.SetMaterial(material);
            }
            ));

      node.Scale = new Vector3(1.0f, 1.4f, 0f) * 2;
    }
  }
}