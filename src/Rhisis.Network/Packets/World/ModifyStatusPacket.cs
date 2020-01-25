using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class ModifyStatusPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the strength.
        /// </summary>
        public ushort Strength { get; private set; }

        /// <summary>
        /// Gets the stamina.
        /// </summary>
        public ushort Stamina { get; private set; }

        /// <summary>
        /// Gets the dexterity.
        /// </summary>
        public ushort Dexterity { get; private set; }

        /// <summary>
        /// Gets the intelligence.
        /// </summary>
        public ushort Intelligence { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Strength = (ushort)packet.Read<int>();
            Stamina = (ushort)packet.Read<int>();
            Dexterity = (ushort)packet.Read<int>();
            Intelligence = (ushort)packet.Read<int>();
        }
    }
}
