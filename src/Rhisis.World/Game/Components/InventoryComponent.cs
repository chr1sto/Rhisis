using Rhisis.World.Game.Structures;
using Rhisis.World.Systems;
using System.Collections.Generic;

namespace Rhisis.World.Game.Components
{
    public class InventoryComponent
    {
        public List<Item> Items { get; }

        public InventoryComponent()
        {
            this.Items = new List<Item>(new Item[InventorySystem.MaxItems]);
        }
    }
}
