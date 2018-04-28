using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.Trade.EventArgs
{
    public class TradePutEventArgs : SystemEventArgs
    {
        public byte Slot { get; }

        public byte ItemType { get; }

        public byte ItemId { get; }

        public short Count { get; }

        public TradePutEventArgs(byte position, byte itemType, byte itemId, short count)
        {
            Slot = position;
            ItemType = itemType;
            ItemId = itemId;
            Count = count;
        }

        public override bool CheckArguments() => Slot < TradeSystem.MaxTrade &&
                                                 Count > 0;
    }
}
