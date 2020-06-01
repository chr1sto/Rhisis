using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.GuildCombat
{
    public class SelectMapGuildCombatPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the map id.
        /// </summary>
        public int Map { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Map = packet.Read<int>();
        }
    }
}