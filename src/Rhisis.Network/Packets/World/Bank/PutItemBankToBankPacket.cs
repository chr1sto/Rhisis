using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Bank
{
    /// <summary>
    /// Defines the <see cref="PutItemBankToBankPacket"/> structure.
    /// </summary>
    public struct PutItemBankToBankPacket : IEquatable<PutItemBankToBankPacket>
    {
        /// <summary>
        /// Gets the flag (always 1).
        /// </summary>
        public byte Flag { get; set; }

        /// <summary>
        /// Gets the destination slot.
        /// </summary>
        public byte DestinationSlot { get; set; }

        /// <summary>
        /// Gets the source slot.
        /// </summary>
        public byte SourceSlot { get; set; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public byte Id { get; set; }

        /// <summary>
        /// Gets the item number.
        /// </summary>
        public short ItemNumber { get; set; }

        /// <summary>
        /// Creates a new <see cref="PutItemBankToBankPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PutItemBankToBankPacket(INetPacketStream packet)
        {
            this.Flag = packet.Read<byte>();
            this.DestinationSlot = packet.Read<byte>();
            this.SourceSlot = packet.Read<byte>();
            this.Id = packet.Read<byte>();
            this.ItemNumber = packet.Read<short>();
        }

        /// <summary>
        /// Compares two <see cref="PutItemBankToBankPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PutItemBankToBankPacket"/></param>
        public bool Equals(PutItemBankToBankPacket other)
        {
            return this.Flag == other.Flag && this.DestinationSlot == other.DestinationSlot && this.SourceSlot == other.SourceSlot && this.Id == other.Id && this.ItemNumber == other.ItemNumber;
        }
    }
}