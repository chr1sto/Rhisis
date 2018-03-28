using System;
using System.Runtime.CompilerServices;
using Rhisis.Core.Structures.Game;

namespace Rhisis.World.Systems.NpcShop
{
    public class NpcShopBuyItemEventArgs : NpcShopEventArgs
    {
        /// <summary>
        /// Gets the item id to buy.
        /// </summary>
        public int ItemId { get; }

        /// <summary>
        /// Gets the quantity of items to buy.
        /// </summary>
        public int Quantity { get; }

        /// <summary>
        /// Gets the item data.
        /// </summary>
        public ItemData ItemData { get; private set; }

        /// <inheritdoc />
        public NpcShopBuyItemEventArgs(int itemId, int quantity)
            : base(NpcShopActionType.Buy, itemId, quantity)
        {
            this.ItemId = itemId;
            this.Quantity = quantity;
        }

        /// <inheritdoc />
        public override bool CheckArguments()
        {
            if (!WorldServer.Items.TryGetValue(this.ItemId, out ItemData itemData))
                throw new ArgumentException($"Cannot find item with Id: {this.ItemId}.");

            this.ItemData = itemData;

            return this.ItemId > 0 && this.Quantity > 0 && this.Quantity <= this.ItemData.PackMax;
        }
    }
}
