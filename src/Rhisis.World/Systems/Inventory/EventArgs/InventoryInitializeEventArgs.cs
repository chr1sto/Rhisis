using System.Collections.Generic;
using Rhisis.Database.Entities;
using Rhisis.World.Game.Core.Systems;

namespace Rhisis.World.Systems.Inventory.EventArgs
{
    internal sealed class InventoryInitializeEventArgs : SystemEventArgs
    {
        /// <summary>
        /// Gets the items.
        /// </summary>
        public ICollection<DbItem> Items { get; }

        /// <summary>
        /// Creates a new <see cref="InventoryInitializeEventArgs"/> instance.
        /// </summary>
        /// <param name="items"></param>
        public InventoryInitializeEventArgs(ICollection<DbItem> items)
        {
            Items = items;
        }

        /// <inheritdoc />
        public override bool CheckArguments() => true;
    }
}
