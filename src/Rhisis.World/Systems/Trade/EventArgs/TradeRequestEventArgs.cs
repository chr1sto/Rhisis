using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.Trade.EventArgs
{
    public class TradeRequestEventArgs : SystemEventArgs
    {
        public int TargetId { get; }

        public TradeRequestEventArgs(int targetId)
        {
            TargetId = targetId;
        }

        public override bool CheckArguments() => TargetId > 0;
    }
}
