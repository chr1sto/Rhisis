using System;
using System.Collections.Generic;
using System.Text;
using Rhisis.World.Systems.Inventory;

namespace Rhisis.World.Systems.Trade
{
    public class TradePutEventArgs : TradeEventArgs
    {
        public byte Slot { get; private set; }

        public byte ItemType { get; private set; }

        public byte ItemId { get; private set; }

        public short Count { get; private set; }

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
