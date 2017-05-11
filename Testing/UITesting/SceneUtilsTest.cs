using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UITesting
{
    [TestClass]
    public class SceneUtilsTest
    {
        [TestMethod]
        public void UpdatePotBalanceTest()
        {
            // Arrange
            unit amount = 30;
            
            // Act
            UpdatePotBalance(amount);
            
            // Assert
            Assert.AreEqual(potText.text,"Pot\n$30");
        }
        
        public void UpdatePlayerInformationTest()
        {
            // Arrange
            Text3D playerText = FindComponent<Text3D>(playerName, SceneManager.hostScene);
             
            // Act
            UpdatePlayerInformation(string playerName, string information);
            
            // Assert
            Assert.AreNotEqual(playerText.Text,null);
        }
        
        public void DisplayWinnerTest()
        {
            // Arrange
            Text3D playerText = FindComponent<Text3D>(playerName, SceneManager.hostScene);
            
            // Act
            DisplayerWinner(ServerPlayer player, CardRanker.Hand hand);
            
            // Assert
            Assert.AreNotEqual(message.Text,null);
            Assert.AreNotEqual(SpeechManager.Speak,null);
        }
    }
}
