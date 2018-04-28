using Ether.Network.Packets;

namespace Rhisis.Core.Network.Packets.World
{
    public class ModifyStatusPacket
    {
        public ushort Strenght { get; }

        public ushort Stamina { get; }

        public ushort Dexterity { get; }

        public ushort Intelligence { get; }

        public ModifyStatusPacket(INetPacketStream packet)
        {
            this.Strenght = (ushort)packet.Read<int>();
            this.Stamina = (ushort)packet.Read<int>();
            this.Dexterity = (ushort)packet.Read<int>();
            this.Intelligence = (ushort)packet.Read<int>();
        }
    }
}
