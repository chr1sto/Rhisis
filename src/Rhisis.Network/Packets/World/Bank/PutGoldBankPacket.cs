using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Bank
{
    /// <summary>
    /// Defines the <see cref="PutGoldBankPacket"/> structure.
    /// </summary>
    public struct PutGoldBankPacket : IEquatable<PutGoldBankPacket>
    {
        /// <summary>
        /// Gets the slot.
        /// </summary>
        public byte Slot { get; set; }

        /// <summary>
        /// Gets the amount of gold.
        /// </summary>
        public uint Gold { get; set; }

        /// <summary>
        /// Creates a new <see cref="PutGoldBankPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PutGoldBankPacket(INetPacketStream packet)
        {
            this.Slot = packet.Read<byte>();
            this.Gold = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="PutGoldBankPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PutGoldBankPacket"/></param>
        public bool Equals(PutGoldBankPacket other)
        {
            return this.Slot == other.Slot && this.Gold == other.Gold;
        }
    }
}