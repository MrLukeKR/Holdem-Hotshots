using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicUnitTesting
{
    [TestClass]
    public class DeckTest
    {
        [TestMethod]
        public void initTest()
        {
            // Arrange
            List<Card> expected = new List<Card> {(0,1),(0,2),(0,3),(0,4),(0,5),(0,6),(0,7),(0,8),(0,9),(0,10),(0,11),(0,12),(0,13),(1,1),(1,2),(1,3),(1,4),(1,5),(1,6),(1,7),(1,8),(1,9),(1,10),(1,11),(1,12),(1,13),(2,1),(2,2),(2,3),(2,4),(2,5),(2,6),(2,7),(2,8),(2,9),(2,10),(2,11),(2,12),(2,13),(3,1),(3,2),(3,3),(3,4),(3,5),(3,6),(3,7),(3,8),(3,9),(3,10),(3,11),(3,12),(3,13)};
            List<Card> deck = new List<Card>();
            
            // Act
            deck.init();
            
            // Assert
            Assert.AreEqual(deck,expected);
        }
        
        public void takeCardTest()
        {
            // Arrange
            List<Card> deck = new List<Card> {(3,11),(3,12),(3,13)}
            List<Card> expected = new List<Card> {(3,12),(3,13)}
            
            // Act
            deck.takeCard();
            
            // Assert
            Assert.AreEqual(card,(3,11));
            Assert.AreEqual(deck,expected);
        }
    }
}
