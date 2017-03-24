using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicUnitTesting
{
    [TestClass]
    public class Card
    {
        [TestMethod]
        public void PayIn()
        {
            // Arrange
            unit amount = 10;
            
            // Act
            latestBet = PayIn(amount);
            
            // Assert
            Assert.AreEqual(latestBet,20);
        }
        public void cashout()
        {
            // Act
            cashout();
            
            // Assert
            Assert.IsTrue(jackpot>=0);
            Assert.AreEqual(amount,0);
        }
    }
}
