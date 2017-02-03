using System;
using System.Collections.Generic;
using System.Text;
namespace TexasHoldemPoker.Game.NetworkEngine
{
    interface SessionInterface
    {
        public static Session getinstance();
        public void init();
    }
}
