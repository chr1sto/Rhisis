using Ether.Network.Packets;
using System;

namespace Rhisis.Core.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="MoveItemPacket"/> structure.
    /// </summary>
    public struct MoveItemPacket : IEquatable<MoveItemPacket>
    {
        /// <summary>
        /// Gets the Item type.
        /// </summary>
        public byte ItemType { get; }

        /// <summary>
        /// Gets the Item source slot.
        /// </summary>
        public byte SourceSlot { get; }

        /// <summary>
        /// Gets the item destination slot.
        /// </summary>
        public byte DestinationSlot { get; }

        /// <summary>
        /// Creates a new <see cref="MoveItemPacket"/> instance.
        /// </summary>
        /// <param name="packet"></param>
        public MoveItemPacket(INetPacketStream packet)
        {
            this.ItemType = packet.Read<byte>();
            this.SourceSlot = packet.Read<byte>();
            this.DestinationSlot = packet.Read<byte>();
        }

        /// <summary>
        /// Compares two <see cref="MoveItemPacket"/> instance.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(MoveItemPacket other)
        {
            throw new NotImplementedException();
        }
    }
}
