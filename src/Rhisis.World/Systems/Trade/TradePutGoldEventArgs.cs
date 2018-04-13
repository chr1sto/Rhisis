using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Systems.Trade
{
    public class TradePutGoldEventArgs : TradeEventArgs
    {
        public int Gold { get; }

        public TradePutGoldEventArgs(int gold)
        {
            Gold = gold;
        }
    }
}
