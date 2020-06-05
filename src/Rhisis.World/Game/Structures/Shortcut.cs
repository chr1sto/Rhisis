using Rhisis.Core.Common;
using Sylver.Network.Data;

namespace Rhisis.World.Game.Structures
{
    public sealed class Shortcut
    {
        /// <summary>
        /// Gets the shortcut slot in its taskbar container.
        /// </summary>
        public int Slot { get; }

        /// <summary>
        /// Gets the shortcut type.
        /// </summary>
        public ShortcutType Type { get; }

        /// <summary>
        /// Gets the shortcut item index in the inventory.
        /// </summary>
        /// <remarks>
        /// Only available when <see cref="Type"/> is a <see cref="ShortcutType.Item"/>.
        /// </remarks>
        public int? ItemIndex { get; }

        /// <summary>
        /// Gets the shortcut object type.
        /// </summary>
        public ShortcutObjectType ObjectType { get; }

        /// <summary>
        /// Gets the shortcut object index in the taskbar container.
        /// </summary>
        public uint ObjectIndex { get; }

        /// <summary>
        /// Gets the shortcut user id.
        /// </summary>
        /// <remarks>
        /// This doesn't seem to be used.
        /// </remarks>
        public uint UserId { get; }

        /// <summary>
        /// Gets the shortcut additionnal data.
        /// </summary>
        /// <remarks>
        /// This seems to be used in official files to store additionnal data.
        /// Not used for now.
        /// </remarks>
        public uint ObjectData { get; }

        /// <summary>
        /// Gets the shortcut text.
        /// </summary>
        /// <remarks>
        /// Only available when <see cref="Type"/> is a <see cref="ShortcutType.Chat"/>.
        /// </remarks>
        public string Text { get; }

        /// <summary>
        /// Creates a new <see cref="Shortcut"/> instance.
        /// </summary>
        /// <param name="slot">Shortcut slot in its taskbar container.</param>
        /// <param name="type">Shortcut type.</param>
        /// <param name="itemIndex">Shortcut target item index in inventory.</param>
        /// <param name="shortcutObjectType">Shortcut object type.</param>
        /// <param name="objIndex">Shortcut index in its taskbar container.</param>
        /// <param name="userId">Shortcut user id. Not used.</param>
        /// <param name="objData">Shortcut additionnal data.</param>
        /// <param name="text">Shortcut text.</param>
        public Shortcut(int slot, ShortcutType type, int? itemIndex, ShortcutObjectType shortcutObjectType, uint objIndex, uint userId, uint objData, string text)
        {
            Slot = slot;
            Type = type;
            ItemIndex = itemIndex;
            ObjectType = shortcutObjectType;
            ObjectIndex = objIndex;
            UserId = userId;
            ObjectData = objData;
            Text = text;
        }

        /// <summary>
        /// Serializes this shortcut into the give packet stream.
        /// </summary>
        /// <param name="packet">Packet stream.</param>
        public void Serialize(INetPacketStream packet)
        {
            packet.Write(Slot);
            packet.Write((uint)Type);
            packet.Write((uint)ItemIndex.GetValueOrDefault());
            packet.Write((uint)ObjectType);
            packet.Write(ObjectIndex);
            packet.Write(UserId);
            packet.Write(ObjectData);

            if (Type == ShortcutType.Chat)
            {
                packet.Write(Text);
            }
        }
    }
}