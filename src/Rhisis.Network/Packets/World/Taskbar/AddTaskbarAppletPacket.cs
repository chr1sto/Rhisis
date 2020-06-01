using Sylver.Network.Data;
using Rhisis.Core.Common;

namespace Rhisis.Network.Packets.World.Taskbar
{
    public class AddTaskbarAppletPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the taskbar slot index.
        /// </summary>
        public int SlotIndex { get; private set; }

        /// <summary>
        /// Gets the taskbar shortcut type.
        /// </summary>
        public ShortcutType Type { get; private set; }

        /// <summary>
        /// Gets the taskbar object id.
        /// </summary>
        public uint ObjectId { get; private set; }

        /// <summary>
        /// Gets the taskbar shortcut object type.
        /// </summary>
        public ShortcutObjectType ObjectType { get; private set; }

        /// <summary>
        /// Gets the taskbar object index.
        /// </summary>
        public uint ObjectIndex { get; private set; }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        public uint UserId { get; private set; }

        /// <summary>
        /// Gets the taskbar object data.
        /// </summary>
        public uint ObjectData { get; private set; }

        /// <summary>
        /// Gets the taskbar text object.
        /// </summary>
        public string Text { get; private set; }

        /// <inheritdoc />
        public virtual void Deserialize(INetPacketStream packet)
        {
            SlotIndex = packet.Read<byte>();
            Type = (ShortcutType)packet.Read<uint>();
            ObjectId = packet.Read<uint>();
            ObjectType = (ShortcutObjectType)packet.Read<uint>();
            ObjectIndex = packet.Read<uint>();
            UserId = packet.Read<uint>();
            ObjectData = packet.Read<uint>();
            Text = Type == ShortcutType.Chat ? packet.Read<string>() : string.Empty;
        }
    }
}