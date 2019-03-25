using System;
using Ether.Network.Packets;
using Rhisis.Core.Structures;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="TeleSkillPacket"/> structure.
    /// </summary>
    public struct TeleSkillPacket : IEquatable<TeleSkillPacket>
    {
        /// <summary>
        /// Gets the position
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Creates a new <see cref="TeleSkillPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public TeleSkillPacket(INetPacketStream packet)
        {
            this.Position = new Vector3(packet.Read<float>(), packet.Read<float>(), packet.Read<float>());
        }

        /// <summary>
        /// Compares two <see cref="TeleSkillPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="TeleSkillPacket"/></param>
        public bool Equals(TeleSkillPacket other)
        {
            return this.Position == other.Position;
        }
    }
}