using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="UseSkillPacket"/> structure.
    /// </summary>
    public struct UseSkillPacket : IEquatable<UseSkillPacket>
    {

        public ushort Type { get; set; }

        public ushort Id { get; set; }

        public uint ObjectId { get; set; }

        public int UseSkill { get; set; }

        public bool Control { get; set; }

        /// <summary>
        /// Creates a new <see cref="UseSkillPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public UseSkillPacket(INetPacketStream packet)
        {
            this.Type = packet.Read<ushort>();
            this.Id = packet.Read<ushort>();
            this.ObjectId = packet.Read<uint>();
            this.UseSkill = packet.Read<int>();
            this.Control = packet.Read<int>() == 1;
        }

        /// <summary>
        /// Compares two <see cref="UseSkillPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="UseSkillPacket"/></param>
        public bool Equals(UseSkillPacket other)
        {
            return this.Type == other.Type && this.Id == other.Id && this.ObjectId == other.ObjectId && this.UseSkill == other.UseSkill && this.Control == other.Control;
        }
    }
}