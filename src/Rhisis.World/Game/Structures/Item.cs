using Ether.Network.Packets;
using Rhisis.Core.Structures.Game;
using System.Collections.Generic;

namespace Rhisis.World.Game.Structures
{
    /// <summary>
    /// FlyFF item structure.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Flyff item refine table.
        /// </summary>
        public static readonly IReadOnlyCollection<int> RefineTable = new[] { 0, 2, 4, 6, 8, 10, 13, 16, 19, 21, 24 };

        /// <summary>
        /// Gets the item Id.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the item unique Id.
        /// </summary>
        public int UniqueId { get; set; }

        /// <summary>
        /// Gets the creator id of the item.
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// Gets the item quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets the item current slot.
        /// </summary>
        public int Slot { get; set; }

        /// <summary>
        /// Gets or sets the item refine.
        /// </summary>
        public byte Refine { get; set; }

        /// <summary>
        /// Gets or sets the item element. (Fire, water, electricity, etc...)
        /// </summary>
        public byte Element { get; set; }

        /// <summary>
        /// Gets or sets the item element refine.
        /// </summary>
        public byte ElementRefine { get; set; }

        /// <summary>
        /// Gets the item data informations.
        /// </summary>
        public ItemData Data { get; }

        /// <summary>
        /// Creates an empty <see cref="Item"/>.
        /// </summary>
        /// <remarks>
        /// All values set to -1.
        /// </remarks>
        public Item()
            : this(-1, -1, -1, -1, -1)
        {
        }

        /// <summary>
        /// Create an <see cref="Item"/> with an id.
        /// </summary>
        /// <param name="id">Item Id</param>
        public Item(int id)
            : this(id, 1, -1, -1, -1)
        {
        }

        /// <summary>
        /// Create an <see cref="Item"/> with an id and a quantity.
        /// </summary>
        /// <param name="id">Item id</param>
        /// <param name="quantity">Item quantity</param>
        public Item(int id, int quantity)
            : this(id, quantity, -1, -1, -1)
        {
        }

        /// <summary>
        /// Create an <see cref="Item"/> with an id, quantity and creator id.
        /// </summary>
        /// <param name="id">Item id</param>
        /// <param name="quantity">Item quantity</param>
        /// <param name="creatorId">Id of the character that created the object (for GM)</param>
        public Item(int id, int quantity, int creatorId)
            : this(id, quantity, creatorId, -1, -1)
        {
        }

        /// <summary>
        /// Create an <see cref="Item"/> with an id, quantity, creator id and destination slot.
        /// </summary>
        /// <param name="id">Item id</param>
        /// <param name="quantity">Itme quantity</param>
        /// <param name="creatorId">Id of the character that created the object (for GM)</param>
        /// <param name="slot">Item slot</param>
        public Item(int id, int quantity, int creatorId, int slot)
            : this(id, quantity, creatorId, slot, -1)
        {
        }

        /// <summary>
        /// Create an <see cref="Item"/> with an id, quantity, creator id and destination slot.
        /// </summary>
        /// <param name="id">Item id</param>
        /// <param name="quantity">Itme quantity</param>
        /// <param name="creatorId">Id of the character that created the object (for GM)</param>
        /// <param name="slot">Item slot</param>
        /// <param name="uniqueId">Item unique id</param>
        public Item(int id, int quantity, int creatorId, int slot, int uniqueId)
            : this(id, quantity, creatorId, slot, -1, 0)
        {
        }


        /// <summary>
        /// Create an <see cref="Item"/> with an id, quantity, creator id, destination slot and refine.
        /// </summary>
        /// <param name="id">Item id</param>
        /// <param name="quantity">Itme quantity</param>
        /// <param name="creatorId">Id of the character that created the object (for GM)</param>
        /// <param name="slot">Item slot</param>
        /// <param name="uniqueId">Item unique id</param>
        /// <param name="refine">Item refine</param>
        public Item(int id, int quantity, int creatorId, int slot, int uniqueId, byte refine)
            : this(id, quantity, creatorId, slot, uniqueId, refine, 0, 0)
        {
        }

        /// <summary>
        /// Create an <see cref="Item"/> with an id, quantity, creator id, destination slot, refine and element.
        /// </summary>
        /// <param name="id">Item id</param>
        /// <param name="quantity">Itme quantity</param>
        /// <param name="creatorId">Id of the character that created the object (for GM)</param>
        /// <param name="slot">Item slot</param>
        /// <param name="uniqueId">Item unique id</param>
        /// <param name="refine">Item refine</param>
        /// <param name="element">Item element</param>
        public Item(int id, int quantity, int creatorId, int slot, int uniqueId, byte refine, byte element)
            : this(id, quantity, creatorId, slot, uniqueId, refine, element, 0)
        {
        }

        /// <summary>
        /// Create an <see cref="Item"/> with an id, quantity, creator id, destination slot, refine and element.
        /// </summary>
        /// <param name="id">Item id</param>
        /// <param name="quantity">Itme quantity</param>
        /// <param name="creatorId">Id of the character that created the object (for GM)</param>
        /// <param name="slot">Item slot</param>
        /// <param name="uniqueId">Item unique id</param>
        /// <param name="refine">Item refine</param>
        /// <param name="element">Item element</param>
        /// <param name="elementRefine">Item element refine</param>
        public Item(int id, int quantity, int creatorId, int slot, int uniqueId, byte refine, byte element, byte elementRefine)
        {
            this.Id = id;
            this.Quantity = quantity;
            this.CreatorId = creatorId;
            this.Slot = slot;
            this.UniqueId = uniqueId;
            this.Refine = refine;
            this.Element = element;
            this.ElementRefine = elementRefine;
            this.Data = WorldServer.Items.ContainsKey(this.Id) ? WorldServer.Items[this.Id] : null;
        }

        /// <summary>
        /// Serialize the item into the packet.
        /// </summary>
        /// <param name="packet"></param>
        public void Serialize(NetPacketBase packet)
        {
            packet.Write(this.Id);
            packet.Write(0);
            packet.Write(0);
            packet.Write((short)this.Quantity);
            packet.Write<byte>(0);
            packet.Write(0);
            packet.Write(0);
            packet.Write<byte>(0);
            packet.Write((int)this.Refine);
            packet.Write(0); // guild id (cloaks?)
            packet.Write(this.Element);
            packet.Write((int)this.ElementRefine);
            packet.Write(0);
            packet.Write(0);
            packet.Write(0);
            packet.Write(0);
            packet.Write<long>(0);
            packet.Write<long>(0);
            packet.Write<byte>(0);
            packet.Write(0);
        }


        /// <summary>
        /// Clones this <see cref="Item"/>.
        /// </summary>
        /// <returns></returns>
        public Item Clone()
        {
            return new Item(this.Id, this.Quantity, this.CreatorId, this.Slot, this.UniqueId, this.Refine, this.Element, this.ElementRefine);
        }

        /// <summary>
        /// Returns the current <see cref="Item"/> on string format
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Item: Id{this.Id} Slot:{this.Slot} UniqueId:{this.UniqueId}";
        }

    }
}
