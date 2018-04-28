using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.Trade.EventArgs
{
    public class TradeBeginEventArgs : SystemEventArgs
    {
        public int TargetId { get; }

        public TradeBeginEventArgs(int targetId)
        {
            TargetId = targetId;
        }

        public override bool CheckArguments() => TargetId > 0;
    }
}
