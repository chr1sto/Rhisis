using Ether.Network.Packets;
using Rhisis.World.Game.Structures;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Components
{
    public class ItemContainerComponent
    {
        /// <summary>
        /// Gets the <see cref="ItemContainerComponent"/> max capacity.
        /// </summary>
        public int MaxCapacity { get; }

        /// <summary>
        /// Gets the list of items in this <see cref="ItemContainerComponent"/>.
        /// </summary>
        public List<Item> Items { get; }

        /// <summary>
        /// Creates a new <see cref="ItemContainerComponent"/> instance.
        /// </summary>
        public ItemContainerComponent(int maxCapacity)
        {
            this.MaxCapacity = maxCapacity;
            this.Items = new List<Item>(new Item[this.MaxCapacity]);

            for (var i = 0; i < this.MaxCapacity; i++)
            {
                this.Items[i] = new Item
                {
                    UniqueId = i
                };
            }
        }

        /// <summary>
        /// Gets the item by the unique id.
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        public Item GetItem(int uniqueId) => this.Items.FirstOrDefault(x => x.UniqueId == uniqueId);

        /// <summary>
        /// Gets the item by slot.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public Item GetItemBySlot(int slot) => this.Items[slot];

        /// <summary>
        /// Returs the position of an available slot.
        /// </summary>
        /// <returns></returns>
        public int GetAvailableSlot()
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i] != null && this.Items[i].Slot == -1)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Check if there is any available slots.
        /// </summary>
        /// <returns></returns>
        public bool HasAvailableSlots() => this.GetAvailableSlot() != -1;

        /// <summary>
        /// Serialize the ItemContainer.
        /// </summary>
        /// <param name="packet"></param>
        public void Serialize(NetPacketBase packet)
        {
            for (var i = 0; i < this.MaxCapacity; ++i)
                packet.Write(this.Items[i].UniqueId);

            packet.Write((byte)this.Items.Count(x => x.Id != -1));

            for (var i = 0; i < this.MaxCapacity; ++i)
            {
                if (this.Items[i].Id > 0)
                {
                    packet.Write((byte)this.Items[i].UniqueId);
                    packet.Write(this.Items[i].UniqueId);
                    this.Items[i].Serialize(packet);
                }
            }

            for (var i = 0; i < this.MaxCapacity; ++i)
                packet.Write(this.Items[i].UniqueId);
        }
    }
}
