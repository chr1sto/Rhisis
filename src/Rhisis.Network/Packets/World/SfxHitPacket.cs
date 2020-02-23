using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class SfxHitPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the SFX projectile id.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the SFX projectile magic power.
        /// </summary>
        public int MagicPower { get; private set; }

        /// <summary>
        /// Gets the SFX projectile skill id.
        /// </summary>
        public int SkillId { get; private set; }

        /// <summary>
        /// Gets the SFX projectile caster id.
        /// </summary>
        public uint AttackerId { get; private set; }

        /// <summary>
        /// Gets the SFX projectile damage count.
        /// </summary>
        public uint DamageCount { get; private set; }

        /// <summary>
        /// Gets the SFX projectile damage angle.
        /// </summary>
        public float DamageAngle { get; private set; }

        /// <summary>
        /// Gets the SFX projectile damage power.
        /// </summary>
        public float DamagePower { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Id = packet.Read<int>();
            MagicPower = packet.Read<int>();
            SkillId = packet.Read<int>();
            AttackerId = packet.Read<uint>();
            DamageCount = packet.Read<uint>();
            DamageAngle = packet.Read<float>();
            DamagePower = packet.Read<float>();
        }
    }
}