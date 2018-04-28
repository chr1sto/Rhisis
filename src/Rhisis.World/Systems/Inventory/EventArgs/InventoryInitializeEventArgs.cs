using System;
using System.Collections.Generic;
using System.Text;
using Rhisis.Database.Structures;
using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.Inventory.EventArgs
{
    internal sealed class InventoryInitializeEventArgs : SystemEventArgs
    {
        public ICollection<Item> Items { get; }

        public InventoryInitializeEventArgs(ICollection<Item> items)
        {
            Items = items;
        }

        /// <inheritdoc />
        public override bool CheckArguments() => true;
    }
}
