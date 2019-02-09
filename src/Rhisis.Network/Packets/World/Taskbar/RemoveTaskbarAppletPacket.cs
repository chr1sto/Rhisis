using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets.World.Taskbar
{
    public struct RemoveTaskbarAppletPacket : IEquatable<RemoveTaskbarAppletPacket>
    {
        public int SlotIndex { get; }

        public RemoveTaskbarAppletPacket(INetPacketStream packet)
        {
            SlotIndex = packet.Read<byte>();
        }

        public bool Equals(RemoveTaskbarAppletPacket other)
        {
            return SlotIndex == other.SlotIndex;
        }
    }
}