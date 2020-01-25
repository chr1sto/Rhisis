using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Bank
{
    /// <summary>
    /// Defines the <see cref="OpenBankWindowPacket"/> structure.
    /// </summary>
    public struct OpenBankWindowPacket : IEquatable<OpenBankWindowPacket>
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; set; }

        /// <summary>
        /// Creates a new <see cref="OpenBankWindowPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public OpenBankWindowPacket(INetPacketStream packet)
        {
            Id = packet.Read<uint>();
            ItemId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="OpenBankWindowPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="OpenBankWindowPacket"/></param>
        public bool Equals(OpenBankWindowPacket other)
        {
            return Id == other.Id && ItemId == other.ItemId;
        }
    }
}