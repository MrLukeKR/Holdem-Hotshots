using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedRealityPoker.Game.GameEngine
{
    class Core
    {
        private static Boolean isRunning = false;

        public void start()
        {
            if (!isRunning)
                run();
        }

        public static void run()
        {
            isRunning = true;

            //TODO: Frame rate init stuff here

            while( isRunning )
            {
                //TODO: GAME LOOP - Limit the update intervals using frame rate
            }
        }

        public void stop()
        {
            isRunning = false;
        }
    }
}
