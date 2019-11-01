using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="RemoveInventoryItemPacket"/> structure.
    /// </summary>
    public struct RemoveInventoryItemPacket : IEquatable<RemoveInventoryItemPacket>
    {
        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; set; }

        /// <summary>
        /// Gets the quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Creates a new <see cref="RemoveInventoryItemPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public RemoveInventoryItemPacket(INetPacketStream packet)
        {
            this.ItemId = packet.Read<uint>();
            this.Quantity = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="RemoveInventoryItemPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="RemoveInventoryItemPacket"/></param>
        public bool Equals(RemoveInventoryItemPacket other)
        {
            return this.ItemId == other.ItemId && this.Quantity == other.Quantity;
        }
    }
}