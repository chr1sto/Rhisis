using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Bank
{
    /// <summary>
    /// Defines the <see cref="PutItemGuildBankPacket"/> structure.
    /// </summary>
    public struct PutItemGuildBankPacket : IEquatable<PutItemGuildBankPacket>
    {

        public byte Id { get; set; }

        public uint ItemId { get; set; }

        public byte Mode { get; set; }

        /// <summary>
        /// Creates a new <see cref="PutItemGuildBankPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PutItemGuildBankPacket(INetPacketStream packet)
        {
            this.Id = packet.Read<byte>();
            this.ItemId = packet.Read<uint>();
            this.Mode = packet.Read<byte>();
        }

        /// <summary>
        /// Compares two <see cref="PutItemGuildBankPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PutItemGuildBankPacket"/></param>
        public bool Equals(PutItemGuildBankPacket other)
        {
            return this.Id == other.Id && this.ItemId == other.ItemId && this.Mode == other.Mode;
        }
    }
}