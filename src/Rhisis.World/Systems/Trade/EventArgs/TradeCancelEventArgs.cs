using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.Trade.EventArgs
{
    public class TradeCancelEventArgs : SystemEventArgs
    {
        public int Mode { get; }

        public TradeCancelEventArgs(int mode)
        {
            Mode = mode;
        }

        public override bool CheckArguments() => true;
    }
}
