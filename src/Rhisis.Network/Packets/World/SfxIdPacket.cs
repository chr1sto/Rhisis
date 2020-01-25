using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class SfxIdPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the target id.
        /// </summary>
        public uint Target { get; private set; }

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
        public uint Skill { get; private set; }

        /// <summary>
        /// Gets the max damage count.
        /// </summary>
        public int MaxDamageCount { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Target = packet.Read<uint>();
            IdSfxHit = packet.Read<int>();
            Type = packet.Read<uint>();
            Skill = packet.Read<uint>();
            MaxDamageCount = packet.Read<int>();
        }
    }
}