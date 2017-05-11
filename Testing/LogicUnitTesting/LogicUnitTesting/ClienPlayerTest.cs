using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicUnitTesting
{
    [TestClass]
    public class ClientPlayerTest
    {
        [TestMethod]
        public void SetChipsTest()
        {
            // Arrange
            unit chips = 0;
            unit amount = 20;
            
            // Act
            SetChips(amount);
            
            // Assert
            Assert.AreEqual(chips,20);
        }
        
        public void TakeTurnTest()
        {
            // Arrange
            bool inputEnabled = false;
            
            // Act
            TakeTurn();
            
            // Assert
            Assert.AreEqual(inputEnabled,true);
        }
        
        public void ResetStakesTest()
        {
            // Arrange
            unit playerBid = 50;
            
            // Act
            ResetStakes();
            
            // Assert
            Assert.AreEqual(playerBid,0);
            Assert.AreEqual(ClientManager.highestBid,0);
        }
    }
}
