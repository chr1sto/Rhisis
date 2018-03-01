using System;
using System.Collections.Generic;
using System.Text;
using Ether.Network.Packets;

namespace Rhisis.Core.Network.Packets.World
{
    public class ModifyStatusPacket
    {
        public readonly int StrenghtCount;

        public readonly int StaminaCount;

        public readonly int DexterityCount;

        public readonly int IntelligenceCount;

        public ModifyStatusPacket(NetPacketBase packet)
        {
            this.StrenghtCount = packet.Read<int>();
            this.StaminaCount = packet.Read<int>();
            this.DexterityCount = packet.Read<int>();
            this.IntelligenceCount = packet.Read<int>();
        }
    }
}
