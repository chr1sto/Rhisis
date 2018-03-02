using System;
using System.Collections.Generic;
using System.Text;
using Ether.Network.Packets;

namespace Rhisis.Core.Network.Packets.World
{
    public class ModifyStatusPacket
    {
        public readonly ushort StrenghtCount;

        public readonly ushort StaminaCount;

        public readonly ushort DexterityCount;

        public readonly ushort IntelligenceCount;

        public ModifyStatusPacket(NetPacketBase packet)
        {
            this.StrenghtCount = (ushort)packet.Read<int>();
            this.StaminaCount = (ushort)packet.Read<int>();
            this.DexterityCount = (ushort)packet.Read<int>();
            this.IntelligenceCount = (ushort)packet.Read<int>();
        }
    }
}
