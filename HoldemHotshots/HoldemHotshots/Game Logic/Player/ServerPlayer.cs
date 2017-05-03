using HoldemHotshots.Networking.ServerNetworkEngine;
using System.Collections.Generic;

namespace HoldemHotshots.GameLogic.Player
{
    /// <summary>
    /// Stores player information on the server-side
    /// </summary>
    public class ServerPlayer
    {
        public uint chips { get; private set; }
        /// <summary>
        /// Stores the player's stake (chips input into the pot)
        /// </summary>
        public uint currentStake { get; private set;}
        public List<Card> hand { get; private set; } = new List<Card>();
        public bool folded { get; private set; } = false;
        public bool hasTakenTurn = false;
        public string name;
        public string originalName;
        public ClientInterface connection;

        internal Pot pot { private get; set; }
        
        public ServerPlayer(ClientInterface connection)
        {
            this.connection = connection;
            GiveChips(1000); //TODO: Allow players to "purcahse" chips
        }
        
        /// <summary>
        /// Gives the player the given amount of chips and adds it to their balance
        /// </summary>
        /// <param name="amount">Amount of chips</param>
        public void GiveChips(uint amount)
        {
            chips += amount;
            connection.SetChips(chips);
        }

        /// <summary>
        /// Sets the current stake to 0
        /// </summary>
        public void ResetStake()
        {
            currentStake = 0;
        }
        
        /// <summary>
        /// Takes a given amount of chips away from the player's balance
        /// </summary>
        /// <param name="amount">Amount to take away</param>
        /// <returns>Amount taken away (used as validation)</returns>
        public uint TakeChips(uint amount)
        {
            if (chips >= amount)
            {
                chips -= amount;
                currentStake += amount;
                connection.SetChips(chips);
                connection.SetPlayerBid(currentStake);

                return amount;
            }
            else
                return 0;
        }

        /// <summary>
        /// Removes the cards from the player's hand and sends a reset command to the client
        /// </summary>
        internal void Reset()
        {
            hand.Clear();  

            if(chips > 0)
                folded = false;
            connection.ResetInterface();
        }

        /// <summary>
        /// Forces a player to pay a certain amount as a blind
        /// </summary>
        /// <param name="amount">Amount to take from the player</param>
        /// <returns>Amount taken from the player (used as validation)</returns>
        public uint ApplyBlind(uint amount)
        {
            if (chips >= amount)
                return TakeChips(amount);
            else
                Fold();
            return 0;
        }

        /// <summary>
        /// Used to determine if the client is still connected to the server
        /// </summary>
        /// <returns>Connection status</returns>
        internal bool IsConnected()
        {
            return connection.IsConnected();
        }

        /// <summary>
        /// Checks if the player can take their turn and then sends the "Take Turn" command to the client
        /// </summary>
        public void TakeTurn()
        {
            if (!folded)
            {
                SetHighestBid();
                connection.TakeTurn();
            }
        }

        public void SetHighestBid()
        {
            connection.SetHighestBid(pot.stake);
        }

        /// <summary>
        /// Forcefully removes a player from the game
        /// </summary>
        internal void Kick()
        {
            connection.SendKicked();
        }
        
        /// <summary>
        /// Gives the player a card (unlike the client, this is the ACTUAL card from the server deck)
        /// </summary>
        /// <param name="card">Card to give to the player</param>
        internal void GiveCard(Card card)
        {
            hand.Add(card);
            connection.GiveCard((int)card.suit, (int)card.rank);
        }

        /// <summary>
        /// Sends a command to the client to animate the card
        /// </summary>
        /// <param name="index"></param>
        internal void AnimateCard(int index)
        {
            connection.AnimateCard(index);
        }

        /// <summary>
        /// Folds the player so that they can't take any more turns this round
        /// </summary>
        internal void Fold()
        {
            folded = true;
            hasTakenTurn = true;
        }

        /// <summary>
        /// Displays a message on the client's screen
        /// </summary>
        /// <param name="message">Message to display</param>
        internal void DisplayMessage(string message)
        {
            connection.DisplayMessage(message);
        }
    }
}