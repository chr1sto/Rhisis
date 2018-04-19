using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.Trade.EventArgs
{
    public class TradeRequestCancelEventArgs : SystemEventArgs
    {
        public int TargetId { get; }

        public TradeRequestCancelEventArgs(int targetId)
        {
            TargetId = targetId;
        }

        public override bool CheckArguments() => TargetId > 0;
    }
}
