using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicUnitTesting
{
    [TestClass]
    public class CardRankerTest
    {
        [TestMethod]
        public void rankCardsTest()
        {
            // Arrange
            List<Card> cards1 = new List<Card> {(0,1),(0,13),(0,12),(0,11),(0,10),(0,5),(0,2)};
            List<Card> cards2 = new List<Card> {(0,13),(0,12),(0,11),(0,10),(0,9),(0,5),(0,2)};
            List<Card> cards3 = new List<Card> {(0,13),(1,13),(2,13),(3,13),(0,9),(0,5),(0,2)};
            List<Card> cards4 = new List<Card> {(0,13),(1,13),(2,13),(0,9),(1,9),(2,9),(0,2)};
            List<Card> cards5 = new List<Card> {(0,13),(0,12),(0,11),(0,9),(0,7),(0,5),(0,2)};
            List<Card> cards6 = new List<Card> {(0,13),(0,12),(0,11),(0,10),(0,9),(0,5),(0,2)};
            List<Card> cards7 = new List<Card> {(0,13),(1,13),(2,13),(0,9),(1,9),(2,9),(0,2)};
            List<Card> cards8 = new List<Card> {(0,13),(1,13),(0,9),(1,9),(0,8),(0,5),(0,2)};
            List<Card> cards9 = new List<Card> {(0,13),(1,13),(0,10),(0,9),(0,8),(0,5),(0,2)};
            List<Card> cards10 = new List<Card> {(2,13),(2,12),(2,11),(2,3),(2,10),(2,2),(2,8)};
            
            // Act
            Hand actual1 = rankCards(cards1);
            Hand actual2 = rankCards(cards2);
            Hand actual3 = rankCards(cards3);
            Hand actual4 = rankCards(cards4);
            Hand actual5 = rankCards(cards5);
            Hand actual6 = rankCards(cards6);
            Hand actual7 = rankCards(cards7);
            Hand actual8 = rankCards(cards8);
            Hand actual9 = rankCards(cards9);
            Hand actual10 = rankCards(cards10);
            
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

        public void isRoyalFlushTest()
        {
            // Arrange
            List<Card> cards1 = new List<Card> {(0,1),(0,13),(0,12),(0,11),(0,10),(0,5),(0,2)};
            List<Card> cards1 = new List<Card> {(0,13),(0,12),(0,11),(0,10),(0,9),(0,5),(0,2)};
            List<Card> cards1 = new List<Card> {(1,1),(0,13),(0,12),(0,11),(0,10),(0,5),(0,2)};
            
            // Act
            bool actual1 = isRoyalFlush(cards1);
            bool actual2 = isRoyalFlush(cards2);
            bool actual3 = isRoyalFlush(cards3);
            
            // Assert
            Assert.AreEqual(actual1,true);
            Assert.AreEqual(actual2,false);
            Assert.AreEqual(actual3,false);
        }
        
        public void isStraightFlushTest()
        {
            // Arrange
            List<Card> cards1 = new List<Card> {(0,13),(0,12),(0,11),(0,10),(0,9),(0,5),(0,2)};
            List<Card> cards2 = new List<Card> {(0,13),(0,12),(0,11),(0,10),(0,8),(0,5),(0,2)};
            List<Card> cards3 = new List<Card> {(1,13),(0,12),(0,11),(0,10),(0,9),(0,5),(0,2)};
            
            // Act
            bool actual1 = isStraightFlush(cards1);
            bool actual2 = isStraightFlush(cards2);
            bool actual3 = isStraightFlush(cards3);
            
            // Assert
            Assert.AreEqual(actual1,true);
            Assert.AreEqual(actual2,false);
            Assert.AreEqual(actual3,false);
        }
        
        public void isFourTest()
        {
            // Arrange
            List<Card> cards1 = new List<Card> {(0,13),(1,13),(2,13),(3,13),(0,9),(0,5),(0,2)};
            List<Card> cards2 = new List<Card> {(0,13),(1,13),(2,13),(0,10),(0,9),(0,5),(0,2)};
            
            // Act
            bool actual1 = isFour(cards1);
            bool actual2 = isFour(cards2);
            
            // Assert
            Assert.AreEqual(actual1,true);
            Assert.AreEqual(actual2,false);
        }
        
        public void isFullHouseTest()
        {
            // Arrange
            List<Card> cards1 = new List<Card> {(0,13),(1,13),(2,13),(0,9),(1,9),(2,9),(0,2)};
            List<Card> cards2 = new List<Card> {(0,13),(1,13),(2,13),(0,9),(1,9),(0,5),(0,2)};
            List<Card> cards3 = new List<Card> {(0,13),(1,13),(2,13),(0,9),(0,8),(0,5),(0,2)};
            List<Card> cards4 = new List<Card> {(0,13),(1,13),(0,9),(1,9),(0,8),(0,5),(0,2)};
            
            // Act
            bool actual1 = isFullHouse(cards1);
            bool actual2 = isFullHouse(cards2);
            bool actual3 = isFullHouse(cards3);
            bool actual4 = isFullHouse(cards4);
            
            // Assert
            Assert.AreEqual(actual1,true);
            Assert.AreEqual(actual2,true);
            Assert.AreEqual(actual3,false);
            Assert.AreEqual(actual4,false);
        }
        
        public void isFlushTest()
        {
            // Arrange
            List<Card> cards1 = new List<Card> {(0,13),(0,12),(0,11),(0,9),(0,7),(0,5),(0,2)};
            List<Card> cards2 = new List<Card> {(0,13),(0,12),(0,11),(0,9),(0,7),(1,5),(1,2)};
            List<Card> cards3 = new List<Card> {(0,13),(0,12),(0,11),(0,9),(1,7),(1,5),(1,2)};
            
            // Act
            bool actual1 = isFlush(cards1);
            bool actual2 = isFlush(cards2);
            bool actual3 = isFlush(cards3);
            
            // Assert
            Assert.AreEqual(actual1,true);
            Assert.AreEqual(actual2,true);
            Assert.AreEqual(actual3,false);
        }
        
        public void isStraightTest()
        {
            // Arrange
            List<Card> cards1 = new List<Card> {(0,13),(0,12),(0,11),(0,10),(0,9),(0,5),(0,2)};
            List<Card> cards2 = new List<Card> {(0,1),(0,13),(0,12),(0,11),(1,10),(0,7),(0,5)};
            List<Card> cards3 = new List<Card> {(0,13),(0,12),(0,11),(0,10),(0,8),(0,5),(0,2)};
            
            // Act
            bool actual1 = isStraight(cards1);
            bool actual2 = isStraight(cards2);
            bool actual3 = isStraight(cards3);
            
            // Assert
            Assert.AreEqual(actual1,true);
            Assert.AreEqual(actual2,true);
            Assert.AreEqual(actual3,false);
        }

        
        public void isThreeTest()
        {
            // Arrange
            List<Card> cards1 = new List<Card> {(0,13),(1,13),(2,13),(0,9),(1,9),(2,9),(0,2)};
            List<Card> cards2 = new List<Card> {(0,13),(1,13),(2,13),(3,13),(0,9),(0,5),(0,2)};
            List<Card> cards3 = new List<Card> {(0,13),(1,13),(0,9),(1,9),(0,8),(0,5),(0,2)};
            
            // Act
            bool actual1 = isThree(cards1);
            bool actual2 = isThree(cards2);
            bool actual3 = isThree(cards3);
            
            // Assert
            Assert.AreEqual(actual1,true);
            Assert.AreEqual(actual2,false);
            Assert.AreEqual(actual3,false);
        }
        
        public void isTwoPairsTest()
        {
            // Arrange
            List<Card> cards1 = new List<Card> {(0,13),(1,13),(0,9),(1,9),(0,8),(0,5),(0,2)};
            List<Card> cards2 = new List<Card> {(0,13),(1,13),(2,13),(0,9),(1,9),(0,5),(0,2)};
            List<Card> cards3 = new List<Card> {(0,13),(1,13),(0,10),(0,9),(0,8),(0,5),(0,2)};
            
            // Act
            bool actual1 = isTwoPairs(cards1);
            bool actual2 = isTwoPairs(cards2);
            bool actual3 = isTwoPairs(cards3);
            
            // Assert
            Assert.AreEqual(actual1,true);
            Assert.AreEqual(actual2,false);
            Assert.AreEqual(actual3,false);
        }
        
        public void isPairTest()
        {
            // Arrange
            List<Card> cards1 = new List<Card> {(0,13),(1,13),(0,10),(0,9),(0,8),(0,5),(0,2)};
            List<Card> cards2 = new List<Card> {(0,13),(1,13),(2,13),(0,9),(0,8),(0,5),(0,2)};
            List<Card> cards3 = new List<Card> {(0,13),(0,12),(0,10),(0,9),(0,8),(0,5),(0,2)};
            
            // Act
            bool actual1 = isPair(cards1);
            bool actual2 = isPair(cards2);
            bool actual3 = isPair(cards3);
            
            // Assert
            Assert.AreEqual(actual1,true);
            Assert.AreEqual(actual2,false);
            Assert.AreEqual(actual3,false);
        }
    }
}
