using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicUnitTesting
{
    [TestClass]
    public class Card
    {
        [TestMethod]
        public void GetRemainingPlayers()
        {
            // Arrange
            
            // Act
            int actual1 = ca.GetRemainingPlayers();
            // Assert
            Assert.AreEqual();
        }
        public void CheckConnections()
        {
            // how to test
        }
        public void Cleanup()
        {
            // Arrange
            List<ServerPlayer> toRemove = new List<ServerPlayer>();
            // Act
            int actual1 = ca.GetRemainingPlayers();
            // Assert
            Assert.AreEqual();
        }
    }
}
