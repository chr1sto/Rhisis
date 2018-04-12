using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Systems.Trade
{
    public class TradePutGoldEventArgs : TradeEventArgs
    {
        public readonly int Gold;

        public TradePutGoldEventArgs(int gold)
        {
            Gold = gold;
        }
    }
}
