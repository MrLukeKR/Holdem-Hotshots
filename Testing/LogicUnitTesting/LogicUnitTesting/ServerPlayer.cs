using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicUnitTesting
{
    [TestClass]
    public class Card
    {
        [TestMethod]
        public void GiveChips()
        {
            // Arrange
            unit amount = 10;
            unit chips = 10;
            
            // Act
            actual = GiveChips(amount);
            
            // Assert
            Assert.AreEqual(actual,20);
        }
        public uint TakeChips1()
        {
            // Arrange
            unit amount = 20;
            unit chips = 30;
            
            // Act
            actual = TakeChips(amount)
            
            // Assert
            Assert.AreEqual(actual,10);
        }
        public uint TakeChips2()
        {
            // Arrange
            unit amount = 20;
            unit chips = 10;
            
            // Act
            actual = TakeChips(amount)
            
            // Assert
            Assert.AreEqual(actual,0);
        }
    }
}
