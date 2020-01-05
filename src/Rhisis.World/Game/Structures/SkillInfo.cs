using Rhisis.Network.Packets;
using Sylver.Network.Data;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Rhisis.World.Game.Structures
{
    public class SkillInfo : IEquatable<SkillInfo>, IPacketSerializer
    {
        /// <summary>
        /// Gets the skill id.
        /// </summary>
        public int SkillId { get; }

        /// <summary>
        /// Gets the character id owner of this skill.
        /// </summary>
        public int CharacterId { get; }

        /// <summary>
        /// Gets the skill level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Compares two <see cref="SkillInfo"/> instances.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals([AllowNull] SkillInfo other) => SkillId == other?.SkillId && CharacterId == other?.CharacterId;

        /// <inheritdoc />
        public void Serialize(INetPacketStream packet)
        {
            packet.Write(SkillId);
            packet.Write(Level);
        }
    }
}