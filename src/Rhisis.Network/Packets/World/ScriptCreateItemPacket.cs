using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="ScriptCreateItemPacket"/> structure.
    /// </summary>
    public struct ScriptCreateItemPacket : IEquatable<ScriptCreateItemPacket>
    {
        /// <summary>
        /// Gets the item type.
        /// </summary>
        public byte ItemType { get; set; }

        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; set; }

        /// <summary>
        /// Gets the quantity.
        /// </summary>
        public short Quantity { get; set; }

        /// <summary>
        /// Gets the option.
        /// </summary>
        public int Option { get; set; }

        /// <summary>
        /// Creates a new <see cref="ScriptCreateItemPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public ScriptCreateItemPacket(INetPacketStream packet)
        {
            this.ItemType = packet.Read<byte>();
            this.ItemId = packet.Read<uint>();
            this.Quantity = packet.Read<short>();
            this.Option = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="ScriptCreateItemPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="ScriptCreateItemPacket"/></param>
        public bool Equals(ScriptCreateItemPacket other)
        {
            return this.ItemType == other.ItemType && this.ItemId == other.ItemId && this.Quantity == other.Quantity && this.Option == other.Option;
        }
    }
}