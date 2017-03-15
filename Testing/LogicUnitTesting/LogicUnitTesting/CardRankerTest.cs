using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicUnitTesting
{
    [TestClass]
    public class CardRankerTest
    {
        [TestMethod]
        public void evaluateGameTest()
        {
            // Arrange
            
            // Act
            
            // Assert
        }
        public void rankCardsTest()
        {
            // Arrange
            List<Card> cards1 = new List<Card>() {(HEARTS,ACE),(HEARTS,KING),(HEARTS,QUEEN),(HEARTS,JACK),(HEARTS,TEN),(DIAMONDS,TWO),(DIAMONDS,EIGHT)};
            List<Card> cards2 = new List<Card>() {(HEARTS,NINE),(HEARTS,KING),(HEARTS,QUEEN),(HEARTS,JACK),(HEARTS,TEN),(DIAMONDS,TWO),(DIAMONDS,EIGHT)};
            List<Card> cards3 = new List<Card>() {(HEARTS,KING),(HEARTS,KING),(HEARTS,KING),(HEARTS,KING),(HEARTS,TEN),(HEARTS,TWO),(HEARTS,EIGHT)};
            List<Card> cards4 = new List<Card>() {(HEARTS,KING),(HEARTS,KING),(HEARTS,KING),(HEARTS,TE),(HEARTS,TEN),(HEARTS,TWO),(HEARTS,EIGHT)};
            List<Card> cards5 = new List<Card>() {(HEARTS,KING),(HEARTS,NINE),(HEARTS,THREE),(HEARTS,FIVE),(HEARTS,TEN),(DIAMONDS,TWO),(SPADES,EIGHT)};
            List<Card> cards6 = new List<Card>() {(HEARTS,KING),(HEARTS,QUEEN),(HEARTS,JACK),(HEARTS,TEN),(DIAMONDS,NINE),(HEARTS,TWO),(HEARTS,EIGHT)};
            List<Card> cards7 = new List<Card>() {(HEARTS,KING),(HEARTS,KING),(HEARTS,KING),(HEARTS,QUEEN),(HEARTS,TEN),(HEARTS,TWO),(HEARTS,EIGHT)};
            List<Card> cards8 = new List<Card>() {(HEARTS,KING),(HEARTS,KING),(HEARTS,QUEEN),(HEARTS,QUEEN),(HEARTS,TEN),(HEARTS,TWO),(HEARTS,EIGHT)};
            List<Card> cards9 = new List<Card>() {(HEARTS,KING),(HEARTS,KING),(HEARTS,QUEEN),(HEARTS,JACK),(HEARTS,TEN),(HEARTS,TWO),(HEARTS,EIGHT)};
            List<Card> cards10 = new List<Card>() {(HEARTS,KING),(HEARTS,QUEEN),(HEARTS,JACK),(HEARTS,THREE),(HEARTS,TEN),(HEARTS,TWO),(HEARTS,EIGHT)};
            
            // Act
            Hand actual1 = cards1.rankCards();
            Hand actual2 = cards2.rankCards();
            Hand actual3 = cards3.rankCards();
            Hand actual4 = cards4.rankCards();
            Hand actual5 = cards5.rankCards();
            Hand actual6 = cards6.rankCards();
            Hand actual7 = cards7.rankCards();
            Hand actual8 = cards8.rankCards();
            Hand actual9 = cards9.rankCards();
            Hand actual10 = cards10.rankCards();
            
            // Assert
            Assert.AreEqual(actual1,10);
            Assert.AreEqual(actual2,9);
            Assert.AreEqual(actual3,8);
            Assert.AreEqual(actual4,7);
            Assert.AreEqual(actual5,6);
            Assert.AreEqual(actual6,5);
            Assert.AreEqual(actual7,4);
            Assert.AreEqual(actual8,3);
            Assert.AreEqual(actual9,2);
            Assert.AreEqual(actual10,1);
        }
        public void IsRoyalFlushTest()
        {
            // Arrange
            List<Card> cards1 = new List<Card>() {(CLUBS,ACE),(CLUBS,KING),(CLUBS,QUEEN),(CLUBS,JACK),(CLUBS,TEN),(CLUBS,NINE),(CLUBS,EIGHT)};
            List<Card> cards2 = new List<Card>() {(CLUBS,ACE),(CLUBS,KING),(CLUBS,QUEEN),(CLUBS,JACK),(SPADES,TEN),(CLUBS,NINE),(CLUBS,EIGHT)};
            List<Card> cards3 = new List<Card>() {(CLUBS,ACE),(CLUBS,KING),(CLUBS,QUEEN),(CLUBS,JACK),(CLUBS,NINE),(CLUBS,EIGHT),(CLUBS,SEVEN)};
            
            // Act
            bool actual1 = cards1.IsRoyalFlush();
            bool actual2 = cards2.IsRoyalFlush();
            bool actual3 = cards2.IsRoyalFlush();
            
            // Assert
            Assert.AreEqual(actual1,true);
            Assert.AreEqual(actual2,false);
            Assert.AreEqual(actual3,false);
        }
        public void IsOfAKindTest()
        {
            // returnHighCard???
            
            // Arrange
            List<Card> cards1 = new List<Card>() {(CLUBS,ACE),(CLUBS,ACE),(CLUBS,ACE),(CLUBS,ACE),(CLUBS,TWO),(CLUBS,THREE),(CLUBS,FOUR)};
            List<Card> cards2 = new List<Card>() {(CLUBS,ACE),(CLUBS,ACE),(CLUBS,ACE),(CLUBS,FIVE),(CLUBS,TWO),(CLUBS,THREE),(CLUBS,FOUR)};
            
            // Act
            int actual1 = cards1.IsRoyalFlush();
            int actual2 = cards2.IsRoyalFlush();
            
            // Assert
            Assert.AreEqual(actual1,4);
            Assert.AreEqual(actual2,3);
        }
        public void isFlushTest()
        {
            // Arrange
            List<Card> cards1 = new List<Card>() {(CLUBS,ACE),(CLUBS,TWO),(CLUBS,THREE),(CLUBS,FOUR),(CLUBS,FIVE),(SPADES,SIX),(SPADES,SEVEN)};
            List<Card> cards2 = new List<Card>() {(CLUBS,ACE),(CLUBS,TWO),(CLUBS,THREE),(CLUBS,FOUR),(DIAMONDS,FIVE),(SPADES,SIX),(SPADES,SEVEN)};
            
            // Act
            bool actual1 = cards1.IsRoyalFlush();
            bool actual2 = cards2.IsRoyalFlush();
            
            // Assert
            Assert.AreEqual(actual1,true);
            Assert.AreEqual(actual2,false);
        }
        public void isStraightTest()
        {
            // Arrange
            List<Card> cards1 = new List<Card>() {(CLUBS,ACE),(CLUBS,TWO),(CLUBS,THREE),(CLUBS,FOUR),(CLUBS,FIVE),(CLUBS,SEVEN),(CLUBS,EIGHT)};
            List<Card> cards2 = new List<Card>() {(CLUBS,ACE),(CLUBS,KING),(CLUBS,QUEEN),(CLUBS,JACK),(CLUBS,TEN),(CLUBS,SIX),(CLUBS,SEVEN)};
            List<Card> cards3 = new List<Card>() {(CLUBS,ACE),(CLUBS,TWO),(CLUBS,THREE),(CLUBS,FOUR),(CLUBS,SIX),(CLUBS,SEVEN),(CLUBS,EIGHT)};
            
            // Act
            bool actual1 = cards1.IsRoyalFlush();
            bool actual2 = cards2.IsRoyalFlush();
            bool actual3 = cards3.IsRoyalFlush();
            
            // Assert
            Assert.AreEqual(actual1,true);
            Assert.AreEqual(actual2,true);
            Assert.AreEqual(actual3,false);
        }
        public void anyPairsTest()
        {
            // Arrange
            List<Card> cards1 = new List<Card>() {(CLUBS,ACE),(CLUBS,TWO),(CLUBS,THREE),(CLUBS,FOUR),(CLUBS,FIVE),(CLUBS,SIX),(CLUBS,SEVEN)};
            List<Card> cards2 = new List<Card>() {(CLUBS,ACE),(CLUBS,ACE),(CLUBS,THREE),(CLUBS,FOUR),(CLUBS,FIVE),(CLUBS,SIX),(CLUBS,SEVEN)};
            List<Card> cards3 = new List<Card>() {(CLUBS,ACE),(CLUBS,ACE),(CLUBS,THREE),(CLUBS,THREE),(CLUBS,FIVE),(CLUBS,SIX),(CLUBS,SEVEN)};
            
            // Act
            int actual1 = cards1.IsRoyalFlush();
            int actual2 = cards2.IsRoyalFlush();
            int actual3 = cards3.IsRoyalFlush();
            
            // Assert
            Assert.AreEqual(actual1,0);
            Assert.AreEqual(actual2,1);
            Assert.AreEqual(actual3,2);
        }
    }
}
