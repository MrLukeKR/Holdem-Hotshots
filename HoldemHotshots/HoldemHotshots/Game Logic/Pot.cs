using HoldemHotshots.Managers;
using HoldemHotshots.Networking.ServerNetworkEngine;
using HoldemHotshots.Utilities;
using System;
using Urho;
using Urho.Audio;

namespace HoldemHotshots.GameLogic
{
    class Pot
    {
        public uint amount { get; private set; } = 0;
        public uint smallBlind { get; set; }
        public uint bigBlind { get; set; }
        public uint stake { get; private set;}
        Node soundnode;
        SoundSource sound;

        public Pot(uint smallBlind, uint bigBlind)
        {
            this.smallBlind = smallBlind;
            this.bigBlind = bigBlind;

            initSound();
        }

        private void initSound()
        {
            soundnode = SceneManager.hostScene.GetChild("SFX", true);
            sound = soundnode.GetComponent<SoundSource>(true);
        }

        public void ResetStake()
        {
            stake = 0;
        }

        public void PayIn(uint amount,uint playerStake)
        {
            this.amount += amount;

            if (playerStake > stake)
                stake = playerStake;
            
            var fileNo = (new Random().Next() % 4) + 1;

            Application.InvokeOnMain(new Action(() =>
            {
                sound.Play(UIManager.cache.GetSound("Sounds/CoinDrop" + fileNo + ".wav"));
                SceneUtils.UpdatePotBalance(this.amount);
            }
            ));
        }
        
        public uint cashout()
        {
            uint jackpot = amount;
            amount = 0;
            SceneUtils.UpdatePotBalance(this.amount);

            sound.Play(UIManager.cache.GetSound("Sounds/Jackpot.wav"));
            return jackpot;
        }

        internal void leaveRemainder()
        {
            amount = 1;
        }
    }
}
