using Ether.Network.Packets;

namespace Rhisis.Core.Common.Game.Structures
{
    public class Shortcut
    {
        public int SlotIndex { get; }

        public ShortcutType Type { get; }

        public uint ObjId { get; }

        public ShortcutObjectType ObjectType { get; }

        public uint ObjIndex { get; }

        public uint UserId { get; }

        public uint ObjData { get; }

        public string Text { get; }

        public Shortcut(int slotIndex, ShortcutType type, uint objId, ShortcutObjectType shortcutObjectType, uint objIndex, uint userId, uint objData, string text)
        {
            SlotIndex = slotIndex;
            Type = type;
            ObjId = objId;
            ObjectType = shortcutObjectType;
            ObjIndex = objIndex;
            UserId = userId;
            ObjData = objData;
            Text = text;
        }

        public void Serialize(INetPacketStream packet)
        {
            packet.Write(SlotIndex);
            packet.Write((uint)Type);
            packet.Write(ObjId);
            packet.Write((uint)ObjectType);
            packet.Write(ObjIndex);
            packet.Write(UserId);
            packet.Write(ObjData);

            if (Type == ShortcutType.Chat)
                packet.Write(Text);
        }
    }
}