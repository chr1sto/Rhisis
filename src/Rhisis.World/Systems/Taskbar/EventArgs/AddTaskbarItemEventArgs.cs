using Rhisis.Core.Common;
using Rhisis.World.Game.Core.Systems;

namespace Rhisis.World.Systems.Taskbar.EventArgs
{
    public class AddTaskbarItemEventArgs : SystemEventArgs
    {
        public int SlotLevelIndex { get; }

        public int SlotIndex { get; }

        public ShortcutType Type { get; }

        public uint ObjId { get; }

        public ShortcutObjectType ObjectType { get; }

        public uint ObjIndex { get; }

        public uint UserId { get; }

        public uint ObjData { get; }

        public string Text { get; }

        public AddTaskbarItemEventArgs(int slotLevelIndex, int slotIndex, ShortcutType type, uint objId, ShortcutObjectType objectType, uint objIndex, uint userId, uint objData, string text)
        {
            SlotLevelIndex = slotLevelIndex;
            SlotIndex = slotIndex;
            Type = type;
            ObjId = objId;
            ObjectType = objectType;
            ObjIndex = objIndex;
            UserId = userId;
            ObjData = objData;
            Text = text;
        }

        public override bool CheckArguments()
        {
            return SlotIndex >= 0 && SlotIndex < TaskbarSystem.MaxTaskbarItems && SlotLevelIndex >= 0 && SlotLevelIndex < TaskbarSystem.MaxTaskbarItemLevels;
        }
    }
}
