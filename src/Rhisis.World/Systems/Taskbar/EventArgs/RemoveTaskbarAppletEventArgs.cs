using Rhisis.World.Game.Core.Systems;

namespace Rhisis.World.Systems.Taskbar.EventArgs
{
    public class RemoveTaskbarAppletEventArgs : SystemEventArgs
    {
        public int SlotIndex { get; }

        public RemoveTaskbarAppletEventArgs(int slotIndex)
        {
            SlotIndex = slotIndex;
        }

        public override bool CheckArguments()
        {
            return SlotIndex >= 0 && SlotIndex < TaskbarSystem.MaxTaskbarApplets;
        }
    }
}