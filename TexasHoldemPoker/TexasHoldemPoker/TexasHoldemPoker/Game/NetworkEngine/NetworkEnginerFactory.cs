using System;
using System.Collections.Generic;
using System.Text;

namespace TexasHoldemPoker.Game.NetworkEngine
{
    class NetworkEnginerFactory
    {


#if __ANDROID__
        public static NetworkEngineInterface create()
        {

            //Create Android network engine

        }

#endif

#if __IOS__

        public static NetworkEngineInterface create()
        {

            //Create IOS network engine




        }


#endif
    }
}
