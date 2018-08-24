using System;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.Inventory.EventArgs
{
    public class InventoryCreateItemEventArgs : SystemEventArgs
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
        /// Gets the data of the item to create.
        /// </summary>
        public ItemData ItemData { get; private set; }

        /// <inheritdoc />
        /// <summary>
        /// Creates a new <see cref="T:Rhisis.World.Systems.Inventory.EventArgs.InventoryCreateItemEventArgs" /> instance.
        /// </summary>
        /// <param name="itemId">Item id</param>
        /// <param name="quantity">Item quantity</param>
        /// <param name="creatorId">Item creator id</param>
        public InventoryCreateItemEventArgs(int itemId, int quantity, int creatorId)
        {
            this.ItemId = itemId;
            this.Quantity = quantity;
            this.CreatorId = creatorId;
        }

        /// <inheritdoc />
        public override bool CheckArguments()
        {
            if (!WorldServer.Items.TryGetValue(this.ItemId, out ItemData itemData))
                throw new ArgumentException($"Cannot find item with Id: {this.ItemId}.");

            this.ItemData = itemData;

            return this.ItemId > 0 && this.Quantity > 0 && this.Quantity <= this.ItemData.PackMax;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Create item: {this.ItemId}; Quantity:{this.Quantity}; Creator: {this.CreatorId}";
        }
    }
}
