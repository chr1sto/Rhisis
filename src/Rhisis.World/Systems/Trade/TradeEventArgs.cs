using System;
using Rhisis.World.Game.Core;
using Rhisis.World.Systems.Statistics;

namespace Rhisis.World.Systems.Trade
{
    public class TradeEventArgs : SystemEventArgs
    {
        public override bool CheckArguments() => true;

        public TradeEventArgs() :
            base(null)
        {
        }
    }
}