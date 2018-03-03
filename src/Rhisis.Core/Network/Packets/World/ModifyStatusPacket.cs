using Ether.Network.Packets;

namespace Rhisis.Core.Network.Packets.World
{
    public class ModifyStatusPacket
    {
        public ushort StrenghtCount { get; }

        public ushort StaminaCount { get; }

        public ushort DexterityCount { get; }

        public ushort IntelligenceCount { get; }

        public ModifyStatusPacket(NetPacketBase packet)
        {
            this.StrenghtCount = (ushort)packet.Read<int>();
            this.StaminaCount = (ushort)packet.Read<int>();
            this.DexterityCount = (ushort)packet.Read<int>();
            this.IntelligenceCount = (ushort)packet.Read<int>();
        }
    }
}
