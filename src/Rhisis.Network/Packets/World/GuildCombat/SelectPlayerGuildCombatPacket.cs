using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.GuildCombat
{
    public class SelectPlayerGuildCombatPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the window.
        /// </summary>
        public bool Window { get; private set; }

        /// <summary>
        /// Gets the defender id.
        /// </summary>
        public uint? DefenderId { get; private set; }

        /// <summary>
        /// Gets the SelectPlayer size.
        /// </summary>
        public int? Size { get; private set; }

        /// <summary>
        /// Gets the players to select.
        /// </summary>
        public uint?[] SelectPlayer { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Window = packet.Read<int>() == 1;
            if (!Window)
            {
                DefenderId = packet.Read<uint>();
                Size = packet.Read<int>();
                SelectPlayer = new uint?[Size.Value];
                for (int i = 0; i < Size.Value; i++)
                    SelectPlayer[i] = packet.Read<uint>();
            }
            else
            {
                DefenderId = null;
                Size = null;
                SelectPlayer = null;
            }
        }
    }
}