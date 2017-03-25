using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicUnitTesting
{
    [TestClass]
    public class Card
    {
        [TestMethod]
        // Do we need to test Call(), AllIn(), Check(), Raise()?
        
        public void SetChips()
        {
            // Arrange
            unit amount = 10;
            
            // Act
            actual = SetChips(amount);
            
            // Assert
            Assert.AreEqual(actual,10);
        }
    }
}
