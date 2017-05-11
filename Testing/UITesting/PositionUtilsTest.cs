using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UITesting
{
    [TestClass]
    public class PositionUtilsTest
    {
        [TestMethod]
        public void GetScreenToWorldPointTest1()
        {
            // Assert
            int x = 20;
            int y = 10;
            float z = 3.0;
            var camera = CameraNode.GetComponent<Camera>();
            
            // Act
            GetScreenToWorldPoint(int x, int y, float z, Camera camera);
            
            // Assert
            Assert.AreNotEqual(a,null);
        }
        
        public void GetScreenToWorldPointTest2()
        {
            // Assert
            float z = 3.0;
            var camera = CameraNode.GetComponent<Camera>();
            
            // Act
            GetScreenToWorldPoint(IntVector2 ScreenPos, float z, Camera camera);
            
            // Assert
            Assert.AreNotEqual(a,null);
        }
    }
}
