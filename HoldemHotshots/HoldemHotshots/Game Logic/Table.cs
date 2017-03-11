﻿using HoldemHotshots.Managers;
using HoldemHotshots.GameLogic.Player;
using HoldemHotshots.Networking.ServerNetworkEngine;
using System;
using System.Collections.Generic;
using System.Threading;
using Urho;
using Urho.Actions;
using Urho.Audio;

namespace HoldemHotshots.GameLogic
{

    class Table
    {
        private Deck deck = new Deck();
        public List<Card> hand { get; } = new List<Card>();
        private Pot pot = new Pot(0, 0);
        Node soundnode;
        SoundSource sound;

        private Room room { get;  set; }
        
        public Table(Room room)
        {
            setRoom(room);
            initSound();
            ServerCommandManager.SetPot(pot);
            foreach (ServerPlayer player in room.players) player.pot = pot;
            deck.Shuffle();
        }
        
        internal void ResetTable()
        {
            foreach (Card card in hand)
                SceneManager.hostScene.RemoveChild(card.node);

            hand.Clear();

            foreach (ServerPlayer player in room.players)
                player.Reset();

            deck = new Deck();
            deck.Shuffle();
        }

        private void initSound()
        {
                soundnode = SceneManager.hostScene.GetChild("SFX", true);
                sound = soundnode.GetComponent<SoundSource>(true);
        }
        
        public void Flop() {
            for (int i = 0; i < 3; i++)
                dealToTable(i);
        }

        public void dealToTable(int index)
        {
            Card newCard;
            Node newCardNode;

            hand.Add(deck.TakeCard());
            newCard = hand[index];
            newCardNode = newCard.node;
            newCardNode.Position = Card.CARD_TABLE_DEALING_POSITION;

            doAnimation(index, newCard, newCardNode);
        }

        private void doAnimation(int index, Card newCard, Node newCardNode)
        {
            Application.InvokeOnMain(new Action(() =>
            {
                SceneManager.hostScene.AddChild(newCardNode);
                animateCardDeal(index, newCard);
            }
            ));
        }

        private void animateCardDeal(int index, Card card)
        {
            Console.WriteLine(card.ToString());
            card.node.RunActions(new Parallel(new RotateBy(0f, 0, 0, 90), new MoveTo(0.1f, Card.CARD_TABLE_POSITIONS[index])));
            sound.Play(UIManager.cache.GetSound("Sounds/Swish.wav"));
            //Need to add this to some form of copyright message in the App: http://www.freesfx.co.uk
        }
        
        public void dealToPlayers(){
            ServerPlayer currPlayer = null;
            for (int i = 0; i < room.players.Count; i++)
            {
                currPlayer = room.players[i];
                Console.WriteLine("Dealing card to " + currPlayer.name);
                currPlayer.GiveCard(deck.TakeCard());
            }
        }

    public void placeBets() {
            Console.WriteLine("Room size is " + room.players.Count);
            ServerPlayer currentPlayer = null;
            for (int i = 0; i < room.players.Count; i++)
            {
                if (room.GetRemainingPlayers() > 1)
                {
                    currentPlayer = room.players[i];
                    currentPlayer.TakeTurn();

                    while (!currentPlayer.hasTakenTurn && !currentPlayer.folded) { Thread.Sleep(1000); }
                    currentPlayer.hasTakenTurn = false;
                }
            }
        }

    public void showdown() {
            var winners = CardRanker.evaluateGame(this, room.players);
            var winnings = pot.cashout();
            var winningsPerPlayer = winnings / winners.Count;

            if (winningsPerPlayer % 1 == 0) //IF the winnings can be split, payout, else........... (TODO)
                foreach (ServerPlayer winner in winners)
                {
                    winner.DisplayMessage("You Win!");
                    winner.GiveChips((uint)winningsPerPlayer);
                }
        }

    internal void setRoom(Room room) { this.room = room; }
  }
}
