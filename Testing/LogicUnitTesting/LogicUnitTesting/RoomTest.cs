using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicUnitTesting
{
    [TestClass]
    public class RoomTest
    {
        List<Players> players = new List<Player> {("John", new Socket(SocketType.Stream, ProtocolType.Tcp)),("Jackson", new Socket(SocketType.Stream, ProtocolType.Tcp)),("Mark", new Socket(SocketType.Stream, ProtocolType.Tcp)),("Julia", new Socket(SocketType.Stream, ProtocolType.Tcp))};
        
        [TestMethod]
        public void addPlayerTest()
        {
            // Arrange
            Player player = new Player("Krystal", new Socket(SocketType.Stream, ProtocolType.Tcp));
            List<Players> expected = new List<Player> {("John", new Socket(SocketType.Stream, ProtocolType.Tcp)),("Jackson", new Socket(SocketType.Stream, ProtocolType.Tcp)),("Mark", new Socket(SocketType.Stream, ProtocolType.Tcp)),("Julia", new Socket(SocketType.Stream, ProtocolType.Tcp)),("Krystal", new Socket(SocketType.Stream, ProtocolType.Tcp))};
            
            // Act
            players.Add(player);
            
            // Assert
            Assert.AreEqual(players,expected);
        }
        
        public void removePlayerTest1()
        {
            // Arrange
            List<Players> expected = new List<Player> {("John", new Socket(SocketType.Stream, ProtocolType.Tcp)),("Mark", new Socket(SocketType.Stream, ProtocolType.Tcp)),("Julia", new Socket(SocketType.Stream, ProtocolType.Tcp))};
            
            // Act
            players.RemoveAt(1);
            
            // Assert
            Assert.AreEqual(players,expected);
        }

        public void getRoomSizeTest()
        {
            // Act
            actual = player.getRoomSize();
            
            // Assert
            Assert.AreEqual(actual,4);
        }
        
        public void removePlayerTest2()
        {
            // Arrange
            Player player = new Player("John", new Socket(SocketType.Stream, ProtocolType.Tcp));
            List<Players> expected = new List<Player> {("Jackson", new Socket(SocketType.Stream, ProtocolType.Tcp)),("Mark", new Socket(SocketType.Stream, ProtocolType.Tcp)),("Julia", new Socket(SocketType.Stream, ProtocolType.Tcp))};
            
            // Act
            players.Remove(player);
            
            // Assert
            Assert.AreEqual(players,expected);
        }
        
        public void getNextPlayeTest1()
        {
            // Arrange
            int index = 0;
            players[0].hasFolded() = false;
            players[1].hasFolded() = false;
            players[2].hasFolded() = false;
            players[3].hasFolded() = false;
            Player expected = new Player("Jackson", new Socket(SocketType.Stream, ProtocolType.Tcp));
            
            // Act
            players.getNextPlayer();
            
            // Assert
            Assert.AreEqual(currentPlayer, expected);
        }
        
        public void getNextPlayeTest2()
        {
            // Arrange
            int index = 0;
            players[0].hasFolded() = false;
            players[1].hasFolded() = true;
            players[2].hasFolded() = false;
            players[3].hasFolded() = false;
            Player expected = new Player("Mark", new Socket(SocketType.Stream, ProtocolType.Tcp));
            
            // Act
            players.getNextPlayer();
            
            // Assert
            Assert.AreEqual(currentPlayer, expected);
        }
        
        public void getNextPlayeTest3()
        {
            // Arrange
            int index = 1;
            players[0].hasFolded() = false;
            players[1].hasFolded() = false;
            players[2].hasFolded() = true;
            players[3].hasFolded() = true;
            Player expected = new Player("John", new Socket(SocketType.Stream, ProtocolType.Tcp));
            
            // Act
            players.getNextPlayer();
            
            // Assert
            Assert.AreEqual(currentPlayer, expected);
        }
        
        public void rotatePlayersTest()
        {
            // Arrange
            List<Players> expected = new List<Player> {("Jackson", new Socket(SocketType.Stream, ProtocolType.Tcp)),("Mark", new Socket(SocketType.Stream, ProtocolType.Tcp)),("Julia", new Socket(SocketType.Stream, ProtocolType.Tcp)),("John", new Socket(SocketType.Stream, ProtocolType.Tcp))};
            
            // Act
            players.rotatePlayers();
            
            // Assert
            Assert.AreEqual(player,expected);
        }
        
        public void getIndexTest1()
        {
            // Arrange
            int index = 1;
            
            // Act
            getIndex(true);
            
            // Assert
            Assert.AreEqual(index,2);
        }
        
        public void getIndexTest2()
        {
            // Arrange
            int index = 1;
            
            // Act
            getIndex(false);
            
            // Assert
            Assert.AreEqual(index,1);
        }
        
        public void getIndexTest3()
        {
            // Arrange
            int index = 4;
            
            // Act
            getIndex(true);
            
            // Assert
            Assert.AreEqual(index,1);
        }
        
        public void getIndexTest4()
        {
            // Arrange
            int index = 4;
            
            // Act
            getIndex(false);
            
            // Assert
            Assert.AreEqual(index,0);
        }
        
        public void countFoldedTest()
        {
            // Arrange
            players[0].hasFolded() = false;
            players[1].hasFolded() = true;
            players[2].hasFolded() = true;
            players[3].hasFolded() = false;
            
            // Act
            players.countFolded();
            
            // Assert
            Asser.AreEqual(count,2);
        }
    }
}
