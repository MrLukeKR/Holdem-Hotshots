using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicUnitTesting
{
    [TestClass]
    public class CardTest
    {
        [TestMethod]
        public void ToStringTest()
        {
            // Arrange
            Card card = new Card(0,1);
            
            // Act
            card.ToString();
            
            // Assert
            Assert.AreEqual(sRank + " of " + sSuit,"Ace of Clubs");
        }
        
        public void getTest()
        {
            // Arrange
            Card card = new Card(3,5);
            
            // Act
            card.getSuit();
            card.getValue();
            
            // Assert
            Assert.AreEqual(suit,3);
            Assert.AreEqual(rank,5);
        }
        
        public void initTest()
        {
            // Arrange
            Card card = new Card(2,7);
            
            // Act
            card.init();
            
            // Assert
            Assert.AreEqual(filename,"H.xml")
        }
    }
}
