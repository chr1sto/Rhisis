using Rhisis.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhisis.Database.Entities
{
    [Table("shortcuts")]
    public sealed class DbShortcut : DbEntity
    {
        public int CharacterId { get; set; }

        public ShortcutTaskbarTarget TargetTaskbar { get; set; }

        public int? SlotLevelIndex { get; set; }

        public int SlotIndex { get; set; }

        public ShortcutType Type { get; set; }

        public uint ObjectId { get; set; }

        public ShortcutObjectType ObjectType { get; set; }

        public uint ObjectIndex { get; set; }

        public uint UserId { get; set; }

        public uint ObjectData { get; set; }

        public string Text { get; set; }

        public DbCharacter Character { get; set; }

        public DbShortcut()
        {
        }

        public DbShortcut(ShortcutTaskbarTarget targetTaskbar, int slotIndex, ShortcutType type, uint objectId, ShortcutObjectType objectType, uint objectIndex, uint userId, uint objectData, string text)
            : this(targetTaskbar, null, slotIndex, type, objectId, objectType, objectIndex, userId, objectData, text)
        {
        }

        public DbShortcut(ShortcutTaskbarTarget targetTaskbar, int? slotLevelIndex, int slotIndex, ShortcutType type, uint objectId, ShortcutObjectType objectType, uint objectIndex, uint userId, uint objectData, string text)
        {
            TargetTaskbar = targetTaskbar;
            SlotLevelIndex = slotLevelIndex;
            SlotIndex = slotIndex;
            Type = type;
            ObjectId = objectId;
            ObjectType = objectType;
            ObjectIndex = objectIndex;
            UserId = userId;
            ObjectData = objectData;
            Text = text;
        }
    }
}