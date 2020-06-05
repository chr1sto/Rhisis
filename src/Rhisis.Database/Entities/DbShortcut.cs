using Rhisis.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace Rhisis.Database.Entities
{
    public sealed class DbShortcut
    {
        /// <summary>
        /// Gets or sets the target taskbar of the shortcut.
        /// </summary>
        public ShortcutTaskbarTarget TargetTaskbar { get; set; }

        /// <summary>
        /// Gets or sets the shortcut type.
        /// </summary>
        public ShortcutType Type { get; set; }

        /// <summary>
        /// Gets or sets the shortcut object type.
        /// </summary>
        public ShortcutObjectType ObjectType { get; set; }

        /// <summary>
        /// Gets or sets the shortcut slot.
        /// </summary>
        public int Slot { get; set; }

        /// <summary>
        /// Gets or sets the slot level index.
        /// </summary>
        /// <remarks>
        /// Available for the item taskbar.
        /// </remarks>
        public int SlotLevelIndex { get; set; }

        /// <summary>
        /// Gets or sets the object item slot in the inventory.
        /// </summary>
        /// <remarks>
        /// Only used when the <see cref="Type"/> is a <see cref="ShortcutType.Item"/>.
        /// </remarks>
        public int? ObjectItemSlot { get; set; }

        /// <summary>
        /// Gets or sets the shortcut index in the container.
        /// </summary>
        /// <remarks>
        /// Not sure if it's used thought.
        /// </remarks>
        public uint ObjectIndex { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public uint UserId { get; set; }

        /// <summary>
        /// Gets or sets the additionnal data.
        /// </summary>
        public uint ObjectData { get; set; }

        /// <summary>
        /// Gets or sets the chat text of the shortcut.
        /// </summary>
        [Encrypted]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the character id that posseses this shortcut.
        /// </summary>
        public int CharacterId { get; set; }

        /// <summary>
        /// Gets or sets the character instance that posseses this shortcut.
        /// </summary>
        public DbCharacter Character { get; set; }

        public DbShortcut()
        {
        }

        public DbShortcut(ShortcutTaskbarTarget targetTaskbar, int slotIndex, ShortcutType type, int? objectId, ShortcutObjectType objectType, uint objectIndex, uint userId, uint objectData, string text)
            : this(targetTaskbar, -1, slotIndex, type, objectId, objectType, objectIndex, userId, objectData, text)
        {
        }

        public DbShortcut(ShortcutTaskbarTarget targetTaskbar, int slotLevelIndex, int slotIndex, ShortcutType type, int? objectId, ShortcutObjectType objectType, uint objectIndex, uint userId, uint objectData, string text)
        {
            TargetTaskbar = targetTaskbar;
            SlotLevelIndex = slotLevelIndex;
            Slot = slotIndex;
            Type = type;
            ObjectItemSlot = objectId;
            ObjectType = objectType;
            ObjectIndex = objectIndex;
            UserId = userId;
            ObjectData = objectData;
            Text = text;
        }
    }
}