using HoldemHotshots.Managers;
using HoldemHotshots.GameLogic.Player;
using HoldemHotshots.Networking.ServerNetworkEngine;
using System;
using System.Collections.Generic;
using System.Threading;
using Urho;
using Urho.Actions;
using Urho.Audio;
using HoldemHotshots.Utilities;

namespace HoldemHotshots.GameLogic
{

    class Table
    {
        private Deck deck = new Deck();
        public List<Card> hand { get; } = new List<Card>();
        private Pot pot = new Pot(50, 100);
        Node soundnode;
        SoundSource sound;

        private Room room { get;  set; }
        
        public Table(Room room)
        {
            setRoom(room);
            initSound();
            ServerCommandManager.SetPot(pot);
            foreach (ServerPlayer player in room.players)
                player.pot = pot;
            deck.Shuffle();
        }
        
        internal void ResetTable()
        {
            foreach (ServerPlayer player in room.players)
                player.Reset();
        }

        private void initSound()
        {
                soundnode = SceneManager.hostScene.GetChild("SFX", true);
                sound = soundnode.GetComponent<SoundSource>(true);
        }
        
        public void Flop() {
            for (int i = 0; i < 3; i++)
            {
                dealToTable(i);
                Thread.Sleep(500);
            }
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

        public void applyBlinds()
        {
            ServerPlayer smallBlindPlayer = room.players[0];
            ServerPlayer bigBlindPlayer   = room.players[1];
            
            pot.PayIn(smallBlindPlayer.ApplyBlind(pot.smallBlind), pot.smallBlind);
            pot.PayIn(bigBlindPlayer.ApplyBlind(pot.bigBlind), pot.bigBlind);

            smallBlindPlayer.DisplayMessage("Paid Small Blind");
            bigBlindPlayer.DisplayMessage("Paid Big Blind");

            SceneUtils.UpdatePlayerInformation(smallBlindPlayer.name, "Paid Small Blind");
            SceneUtils.UpdatePlayerInformation(bigBlindPlayer.name, "Paid Big Blind");

            foreach (ServerPlayer player in room.players)
                player.connection.setHighestBid(pot.stake);
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

        private bool nextRoundCheck(int remainingPlayers)
        {
            Console.WriteLine("POT STAKE: " + pot.stake);

            if (room.GetRemainingPlayers() == 1)
                return true;

            foreach (ServerPlayer player in room.players)
                if (!player.folded)
                {
                    Console.WriteLine(player.name + " STAKE: " + player.currentStake);
                    
                    if (player.currentStake != pot.stake)
                        return false;
                }

            if (pot.stake == 0 && remainingPlayers > 0)
                return false;
            else
                return true;
        }

        public void placeBets() {
            ServerPlayer currentPlayer = null;
            do {
                for (int i = 0; i < room.players.Count; i++)
                {
                    if (room.GetRemainingPlayers() > 1)
                    {
                        currentPlayer = room.players[i];
                        currentPlayer.TakeTurn();

                        while (!currentPlayer.hasTakenTurn && !currentPlayer.folded)
                            Thread.Sleep(1000);

                        currentPlayer.hasTakenTurn = false;    
                    }

                    if (nextRoundCheck(room.players.Count - i - 1))
                        break;
                }

            } while(!nextRoundCheck(0));

            pot.ResetStake();

            foreach (ServerPlayer player in room.players)
                player.ResetStake();
            
        }

        public void showdown() {
            //TODO: Display player names around table with what their hand is worth

            var winners = CardRanker.evaluateGame(this, room.players);
            var winnings = pot.cashout();
            double winningsPerPlayer = winnings / winners.Count;
            string winnerText = "";
            string hand = "";

            if (winningsPerPlayer % 1 != 0) //If the winnings can't be split, leave the remainder in the opt
            {
                winningsPerPlayer = Math.Floor(winningsPerPlayer);
                pot.leaveRemainder();
            }
            
            foreach (ServerPlayer winner in winners)
            {
                winner.DisplayMessage("You Win!");
                winner.GiveChips((uint)winningsPerPlayer);
                winnerText += winner.name;
            }
        }

    internal void setRoom(Room room) { this.room = room; }
  }
}
