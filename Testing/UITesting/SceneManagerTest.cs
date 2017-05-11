using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UITesting
{
    [TestClass]
    public class SceneManagerTest
    {
        [TestMethod]
        public void CreateMenuSceneTest()
        {
            // Act
            CreateMenuScene();
            
            // Assert
            Assert.AreNotEqual(cameraNode,null);
            Assert.AreEqual(cameraNode.Name,"MainCamera");
        }
        
        public void CreatePlaySceneTest()
        {
            // Act
            CreatePlayScene();
            
            // Assert
            Assert.AreNotEqual(cameraNode,null);
            Assert.AreNotEqual(soundNode,null);
            Assert.AreNotEqual(lightNode,null);
            Assert.AreNotEqual(light,null);
        }
        
        public void CreateHostSceneTest()
        {
            // Act
            CreateHostScene();
            
            // Assert
            Assert.AreNotEqual(cameraNode,null);
            Assert.AreNotEqual(soundNode,null);
            Assert.AreNotEqual(lightNode,null);
            Assert.AreNotEqual(light,null);
            Assert.AreNotEqual(potNode,null);
            Assert.AreNotEqual(pot,null);
            Assert.AreNotEqual(messageNode,null);
            Assert.AreNotEqual(message,null);
        }
    }
}
