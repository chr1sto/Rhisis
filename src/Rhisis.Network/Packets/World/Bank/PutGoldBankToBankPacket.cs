using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.Bank
{
    /// <summary>
    /// Defines the <see cref="PutGoldBankToBankPacket"/> structure.
    /// </summary>
    public struct PutGoldBankToBankPacket : IEquatable<PutGoldBankToBankPacket>
    {
        /// <summary>
        /// Gets the flag (always 0).
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
        /// Gets the amount of gold.
        /// </summary>
        public uint Gold { get; set; }

        /// <summary>
        /// Creates a new <see cref="PutGoldBankToBankPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PutGoldBankToBankPacket(INetPacketStream packet)
        {
            this.Flag = packet.Read<byte>();
            this.DestinationSlot = packet.Read<byte>();
            this.SourceSlot = packet.Read<byte>();
            this.Gold = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="PutGoldBankToBankPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PutGoldBankToBankPacket"/></param>
        public bool Equals(PutGoldBankToBankPacket other)
        {
            return this.Flag == other.Flag && this.DestinationSlot == other.DestinationSlot && this.SourceSlot == other.SourceSlot && this.Gold == other.Gold;
        }
    }
}