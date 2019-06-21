using Rhisis.World.Game.Core.Systems;

namespace Rhisis.World.Systems.Inventory.EventArgs
{
    internal sealed class InventoryDropItemEventArgs : SystemEventArgs
    {
        /// <summary>
        /// Gets the unique item id to drop.
        /// </summary>
        public int UniqueItemId { get; }

        /// <summary>
        /// Gets the amount of items to drop.
        /// </summary>
        public int Quantity { get; }

        /// <summary>
        /// Creates a new <see cref="InventoryDropItemEventArgs"/> instance.
        /// </summary>
        /// <param name="uniqueItemId">Unique Item Id to drop.</param>
        /// <param name="quantity">Quantity to drop.</param>
        public InventoryDropItemEventArgs(int uniqueItemId, int quantity)
        {
            this.UniqueItemId = uniqueItemId;
            this.Quantity = quantity;
        }

        /// <inheritdoc />
        public override bool CheckArguments()
        {
            return this.UniqueItemId >= 0 && this.Quantity > 0;
        }
    }
}
