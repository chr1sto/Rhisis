using System;
using Sylver.Network.Data;
using Rhisis.Core.Structures;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="DropGoldPacket"/> structure.
    /// </summary>
    public struct DropGoldPacket : IEquatable<DropGoldPacket>
    {
        /// <summary>
        /// Gets the amount of gold.
        /// </summary>
        public uint Gold { get; set; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Creates a new <see cref="DropGoldPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public DropGoldPacket(INetPacketStream packet)
        {
            Gold = packet.Read<uint>();
            Position = new Vector3(packet.Read<float>(), packet.Read<float>(), packet.Read<float>());
        }

        /// <summary>
        /// Compares two <see cref="DropGoldPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="DropGoldPacket"/></param>
        public bool Equals(DropGoldPacket other)
        {
            return this.Gold == other.Gold && this.Position == other.Position;
        }
    }
}