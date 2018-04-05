using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Systems.Trade
{
    public class TradeRequestEventArgs : TradeEventArgs
    {
        public int TargetId { get; private set; }

        public TradeRequestEventArgs(int targetId)
        {
            TargetId = targetId;
        }

        public override bool CheckArguments()
        {
            return TargetId != 0;
        }
    }
}
