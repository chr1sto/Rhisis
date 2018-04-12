using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Systems.Trade
{
    public class TradeCancelEventArgs : TradeEventArgs
    {
        public readonly int Mode;

        public TradeCancelEventArgs(int mode)
        {
            Mode = mode;
        }
    }
}
