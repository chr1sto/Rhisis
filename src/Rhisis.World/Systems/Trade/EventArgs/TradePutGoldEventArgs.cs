using Rhisis.World.Game.Core.Systems;

namespace Rhisis.World.Systems.Trade.EventArgs
{
    public class TradePutGoldEventArgs : SystemEventArgs
    {
        public int Gold { get; }

        public TradePutGoldEventArgs(int gold)
        {
            Gold = gold;
        }

        public override bool GetCheckArguments()
        {
            return Gold > 0;
        }
    }
}
