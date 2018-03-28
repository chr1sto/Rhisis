namespace Rhisis.World.Systems.NpcShop
{
    public class NpcShopSellItemEventArgs : NpcShopEventArgs
    {
        /// <summary>
        /// Gets the item's unique id.
        /// </summary>
        public byte ItemUniqueId { get; }

        /// <summary>
        /// Gets the item quantity to sell.
        /// </summary>
        public short Quantity { get; }

        /// <summary>
        /// Creates a new <see cref="NpcShopSellItemEventArgs"/> instance.
        /// </summary>
        /// <param name="itemUniqueId">Item unique id</param>
        /// <param name="quantity">Item quantity</param>
        public NpcShopSellItemEventArgs(byte itemUniqueId, short quantity)
            : base(NpcShopActionType.Sell, itemUniqueId, quantity)
        {
            this.ItemUniqueId = itemUniqueId;
            this.Quantity = quantity;
        }

        /// <inheritdoc />
        public override bool CheckArguments() => this.Quantity > 0 && this.ItemUniqueId > 0;
    }
}
