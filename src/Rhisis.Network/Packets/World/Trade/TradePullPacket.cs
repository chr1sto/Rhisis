using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Trade
{
    /// <summary>
    /// Defines the <see cref="TradePullPacket"/> structure.
    /// </summary>
    public struct TradePullPacket : IEquatable<TradePullPacket>
    {
        /// <summary>
        /// Gets the slot.
        /// </summary>
        public byte Slot { get; set; }

        /// <summary>
        /// Creates a new <see cref="TradePullPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public TradePullPacket(INetPacketStream packet)
        {
            Slot = packet.Read<byte>();
        }

        /// <summary>
        /// Compares two <see cref="TradePullPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="TradePullPacket"/></param>
        public bool Equals(TradePullPacket other)
        {
            return Slot == other.Slot;
        }
    }
}