using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Party
{
    public class PartyChangeTroupPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; private set; }

        /// <summary>
        /// Gets if a name was sent.
        /// </summary>
        public bool SendName { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            PlayerId = packet.Read<uint>();
            SendName = packet.Read<int>() == 1;
            Name = SendName ? packet.Read<string>() : null;
        }
    }
}