using System;
using System.Collections.Generic;
using System.Text;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.NpcShop.EventArgs
{
    internal sealed class NpcShopSellEventArgs : SystemEventArgs
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
        /// Creates a new <see cref="NpcShopSellEventArgs"/> instance.
        /// </summary>
        /// <param name="itemUniqueId">Item unique id</param>
        /// <param name="quantity">Item quantity</param>
        public NpcShopSellEventArgs(byte itemUniqueId, short quantity)
        {
            this.ItemUniqueId = itemUniqueId;
            this.Quantity = quantity;
        }

        /// <inheritdoc />
        public override bool CheckArguments() => this.Quantity > 0;
    }
}
