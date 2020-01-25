using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="BuyChipItemPacket"/> structure.
    /// </summary>
    public struct BuyChipItemPacket : IEquatable<BuyChipItemPacket>
    {
        /// <summary>
        /// Gets the tab.
        /// </summary>
        public byte Tab { get; set; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public byte Id { get; set; }

        /// <summary>
        /// Gets the quantity.
        /// </summary>
        public short Quantity { get; set; }

        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; set; }

        /// <summary>
        /// Creates a new <see cref="BuyChipItemPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public BuyChipItemPacket(INetPacketStream packet)
        {
            Tab = packet.Read<byte>();
            Id = packet.Read<byte>();
            Quantity = packet.Read<short>();
            ItemId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="BuyChipItemPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="BuyChipItemPacket"/></param>
        public bool Equals(BuyChipItemPacket other)
        {
            return Tab == other.Tab && Id == other.Id && Quantity == other.Quantity && ItemId == other.ItemId;
        }
    }
}