using Sylver.Network.Data;
using Rhisis.Core.Data;

namespace Rhisis.Network.Packets.World
{
    public class MeleeAttackPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the attack message.
        /// </summary>
        public ObjectMessageType AttackMessage { get; private set; }

        /// <summary>
        /// Gets the object id.
        /// </summary>
        public uint ObjectId { get; private set; }
        
        /// <summary>
        /// Gets the unknown parameter.
        /// </summary>
        public int UnknownParameter { get; private set; }
        
        /// <summary>
        /// Gets the attack flags.
        /// </summary>
        public int AttackFlags { get; private set; }

        /// <summary>
        /// Gets the attack speed.
        /// </summary>
        public float WeaponAttackSpeed { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            AttackMessage = (ObjectMessageType)packet.Read<int>();
            ObjectId = packet.Read<uint>();
            UnknownParameter = packet.Read<int>(); // ??
            AttackFlags = packet.Read<int>() & 0xFFFF; // Attack flags ?!
            WeaponAttackSpeed = packet.Read<float>();
        }
    }
}
