namespace Rhisis.World.Systems.Events.Inventory
{
    public class InventoryCreateItemEventArgs : InventoryEventArgs
    {
        /// <summary>
        /// Gets the item id to create.
        /// </summary>
        public int ItemId { get; }

        /// <summary>
        /// Gets the item quantity.
        /// </summary>
        public int Quantity { get; }

        /// <summary>
        /// Gets the item creator id.
        /// </summary>
        public int CreatorId { get; }

        /// <summary>
        /// Creates a new <see cref="InventoryCreateItemEventArgs"/> instance.
        /// </summary>
        /// <param name="itemId">Item id</param>
        /// <param name="quantity">Item quantity</param>
        /// <param name="creatorId">Item creator id</param>
        public InventoryCreateItemEventArgs(int itemId, int quantity, int creatorId)
            : base(InventoryActionType.CreateItem, itemId, quantity, creatorId)
        {
            this.ItemId = itemId;
            this.Quantity = quantity;
            this.CreatorId = creatorId;
        }

        public override string ToString()
        {
            return $"Create item: {this.ItemId}; Quantity:{this.Quantity}; Creator: {this.CreatorId}";
        }
    }
}
