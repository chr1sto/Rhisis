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
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; private set; }

        /// <summary>
        /// Gets the id of the hit SFX.
        /// </summary>
        public int IdSfxHit { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            AttackMessage = (ObjectMessageType)packet.Read<uint>();
            ObjectId = packet.Read<uint>();
            ItemId = packet.Read<uint>();
            IdSfxHit = packet.Read<int>();
        }
    }
}
