using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets.World.Taskbar
{
    public struct RemoveTaskbarItemPacket : IEquatable<RemoveTaskbarItemPacket>
    {
        public int SlotLevelIndex { get; }

        public int SlotIndex { get; }

        public RemoveTaskbarItemPacket(INetPacketStream packet)
        {
            SlotLevelIndex = packet.Read<byte>();
            SlotIndex = packet.Read<byte>();
        }

        public bool Equals(RemoveTaskbarItemPacket other)
        {
            return SlotLevelIndex == other.SlotLevelIndex && SlotIndex == other.SlotIndex;
        }
    }
}