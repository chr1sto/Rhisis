using Sylver.Network.Data;
using Rhisis.Core.Data;

namespace Rhisis.Network.Packets.World
{
    public class MagicAttackPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the attack message.
        /// </summary>
        public ObjectMessageType AttackMessage { get; private set; }

        /// <summary>
        /// Gets the target object id.
        /// </summary>
        public uint ObjectId { get; private set; }

        /// <summary>
        /// Gets the second parameter.
        /// </summary>
        public int Parameter2 { get; private set; }

        /// <summary>
        /// Gets the third parameter.
        /// </summary>
        public int Parameter3 { get; private set; }

        /// <summary>
        /// Gets the magic power.
        /// </summary>
        public int MagicPower { get; private set; }

        /// <summary>
        /// Gets the id of the hit SFX.
        /// </summary>
        public int IdSfxHit { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            AttackMessage = (ObjectMessageType)packet.Read<uint>();
            ObjectId = packet.Read<uint>();
            Parameter2 = packet.Read<int>();
            Parameter3 = packet.Read<int>();
            MagicPower = packet.Read<int>();
            IdSfxHit = packet.Read<int>();
        }
    }
}
