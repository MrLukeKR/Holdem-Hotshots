using System;
using System.Collections.Generic;
using System.Text;

namespace TexasHoldemPoker.Game.NetworkEngine
{
    class NetworkEngineFactory
    {


#if __ANDROID__
        public static NetworkEngineInterface create()
        {

            //Create Android network engine
            return null; // NEEDS TO RETURN A NETWORKENGINEINTERFACE!
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
