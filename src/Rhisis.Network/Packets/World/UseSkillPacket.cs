using System;
using Rhisis.Core.Data;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Packet structure received from the client when the player uses a skill.
    /// </summary>
    public class UseSkillPacket : IPacketDeserializer
    {
        public ushort Type { get; private set; }

        public ushort SkillIndex { get; private set; }

        public uint TargetObjectId { get; private set; }

        public SkillUseType UseType { get; private set; }

        public bool Control { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            this.Type = packet.Read<ushort>();
            this.SkillIndex = packet.Read<ushort>();
            this.TargetObjectId = packet.Read<uint>();
            this.UseType = (SkillUseType)packet.Read<int>();
            this.Control = Convert.ToBoolean(packet.Read<int>());
        }
    }
}