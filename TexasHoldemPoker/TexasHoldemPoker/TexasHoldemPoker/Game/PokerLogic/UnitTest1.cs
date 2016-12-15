using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class Test1
    {
        [TestMethod]
        public void methodTest()
        {
            Player player = new Player("Billy", new Socket(SocketType.Stream, ProtocolType.Tcp));
            player.chips = 30;
            var.result1 = takechips(20);
            var.result2 = takechips(40);
            var expect1 = 10;
            var expect2 = 0;
            Assert.True(result == expect1 && result2 == expect2);
        }
    }
}
