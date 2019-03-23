using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World
{
    public struct SellItemPacket : IEquatable<SellItemPacket>
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
        /// Creates a new <see cref="SellItemPacket"/> instance.
        /// </summary>
        /// <param name="packet"></param>
        public SellItemPacket(INetPacketStream packet)
        {
            this.ItemUniqueId = packet.Read<byte>();
            this.Quantity = packet.Read<short>();
        }

        /// <inheritdoc />
        public bool Equals(SellItemPacket other) => this.ItemUniqueId == other.ItemUniqueId && this.Quantity == other.Quantity;
    }
}
