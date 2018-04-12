using Rhisis.World.Game.Structures;
using Rhisis.World.Systems.Inventory;
using Rhisis.World.Systems.Trade;

namespace Rhisis.World.Game.Components
{
    public sealed class TradeComponent
    {
        public enum TradeConfirm { Error, Ok }

        public enum TradeState { Item, Ok, Confirm }

        public int TargetId { get; set; }

        public ItemContainerComponent Items { get; private set; }

        public ushort ItemCount { get; set; }

        public int Gold { get; set; }

        /// <summary>
        /// Exchange state/progress
        /// </summary>
        public TradeState State { get; set; }

        public TradeComponent()
        {
            this.TargetId = 0;
            State = TradeState.Item;
            Items = new ItemContainerComponent(TradeSystem.MaxTrade);
            ItemCount = 0;
            Gold = 0;
        }

        /// <summary>
        /// Gets the item at a specific slot
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public Item this[int slot]
        {
            get => Items[slot];
            set => Items[0] = value;
        }
    }
}
