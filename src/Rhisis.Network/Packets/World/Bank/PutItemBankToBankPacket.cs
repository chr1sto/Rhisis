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
            Flag = packet.Read<byte>();
            DestinationSlot = packet.Read<byte>();
            SourceSlot = packet.Read<byte>();
            Id = packet.Read<byte>();
            ItemNumber = packet.Read<short>();
        }

        /// <summary>
        /// Compares two <see cref="PutItemBankToBankPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PutItemBankToBankPacket"/></param>
        public bool Equals(PutItemBankToBankPacket other)
        {
            return Flag == other.Flag && DestinationSlot == other.DestinationSlot && SourceSlot == other.SourceSlot && Id == other.Id && ItemNumber == other.ItemNumber;
        }
    }
}