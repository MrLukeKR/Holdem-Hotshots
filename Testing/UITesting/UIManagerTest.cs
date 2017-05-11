using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UITesting
{
    [TestClass]
    public class UIManagerTest
    {
        [TestMethod]
        public void CreateMenuUITest()
        {
            // Arrange
            menuUI.Count = 0;
            
            // Act
            CreateMenuUI();
            
            // Assert
            Assert.AreNotEqual(menuBackground,null);
            Assert.AreNotEqual(settingsButton,null);
            Assert.AreNotEqual(gameLogo,null);
            Assert.AreNotEqual(joinButton,null);
            Assert.AreNotEqual(hostButton,null);
            Assert.AreNotEqual(copyrightNotice,null);
        }
        
        public void CreateJoinUITest()
        {
            // Arrange
            joinUI.Count = 0;
            
            // Act
            CreateJoinUI();
            
            // Assert
            Assert.AreNotEqual(joinBackButton,null);
            Assert.AreNotEqual(qrCodeButton,null);
            Assert.AreNotEqual(playerNameBox,null);
            Assert.AreNotEqual(scanQRButton,null);
            Assert.AreNotEqual(joinLobbyButton,null);
        }
        
        public void CreateTableUITest()
        {
            // Arrange
            tableUI.Count = 0;
            
            // Act
            CreateTableUI();
            
            // Assert
            Assert.AreNotEqual(tableExitButton,null);
            Assert.AreNotEqual(gameRestartButton,null);
            Assert.AreNotEqual(bigBlindImage,null);
            Assert.AreNotEqual(smallBlindImage,null);
        }
        
        public void CreatePlayerUITest()
        {
            // Arrange
            playerUI.Count = 0;
            
            // Act
            CreatePlayerUI();
            
            // Assert
            Assert.AreNotEqual(balanceText,null);
            Assert.AreNotEqual(playerInfoText,null);
            Assert.AreNotEqual(playerExitButton,null);
            Assert.AreNotEqual(callButton,null);
            Assert.AreNotEqual(checkButton,null);
            Assert.AreNotEqual(raiseButton,null);
            Assert.AreNotEqual(allInButton,null);
            Assert.AreNotEqual(foldButton,null);
        }

        public void CreatePlayerRaiseUITest()
        {
            // Arrange
            playerUI_raise.Count = 0;
            
            // Act
            CreatePlayerRaiseUI();
            
            // Assert
            Assert.AreNotEqual(raiseExitButton,null);
            Assert.AreNotEqual(currentBetText,null);
            Assert.AreNotEqual(increaseBetButton,null);
            Assert.AreNotEqual(decreaseBetButton,null);
            Assert.AreNotEqual(raiseCancelButton,null);
            Assert.AreNotEqual(raiseConfirmButton,null);
        }
        
        public void CreateSettingsUITest()
        {
            // Arrange
            settingsUI.Count = 0;
            
            // Act
            CreateSettingsUI();
            
            // Assert
            Assert.AreNotEqual(settingsExitButton,null);
            Assert.AreNotEqual(preferredHandText,null);
            Assert.AreNotEqual(leftHandToggleButton,null);
            Assert.AreNotEqual(rightHandToggleButton,null);
        }
        
        public void CreateLobbyUITest()
        {
            // Arrange
            lobbyUI.Count = 0;
            
            // Act
            CreateLobbyUI();
            
            // Assert
            Assert.AreNotEqual(lobbyBackButton,null);
            Assert.AreNotEqual(addressQRCode,null);
            Assert.AreNotEqual(lobbyMessageText,null);
            Assert.AreNotEqual(lobbyMessageText,null);
        }
    }
}
