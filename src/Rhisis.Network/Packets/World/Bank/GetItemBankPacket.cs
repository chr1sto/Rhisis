using System;
using System.Runtime.CompilerServices;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Bank
{
    /// <summary>
    /// Defines the <see cref="GetItemBankPacket"/> structure.
    /// </summary>
    public struct GetItemBankPacket : IEquatable<GetItemBankPacket>
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
        /// Gets the item number.
        /// </summary>
        public short ItemNumber { get; set; }

        /// <summary>
        /// Creates a new <see cref="GetItemBankPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public GetItemBankPacket(INetPacketStream packet)
        {
            this.Slot = packet.Read<byte>();
            this.Id = packet.Read<byte>();
            this.ItemNumber = packet.Read<short>();
        }

        /// <summary>
        /// Compares two <see cref="GetItemBankPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="GetItemBankPacket"/></param>
        public bool Equals(GetItemBankPacket other)
        {
            return this.Slot == other.Slot && this.Id == other.Id && this.ItemNumber == other.ItemNumber;
        }
    }
}