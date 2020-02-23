using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class SfxIdPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the target id.
        /// </summary>
        public uint TargetId { get; private set; }

        /// <summary>
        /// Gets the id of the hit SFX.
        /// </summary>
        public int IdSfxHit { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public uint Type { get; private set; }

        /// <summary>
        /// Gets the skill.
        /// </summary>
        public int Skill { get; private set; }

        /// <summary>
        /// Gets the max damage count.
        /// </summary>
        public int MaxDamageCount { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            TargetId = packet.Read<uint>();
            IdSfxHit = packet.Read<int>();
            Type = packet.Read<uint>();
            Skill = packet.Read<int>();
            MaxDamageCount = packet.Read<int>();
        }
    }
}