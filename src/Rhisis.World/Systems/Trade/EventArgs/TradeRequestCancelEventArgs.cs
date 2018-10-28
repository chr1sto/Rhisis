using Rhisis.World.Game.Core.Systems;

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
