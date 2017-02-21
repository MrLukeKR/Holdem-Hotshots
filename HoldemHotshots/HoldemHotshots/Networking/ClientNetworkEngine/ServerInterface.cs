using System;
using System.Collections.Generic;
using System.Text;

namespace HoldemHotshots
{
    public interface ServerInterface
    {
        void sendFold();
        void sendRaise(uint amount);
        void sendCheck();
        void sendAllIn();
        void sendCall();
    }
}
