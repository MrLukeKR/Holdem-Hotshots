
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace PokerLogic
{
    class Table
    {
        private Deck deck = new Deck();
        private List<Card> hand = new List<Card>();
        private Pot pot = new Pot();
        private Room pokerRoom = new Room();
        int index = 0;

        public Table()
        {

        }

        public void populate()
        {
            pokerRoom.addPlayer(new Player("Billy", new Socket(SocketType.Stream, ProtocolType.Tcp)));
            pokerRoom.addPlayer(new Player("Bob", new Socket(SocketType.Stream, ProtocolType.Tcp)));
            pokerRoom.addPlayer(new Player("Barry", new Socket(SocketType.Stream, ProtocolType.Tcp)));
            pokerRoom.addPlayer(new Player("Gertrude", new Socket(SocketType.Stream, ProtocolType.Tcp)));
            pokerRoom.addPlayer(new Player("Gretchen", new Socket(SocketType.Stream, ProtocolType.Tcp)));
            pokerRoom.addPlayer(new Player("Gwendoline", new Socket(SocketType.Stream, ProtocolType.Tcp)));

            Console.WriteLine(pokerRoom.ToString());
        }

        public void init(uint startBalance, uint smallBlind, uint bigBlind)
        {
            pot.setSmallBlind(smallBlind);
            pot.setBigBlind(bigBlind);

            for (int i = 0; i < pokerRoom.getRoomSize(); i++)
                pokerRoom.getNextPlayer().giveChips(startBalance);
        }

        internal void placeBets()
        {
            Player currentPlayer;
            for (int i = index; i < pokerRoom.getRoomSize() - pokerRoom.countFolded(); i++)
            {
                currentPlayer = pokerRoom.getNextPlayer();
                currentPlayer.takeTurn();
                incrementIndex();
            }
        }

        internal void dealToTable()
        {
            hand.Add(deck.takeCard());
        }

        internal int getRemainingPlayers()
        {
            return pokerRoom.countFolded();
        }

        public void dealBlinds()
        {
            pokerRoom.getNextPlayer().payBlind(pot, true);
            pokerRoom.getNextPlayer().payBlind(pot, false);
            index = 2;
        }

        public void flop()
        {
            for (int i = 0; i < 3; i++)
                hand.Add(deck.takeCard());
        }

        public void dealToPlayers()
        {
            Player currentPlayer;
            for(int card = 0; card < 2; card++)
              for (int i = 0; i < pokerRoom.getRoomSize() - pokerRoom.countFolded(); i++)
             {
                 currentPlayer = pokerRoom.getNextPlayer();
                 if (!currentPlayer.hasFolded())
                     currentPlayer.giveCard(deck.takeCard());
             }
        }

        private void incrementIndex()
        {
            index++;
            if (index == pokerRoom.getRoomSize())
                index = 0;
        }

        public void printHand()
        {
            Console.WriteLine("Table Cards:\n");
            for (int i = 0; i < hand.Count; i++)
                Console.WriteLine(hand[i].ToString());
            Console.WriteLine();
        }
    }
}
