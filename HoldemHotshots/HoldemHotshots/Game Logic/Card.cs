using System;
using System.Threading.Tasks;
using Urho;

namespace HoldemHotshots{
  class Card : Component{

        public static Vector3 cardDealingPos = new Vector3(0, 20, 0);

        //Card1 positions

        public static Vector3 card1HoldingPos = new Vector3(2.65f, -3.1f, 0.1f);
        public static Vector3 card1ViewingPos = new Vector3(-1.1f,  2.0f, 0.0f);

        //Card2 positions
        public static Vector3 card2HoldingPos = new Vector3(3.1f, -3.05f, 0.0f);
        public static Vector3 card2ViewingPos = new Vector3(1.1f,  2.00f, 0.0f);

        //Table card placement positions
        public static Vector3 cardTableDealingPos = new Vector3(10.0f, 0.0f, 0.4f);
        public static Vector3 card1TablePos = new Vector3(0.0f,  4.4f, 3.4f);
        public static Vector3 card2TablePos = new Vector3(0.0f,  2.2f, 3.4f);
        public static Vector3 card3TablePos = new Vector3(0.0f,  0.0f, 3.4f);
        public static Vector3 card4TablePos = new Vector3(0.0f, -2.2f, 3.4f);
        public static Vector3 card5TablePos = new Vector3(0.0f, -4.4f, 3.4f);
        
        public enum Rank { ACE = 1, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING }
    	public enum Suit { CLUBS, SPADES, DIAMONDS, HEARTS }
   	 	public Suit suit { set; get; }
   		public Rank rank { set; get; }
    	Node node = new Node();

    	public Card(Suit suit, Rank rank){
      		this.suit = suit;
      		this.rank = rank;
      		init();
    	}

    	public override String ToString(){
      		String sSuit = "", sRank = "";
      		
			switch (suit){
        		case Suit.CLUBS: sSuit = "Clubs"; break;
        		case Suit.DIAMONDS: sSuit = "Diamonds"; break;
        		case Suit.HEARTS: sSuit = "Hearts"; break;
        		case Suit.SPADES: sSuit = "Spades"; break;
      		}

      		switch (rank){
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
    public void init(){
      var cache = Application.ResourceCache;
      StaticModel model = node.CreateComponent<StaticModel>();
      model.Model = cache.GetModel("Models/Box.mdl");
      String filename = "";
      switch (suit){
        case Suit.CLUBS: filename = "C"; break;
        case Suit.DIAMONDS: filename = "D"; break;
        case Suit.SPADES: filename = "S"; break;
        case Suit.HEARTS: filename = "H"; break;
      }

      filename += (int)rank + ".xml";
      var material = cache.GetMaterial("Materials/Cards/" + filename);
      model.SetMaterial(material);
      node.Scale = new Vector3(1.0f, 1.4f, 0f) * 2;
    }
    internal Node getNode(){ return node; }
  }
}