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
            Id = packet.Read<byte>();
            ItemId = packet.Read<uint>();
            Mode = packet.Read<byte>();
        }

        /// <summary>
        /// Compares two <see cref="PutItemGuildBankPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PutItemGuildBankPacket"/></param>
        public bool Equals(PutItemGuildBankPacket other)
        {
            return Id == other.Id && ItemId == other.ItemId && Mode == other.Mode;
        }
    }
}