using System;
using Sylver.Network.Data;

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
            Type = packet.Read<ushort>();
            Id = packet.Read<ushort>();
            ObjectId = packet.Read<uint>();
            UseSkill = packet.Read<int>();
            Control = packet.Read<int>() == 1;
        }

        /// <summary>
        /// Compares two <see cref="UseSkillPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="UseSkillPacket"/></param>
        public bool Equals(UseSkillPacket other)
        {
            return Type == other.Type && Id == other.Id && ObjectId == other.ObjectId && UseSkill == other.UseSkill && Control == other.Control;
        }
    }
}