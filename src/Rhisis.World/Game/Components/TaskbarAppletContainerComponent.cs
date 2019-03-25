using Ether.Network.Packets;
using Rhisis.Core.Common;
using Rhisis.Core.Common.Game.Structures;
using Rhisis.World.Game.Entities;
using Rhisis.World.Systems.Taskbar;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Components
{
    public class TaskbarAppletContainerComponent
    {
        public int MaxCapacity { get; }

        public List<Shortcut> Shortcuts { get; }

        public TaskbarAppletContainerComponent(int maxCapacity)
        {
            MaxCapacity = maxCapacity;
            Shortcuts = new List<Shortcut>(new Shortcut[maxCapacity]);
        }

        public bool IsSlotAvailable(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= TaskbarSystem.MaxTaskbarApplets)
                return false;

            return Shortcuts[slotIndex] == null;
        }

        public void CreateShortcut(Shortcut shortcut)
        {
            if (shortcut.SlotIndex < 0 || shortcut.SlotIndex >= TaskbarSystem.MaxTaskbarApplets)
                return;

            if (Shortcuts[shortcut.SlotIndex] != null)
                return;

            Shortcuts[shortcut.SlotIndex] = shortcut;
        }

        public void RemoveShortcut(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= TaskbarSystem.MaxTaskbarApplets)
                return;

            Shortcuts[slotIndex] = null;
        }

        public int Count => this.Shortcuts.Count(x => x != null && x.Type != ShortcutType.None);

        public void Serialize(INetPacketStream packet)
        {
            packet.Write(Count);
            for(int i = 0; i < MaxCapacity; i++)
            {
                if(Shortcuts[i] != null && Shortcuts[i].Type != ShortcutType.None)
                {
                    Shortcuts[i].Serialize(packet);
                }
            }
        }
    }
}