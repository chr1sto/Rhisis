using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.Trade.EventArgs
{
    public class TradePutGoldEventArgs : SystemEventArgs
    {
        public int Gold { get; }

        public TradePutGoldEventArgs(int gold)
        {
            Gold = gold;
        }

        public override bool CheckArguments() => Gold > 0;
    }
}
