using Ether.Network.Packets;
using Rhisis.Core.Common;
using Rhisis.Core.Common.Game.Structures;
using Rhisis.World.Game.Structures;
using Rhisis.World.Systems.Taskbar;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Components
{
    public class TaskbarItemContainerComponent
    {
        public int MaxItemCapacity { get; }

        public int MaxLevelCapacity { get; }

        public List<List<Shortcut>> Shortcuts { get; }

        public TaskbarItemContainerComponent(int maxItemCapacity, int maxLevelCapacity)
        {
            MaxItemCapacity = maxItemCapacity;
            MaxLevelCapacity = maxLevelCapacity;
            Shortcuts = new List<List<Shortcut>>(new List<Shortcut>[maxLevelCapacity]);

            for(int i = 0; i < Shortcuts.Count; i++)
                Shortcuts[i] = new List<Shortcut>(new Shortcut[maxItemCapacity]);
        }

        public void CreateShortcut(Shortcut shortcut, int slotLevelIndex)
        {
            if (slotLevelIndex < 0 || slotLevelIndex >= TaskbarSystem.MaxTaskbarItemLevels)
                return;

            if (shortcut.SlotIndex < 0 || shortcut.SlotIndex >= TaskbarSystem.MaxTaskbarItems)
                return;

            Shortcuts[slotLevelIndex][shortcut.SlotIndex] = shortcut;
        }

        public void RemoveShortcut(int slotLevelIndex, int slotIndex)
        {
            if (slotLevelIndex < 0 || slotLevelIndex >= TaskbarSystem.MaxTaskbarItemLevels)
                return;

            if (slotIndex < 0 || slotIndex >= TaskbarSystem.MaxTaskbarItems)
                return;

            Shortcuts[slotLevelIndex][slotIndex] = null;
        }

        public int Count => Shortcuts.Sum(x => x.Count(y => y != null && y.Type != ShortcutType.None));

        public void Serialize(INetPacketStream packet)
        {
            packet.Write(Count);
            for(int level = 0; level < TaskbarSystem.MaxTaskbarItemLevels; level++)
            {
                for(int slot = 0; slot < TaskbarSystem.MaxTaskbarItems; slot++)
                {
                    if(Shortcuts[level][slot] != null && Shortcuts[level][slot].Type != ShortcutType.None)
                    {
                        packet.Write(level);
                        Shortcuts[level][slot].Serialize(packet);
                    }                    
                }
            }
        }
    }
}