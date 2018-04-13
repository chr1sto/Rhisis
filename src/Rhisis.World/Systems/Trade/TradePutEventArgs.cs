using System;
using System.Collections.Generic;
using System.Text;
using Rhisis.World.Systems.Inventory;

namespace Rhisis.World.Systems.Trade
{
    public class TradePutEventArgs : TradeEventArgs
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

        public override bool CheckArguments()
        {
            return Slot > 0 && Slot < TradeSystem.MaxTrade &&
                   Count > 0;
        }
    }
}
