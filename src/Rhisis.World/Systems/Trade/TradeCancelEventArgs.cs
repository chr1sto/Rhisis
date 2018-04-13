using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Systems.Trade
{
    public class TradeCancelEventArgs : TradeEventArgs
    {
        public int Mode { get; }

        public TradeCancelEventArgs(int mode)
        {
            Mode = mode;
        }
    }
}
