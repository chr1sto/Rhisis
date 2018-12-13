using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.Bank
{
    /// <summary>
    /// Defines the <see cref="GetItemGuildBankPacket"/> structure.
    /// </summary>
    public struct GetItemGuildBankPacket : IEquatable<GetItemGuildBankPacket>
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public byte Id { get; set; }

        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; set; }

        /// <summary>
        /// Gets the mode.
        /// </summary>
        public byte Mode { get; set; }

        /// <summary>
        /// Creates a new <see cref="GetItemGuildBankPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public GetItemGuildBankPacket(INetPacketStream packet)
        {
            this.Id = packet.Read<byte>();
            this.ItemId = packet.Read<uint>();
            this.Mode = packet.Read<byte>();
        }

        /// <summary>
        /// Compares two <see cref="GetItemGuildBankPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="GetItemGuildBankPacket"/></param>
        public bool Equals(GetItemGuildBankPacket other)
        {
            return this.Id == other.Id && this.ItemId == other.ItemId && this.Mode == other.Mode;
        }
    }
}