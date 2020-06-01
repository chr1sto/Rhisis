namespace Rhisis.World.Game.Components
{
    public sealed class TradeComponent
    {
        public enum TradeConfirm { Error, Ok }

        public enum TradeState { Item, Ok, Confirm }

        private readonly int _maxTradeItemsCapacity;

        /// <summary>
        /// Gets or sets the target trader id.
        /// </summary>
        public uint TargetId { get; set; }

        /// <summary>
        /// Gets or sets the current items for trade.
        /// </summary>
        public ItemContainerComponent Items { get; private set; }

        /// <summary>
        /// Gets the number of items to trade.
        /// </summary>
        public ushort ItemCount { get; set; }

        /// <summary>
        /// Gets the amount of gold to trade.
        /// </summary>
        public int Gold { get; set; }

        /// <summary>
        /// Exchange state/progress
        /// </summary>
        public TradeState State { get; set; } = TradeState.Item;

        /// <summary>
        /// Gets a value that indicates if the current entity is already trading.
        /// </summary>
        public bool IsTrading => TargetId != 0;

        /// <summary>
        /// Creates a new <see cref="TradeComponent"/> instance.
        /// </summary>
        /// <param name="maxItemCapacity">Maximum amount of items in trade window.</param>
        public TradeComponent(int maxItemCapacity)
        {
            _maxTradeItemsCapacity = maxItemCapacity;
            Reset();
        }

        /// <summary>
        /// Resets the trade component.
        /// </summary>
        public void Reset()
        {
            TargetId = 0;
            ItemCount = 0;
            Gold = 0;
            State = TradeState.Item;
            Items = new ItemContainerComponent(_maxTradeItemsCapacity);
        }
    }
}
