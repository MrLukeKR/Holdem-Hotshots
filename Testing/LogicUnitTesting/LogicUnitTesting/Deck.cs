using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicUnitTesting
{
    [TestClass]
    public class Card
    {
        [TestMethod]
        public void Shuffle()
        {
            // how to test?
        }
        
        public void TakeCard()
        {
            // Arrange
            List<Card> deck = new List<Card>() {(CLUBS,ACE),(CLUBS,TWO),(CLUBS,THREE),(CLUBS,FOUR),(CLUBS,FIVE),(CLUBS,SIX),(CLUBS,SEVEN),(CLUBS,EIGHT),(CLUBS,NINE),(CLUBS,TEN),(CLUBS,JACK),(CLUBS,QUEEN),(CLUBS,KING),(SPADES,ACE),(SPADES,TWO),(SPADES,THREE),(SPADES,FOUR),(SPADES,FIVE),(SPADES,SIX),(SPADES,SEVEN),(SPADES,EIGHT),(SPADES,NINE),(SPADES,TEN),(SPADES,JACK),(SPADES,QUEEN),(SPADES,KING),(DIAMONDS,ACE),(DIAMONDS,TWO),(DIAMONDS,THREE),(DIAMONDS,FOUR),(DIAMONDS,FIVE),(DIAMONDS,SIX),(DIAMONDS,SEVEN),(DIAMONDS,EIGHT),(DIAMONDS,NINE),(DIAMONDS,TEN),(DIAMONDS,JACK),(DIAMONDS,QUEEN),(DIAMONDS,KING),(HEARTS,ACE),(HEARTS,TWO),(HEARTS,THREE),(HEARTS,FOUR),(HEARTS,FIVE),(HEARTS,SIX),(HEARTS,SEVEN),(HEARTS,EIGHT),(HEARTS,NINE),(HEARTS,TEN),(HEARTS,JACK),(HEARTS,QUEEN),(HEARTS,KING)}
            
            // Act
            Card actual = TakeCard(deck);
            
            // Assert
            Assert.AreEqual(actual,(CLUBS,ACE));
        }
    }
}
