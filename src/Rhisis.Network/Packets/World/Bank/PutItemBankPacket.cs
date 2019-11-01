using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Bank
{
    /// <summary>
    /// Defines the <see cref="PutItemBankPacket"/> structure.
    /// </summary>
    public struct PutItemBankPacket : IEquatable<PutItemBankPacket>
    {
        /// <summary>
        /// Gets the slot.
        /// </summary>
        public byte Slot { get; set; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public byte Id { get; set; }

        /// <summary>
        /// Gets the iten number.
        /// </summary>
        public short ItemNumber { get; set; }

        /// <summary>
        /// Creates a new <see cref="PutItemBankPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PutItemBankPacket(INetPacketStream packet)
        {
            this.Slot = packet.Read<byte>();
            this.Id = packet.Read<byte>();
            this.ItemNumber = packet.Read<short>();
        }

        /// <summary>
        /// Compares two <see cref="PutItemBankPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PutItemBankPacket"/></param>
        public bool Equals(PutItemBankPacket other)
        {
            return this.Slot == other.Slot && this.Id == other.Id && this.ItemNumber == other.ItemNumber;
        }
    }
}