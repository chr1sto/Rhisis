using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Bank
{
    /// <summary>
    /// Defines the <see cref="GetGoldBankPacket"/> structure.
    /// </summary>
    public struct GetGoldBankPacket : IEquatable<GetGoldBankPacket>
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
        /// Creates a new <see cref="GetGoldBankPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public GetGoldBankPacket(INetPacketStream packet)
        {
            this.Slot = packet.Read<byte>();
            this.Gold = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="GetGoldBankPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="GetGoldBankPacket"/></param>
        public bool Equals(GetGoldBankPacket other)
        {
            return this.Slot == other.Slot && this.Gold == other.Gold;
        }
    }
}