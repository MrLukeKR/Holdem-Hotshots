using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicUnitTesting
{
    [TestClass]
    public class PotTest
    {
        [TestMethod]
        public void getBlindTest()
        {
            // Arrange
            Pot pot = new Pot(20,40);
            
            // Act
            pot.getSmallBlind();
            pot.getBigBlind();
            
            // Assert
            Assert.AreEqual(sBlind,20);
            Assert.AreEqual(bBlind,40);
        }
        
        public void setBlindTest()
        {
            // Act
            setSmallBlind(20);
            setBigBlind(40);
            
            // Assert
            Assert.AreEqual(sBlind,20);
            Assert.AreEqual(bBlind,40);
        }
        
        public void payInTest()
        {
            // Arrange
            unit amount = 20;
            
            // Act
            payIn(40);
            
            // Assert
            Assert.AreEqual(amount,60);
        }
        
        public void cashoutTest()
        {
            // Arrange
            unit amount = 40;
            
            // Act
            cashout();
            
            // Assert
            Assert.AreEqual(jackpot,40);
            Assert.AreEqual(amount,0);
        }
    }
}
