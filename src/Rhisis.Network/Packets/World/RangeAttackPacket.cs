using Sylver.Network.Data;
using Rhisis.Core.Data;

namespace Rhisis.Network.Packets.World
{
    public class RangeAttackPacket : IPacketDeserializer
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
        /// Gets the range attack power.
        /// </summary>
        public int Power { get; private set; }

        /// <summary>
        /// Gets the projectile id.
        /// </summary>
        public int ProjectileId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            AttackMessage = (ObjectMessageType)packet.Read<uint>();
            ObjectId = packet.Read<uint>();
            Power = packet.Read<int>();
            ProjectileId = packet.Read<int>();
        }
    }
}
