using Ether.Network.Packets;
using Rhisis.Core.Common;
using System;

namespace Rhisis.Network.Packets.World.Taskbar
{
    public struct AddTaskbarAppletPacket : IEquatable<AddTaskbarAppletPacket>
    {
        public int SlotIndex { get; }

        public ShortcutType Type { get; }

        public uint ObjectId { get; }

        public ShortcutObjectType ObjectType { get; }

        public uint ObjectIndex { get; }

        public uint UserId { get; }

        public uint ObjectData { get; }

        public string Text { get; }

        public AddTaskbarAppletPacket(INetPacketStream packet)
        {
            SlotIndex = packet.Read<byte>();
            Type = (ShortcutType)packet.Read<uint>();
            ObjectId = packet.Read<uint>();
            ObjectType = (ShortcutObjectType)packet.Read<uint>();
            ObjectIndex = packet.Read<uint>();
            UserId = packet.Read<uint>();
            ObjectData = packet.Read<uint>();
            Text = null;

            if (Type == ShortcutType.Chat)
                Text = packet.Read<string>();
        }

        public bool Equals(AddTaskbarAppletPacket other)
        {
            return
                SlotIndex == other.SlotIndex &&
                Type == other.Type &&
                ObjectId == other.ObjectId &&
                ObjectType == other.ObjectType &&
                ObjectIndex == other.ObjectIndex &&
                UserId == other.UserId &&
                ObjectData == other.ObjectData &&
                Text == other.Text;
        }
    }
}