using HoldemHotshots.Managers;
using HoldemHotshots.Utilities;
using System;
using Urho;
using Urho.Audio;

namespace HoldemHotshots.GameLogic
{
    /// <summary>
    /// Manages the chips used in the game and various values involved in balance based decisions
    /// </summary>
    class Pot
    {
        public uint amount { get; private set; } = 0;
        public uint smallBlind { get; set; }
        public uint bigBlind { get; set; }
        /// <summary>
        /// Used to find the current stake of the game (amount players have to pay in to progress the round)
        /// </summary>
        public uint stake { get; private set;}
        Node soundnode;
        SoundSource sound;

        public Pot(uint smallBlind, uint bigBlind)
        {
            this.smallBlind = smallBlind;
            this.bigBlind = bigBlind;

            InitSound();
        }

        /// <summary>
        /// Intialises the sound node of the scene
        /// </summary>
        private void InitSound()
        {
            soundnode = SceneManager.hostScene.GetChild("SFX", true);
            sound = soundnode.GetComponent<SoundSource>(true);
        }

        /// <summary>
        /// Sets the total stake to 0 
        /// </summary>
        public void ResetStake()
        {
            stake = 0;
        }

        /// <summary>
        /// Add a given amount of chips to the pot
        /// </summary>
        /// <param name="amount">Amount of chips</param>
        /// <param name="playerStake">The amount of chips in total that the player has paid in to the pot</param>
        public void PayIn(uint amount,uint playerStake)
        {
            this.amount += amount;

            if (playerStake > stake)
                stake = playerStake;
            
            var fileNo = (new Random().Next() % 4) + 1;

            Application.InvokeOnMain(new Action(() => sound.Play(UIManager.cache.GetSound("Sounds/CoinDrop" + fileNo + ".wav"))));

            SceneUtils.UpdatePotBalance(this.amount);
        }

        /// <summary>
        /// Resets the pot's balance to zero and returns the amount
        /// </summary>
        /// <returns>Amount of money in the pot</returns>
        public uint Cashout()
        {
            uint jackpot = amount;
            amount = 0;
            SceneUtils.UpdatePotBalance(this.amount);

           //sound.Play(UIManager.cache.GetSound("Sounds/Jackpot.wav"));
            return jackpot;
        }

        /// <summary>
        /// If the pot can't be split, the remainder is left in the pot
        /// </summary>
        internal void LeaveRemainder(uint remAmount)
        {
            amount = remAmount;
        }
    }
}
