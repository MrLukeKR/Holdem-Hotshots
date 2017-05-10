using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicUnitTesting
{
    [TestClass]
    public class TableTest
    {
        Room pokerRoom = new Room();
        
        [TestMethod]
        public void dealToTableTest()
        {
            // Arrange
            List<Card> deck = new List<Card> {(3,8),(2,11),(2,5),(1,3),(3,12)}
            List<Card> hand = new List<Card>();
            
            // Act
            dealToTable();
            
            // Assert
            Assert.AreEqual(hand,(3,8));
        }
        
        public void getRemainingPlayersTest()
        {
            // Act
            actual = getRemainingPlayers();
            
            // Assert
            Assert.AreEqual(actual,3);
        }
        
        public void flopTest()
        {
            // Arrange
            List<Card> hand = new List<Card>();
            List<Card> deck = new List<Card> {(3,8),(2,11),(2,5),(1,3),(3,12)}
            List<Card> expected = new List<Card> {(3,8),(2,11),(2,5)};
            
            // Act
            flop();
            
            // Assert
            Assert.AreEqual(hand,expected);
        }
        
        public void incrementIndexTest1()
        {
            // Arrange
            int index = 2;
            
            // Act
            pokerRoom.incremenIndex();
            
            // Assert
            Assert.AreEqual(index,3);
        }
        
        public void incrementIndexTest2()
        {
            // Arrange
            int index = 3;
            
            // Act
            pokerRoom.incremenIndex();
            
            // Assert
            Assert.AreEqual(index,0);
        }
    }
}
