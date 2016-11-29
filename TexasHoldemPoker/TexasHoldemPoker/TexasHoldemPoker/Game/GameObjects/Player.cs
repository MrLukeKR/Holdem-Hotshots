using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using TexasHoldemPoker.Game.PokerObjects;

namespace MixedRealityPoker.Game.PokerObjects
{
    class Player
    {
        private uint id;
        private String name;
        private String connection;
        private uint chips;
        private Card[] hand;

        public Player(uint id)
        {
            hand = new Card[2];
            this.id = id;
            name = "Player " + id;
        }

        public Player(uint id, String name)
        {
            hand = new Card[2];
            this.id = id;
            this.name = name;
        }

        public Player(uint id, Socket connection) //This will be broken until "String" is changed to the appropriate socket type
        {
            this.id = id;
            this.connection = connection;
        }
        
        public Player(uint id, String name, Socket connection)
        {
            this.id = id;
            this.name = name;
            this.connection = connection;
        }

        public String getName()
        {
            return name;
        }

        public uint getID()
        {
            return id;
        }
    }
}
