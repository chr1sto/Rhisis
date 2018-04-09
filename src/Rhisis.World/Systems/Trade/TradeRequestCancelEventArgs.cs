using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Systems.Trade
{
    public class TradeRequestCancelEventArgs : TradeEventArgs
    {
        public int TargetId { get; private set; }

        public TradeRequestCancelEventArgs(int targetId)
        {
            TargetId = targetId;
        }

        public override bool CheckArguments()
        {
            return TargetId != 0;
        }
    }
}
