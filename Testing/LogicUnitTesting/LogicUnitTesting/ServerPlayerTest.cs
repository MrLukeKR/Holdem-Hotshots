using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicUnitTesting
{
    [TestClass]
    public class ServerPLayerTest
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
        
        public void TakeChipsTest1()
        {
            // Arrange
            unit chips = 50;
            unit amount = 20;
            unit currentStake = 70;
            
            // Act
            bool actual = TakeChips(amount);
            
            // Assert
            Assert.AreEqual(chips,30);
            Assert.AreEqual(currentStake,90);
            Assert.AreEqual(actual,20);
        }
        
        public void TakeChipsTest2()
        {
            // Arrange
            unit chips = 50;
            unit amount = 60;
            
            // Act
            bool actual = TakeChips(amount);
            
            // Assert
            Assert.AreEqual(chips,50);
            Assert.AreEqual(actual,0);
        }
        
        public void ApplyBlindTest()
        {
            // Arrange
            unit chips = 50;
            unit amount1 = 20;
            unit amount2 = 60;
            
            // Act
            bool actual1 = ApplyBlind(amount1);
            bool actual2 = ApplyBlind(amount2);
            
            // Assert
            Assert.AreEqual(actual2,20);
            Assert.AreEqual(actual2,0);
        }
        
        public void FoldTest()
        {
            // Arrange
            bool folded = false;
            
            // Act
            Fold();
            
            // Assert
            Assert.AreEqual(folded,true);
            Assert.AreEqual(hasTakenTurn,true;
        }
    }
}
