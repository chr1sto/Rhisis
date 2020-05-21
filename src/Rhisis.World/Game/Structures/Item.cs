using Rhisis.Core.Data;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Game;
using Rhisis.Database.Entities;
using Rhisis.World.Systems.Inventory;
using Sylver.Network.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rhisis.World.Game.Structures
{
    /// <summary>
    /// FlyFF item structure.
    /// </summary>
    [DebuggerDisplay("({Quantity}) {Data?.Name ?? \"Empty\"} +{Refine} ({Element}+{ElementRefine})")]
    public class Item : ItemDescriptor
    {
        public const int RefineMax = 10;

        /// <summary>
        /// Flyff item refine table.
        /// </summary>
        public static readonly IReadOnlyCollection<int> RefineTable = new[] {0, 2, 4, 6, 8, 10, 13, 16, 19, 21, 24};

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
        /// Gets the current used item quantity
        /// </summary>
        public int ExtraUsed { get; set; }

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
        /// Creates a <see cref="Item"/> instance with an id.
        /// </summary>
        /// <param name="id">Item Id.</param>
        public Item(int id)
            : this(id, -1, -1, -1, -1)
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
            : this(id, quantity, creatorId, slot, uniqueId, 0)
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
            : this(id, quantity, creatorId, slot, uniqueId, refine, 0)
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
        public Item(int id, int quantity, int creatorId, int slot, int uniqueId, byte refine, ElementType element)
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
        /// <param name="elementRefine"></param>
        public Item(int id, int quantity, int creatorId, int slot, int uniqueId, byte refine, ElementType element, byte elementRefine)
            : this(id, quantity, creatorId, slot, uniqueId, refine, element, elementRefine, 0)
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
        /// <param name="extraUsed">Item extra used quantity</param>
        public Item(int id, int quantity, int creatorId, int slot, int uniqueId, byte refine, ElementType element,
            byte elementRefine, int extraUsed)
        {
            Id = id;
            Quantity = quantity;
            CreatorId = creatorId;
            Slot = slot;
            UniqueId = uniqueId;
            Refine = refine;
            Element = element;
            ElementRefine = elementRefine;
            ExtraUsed = extraUsed;
        }

        /// <summary>
        /// Creates a new <see cref="Item"/> based on a database item.
        /// </summary>
        /// <param name="dbItem">Database item</param>
        /// <param name="itemData">Item data.</param>
        public Item(DbItem dbItem, ItemData itemData)
            : this(dbItem.ItemId, dbItem.ItemCount, dbItem.CreatorId, dbItem.ItemSlot, -1, dbItem.Refine,
                (ElementType)dbItem.Element, dbItem.ElementRefine, 0)
        {
            DbId = dbItem.Id;
            Data = itemData;
        }

        /// <summary>
        /// Creates a new <see cref="Item"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="refine"></param>
        /// <param name="element"></param>
        /// <param name="elementRefine"></param>
        /// <param name="itemData"></param>
        /// <param name="creatorId"></param>
        public Item(int id, byte refine, ElementType element, byte elementRefine, ItemData itemData, int creatorId)
        {
            Id = id;
            Quantity = 1;
            Refine = refine;
            Element = element;
            ElementRefine = elementRefine;
            Data = itemData;
            CreatorId = creatorId;
        }

        /// <summary>
        /// Serialize the item into the packet.
        /// </summary>
        /// <param name="packet"></param>
        public void Serialize(INetPacketStream packet)
        {
            packet.Write(UniqueId);
            packet.Write(Id);
            packet.Write(0); // Serial number
            packet.Write(Data?.Name.TakeCharacters(31) ?? "[undefined]");
            packet.Write((short) Quantity);
            packet.Write<byte>(0); // Repair number
            packet.Write(0); // Hp
            packet.Write(0); // Repair
            packet.Write<byte>(0); // flag ?
            packet.Write((int) Refine);
            packet.Write(0); // guild id (cloaks?)
            packet.Write((byte)Element);
            packet.Write((int)ElementRefine);
            packet.Write(0); // m_nResistSMItemId
            packet.Write(0); // Piercing size
            packet.Write(0); // Ultimate piercing size
            packet.Write(0); // Pet vis
            packet.Write(0); // charged
            packet.Write<long>(0); // m_iRandomOptItemId
            packet.Write(0); // m_dwKeepTime
            packet.Write<byte>(0); // pet
            packet.Write(0); // m_bTranformVisPet
        }
        
        /// <summary>
        /// Clones this <see cref="Item"/>.
        /// </summary>
        /// <returns></returns>
        public Item Clone()
        {
            return new Item(Id, Refine, Element, ElementRefine, Data, CreatorId)
            {
                ExtraUsed = ExtraUsed,
                Slot = Slot,
                UniqueId = UniqueId,
                Quantity = Quantity
            };
        }

        /// <summary>
        /// Reset the item.
        /// </summary>
        public void Reset()
        {
            Id = -1;
            DbId = -1;
            Quantity = 0;
            CreatorId = -1;
            Refine = 0;
            Element = 0;
            ElementRefine = 0;
            ExtraUsed = 0;
            Slot = -1;
            Data = null;
        }

        /// <summary>
        /// Returns the current <see cref="Item"/> on string format
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Data?.Name}";
    }
}