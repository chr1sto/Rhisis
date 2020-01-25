using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Party
{
    public class PartyChangeNamePacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            PlayerId = packet.Read<uint>();
            Name = packet.Read<string>();
        }
    }
}