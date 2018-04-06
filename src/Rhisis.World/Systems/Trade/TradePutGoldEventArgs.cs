using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Systems.Trade
{
    public class TradePutGoldEventArgs : TradeEventArgs
    {
        public uint Gold { get; private set; }

        public TradePutGoldEventArgs(uint gold)
        {
            Gold = gold;
        }
    }
}
