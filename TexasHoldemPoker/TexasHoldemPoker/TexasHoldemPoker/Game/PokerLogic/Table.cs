﻿
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

            
            Console.WriteLine(pokerRoom.ToString());
        }

        internal List<Card> getCards()
        {
            return hand;
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
            pokerRoom.getNextPlayer().payBlind(false);
            pokerRoom.getNextPlayer().payBlind(true);
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
