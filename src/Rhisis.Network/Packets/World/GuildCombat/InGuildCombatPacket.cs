using Sylver.Network.Data;
using Rhisis.Core.Data;

namespace Rhisis.Network.Packets.World.GuildCombat
{
    public class InGuildCombatPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the guild combat type.
        /// </summary>
        public GuildCombatType GuildCombatType { get; private set; }

        /// <summary>
        /// Gets the guild combat gold amount.
        /// </summary>
        public uint? Penya { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            GuildCombatType = (GuildCombatType)packet.Read<int>();
            Penya = GuildCombatType == GuildCombatType.GC_IN_APP ? (uint?)packet.Read<uint>() : null;
        }
    }
}