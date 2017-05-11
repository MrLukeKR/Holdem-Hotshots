using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UITesting
{
    [TestClass]
    public class UIUtilsTest
    {
        [TestMethod]
        public void FindUIElementTest1()
        {
            // Assert
            string elementName = "raise"
            List<UIElement> uiToSearch = new List<UIElement> {"raise","check","all-in","fold"};
            
            // Act
            actual = FindUIElement(string elementName, List<UIElement> uiToSearch);
            
            // Assert
            Assert.AreEqual(actual,"raise");
        }
        
        public void FindUIElementTest2()
        {
            // Assert
            string elementName = "raise"
            List<UIElement> uiToSearch = new List<UIElement> {"check","all-in","fold"};
            
            // Act
            actual = FindUIElement(string elementName, List<UIElement> uiToSearch);
            
            // Assert
            Assert.AreEqual(actual,null);
        }
        
        public void GetPlayerNameTest()
        {
            // Arrange
            LineEdit playerName = null;
            
            // Act
            string actual = GetPlayerName();
            
            // Assert
            Assert.Equal(actual,"Unknown Player");
        }
    }
}
