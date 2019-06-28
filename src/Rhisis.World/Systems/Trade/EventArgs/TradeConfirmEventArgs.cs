using Rhisis.World.Game.Core.Systems;

namespace Rhisis.World.Systems.Trade.EventArgs
{
    public class TradeConfirmEventArgs : SystemEventArgs
    {
        public override bool GetCheckArguments()
        {
            return true;
        }
    }
}
