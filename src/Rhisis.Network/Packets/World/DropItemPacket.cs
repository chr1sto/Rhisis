using System;
using Ether.Network.Packets;
using Rhisis.Core.Structures;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="DropItemPacket"/> structure.
    /// </summary>
    public struct DropItemPacket : IEquatable<DropItemPacket>
    {
        /// <summary>
        /// Gets the item type
        /// </summary>
        public uint ItemType { get; set; }

        /// <summary>
        /// Gets the unique item id.
        /// </summary>
        public uint ItemId { get; set; }

        /// <summary>
        /// Gets the item quantity.
        /// </summary>
        public int ItemQuantity { get; set; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Creates a new <see cref="DropItemPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public DropItemPacket(INetPacketStream packet)
        {
            ItemType = packet.Read<uint>();
            ItemId = packet.Read<uint>();
            ItemQuantity = packet.Read<int>();
            Position = new Vector3(packet.Read<float>(), packet.Read<float>(), packet.Read<float>());
        }

        /// <summary>
        /// Compares two <see cref="DropItemPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="DropItemPacket"/></param>
        public bool Equals(DropItemPacket other)
        {
            return this.ItemType == other.ItemType && this.ItemId == other.ItemId && this.ItemQuantity == other.ItemQuantity && this.Position == other.Position;
        }
    }
}