using Rhisis.World.Game.Core.Systems;

namespace Rhisis.World.Systems.Taskbar.EventArgs
{
    public class RemoveTaskbarItemEventArgs : SystemEventArgs
    {
        public int SlotLevelIndex { get; }

        public int SlotIndex { get; }

        public RemoveTaskbarItemEventArgs(int slotLevelIndex, int slotIndex)
        {
            SlotLevelIndex = slotLevelIndex;
            SlotIndex = slotIndex;
        }

        public override bool CheckArguments()
        {
            return SlotIndex >= 0 && SlotIndex < TaskbarSystem.MaxTaskbarItems && SlotLevelIndex >= 0 && SlotLevelIndex < TaskbarSystem.MaxTaskbarItemLevels;
        }
    }
}