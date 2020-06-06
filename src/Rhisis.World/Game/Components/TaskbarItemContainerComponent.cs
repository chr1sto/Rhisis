using Rhisis.Core.Common;
using Rhisis.World.Game.Structures;
using Sylver.Network.Data;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Components
{
    public class TaskbarItemContainerComponent : ObjectContainerComponent<List<Shortcut>>
    {
        /// <summary>
        /// Gets the number of items in the item taskbar.
        /// </summary>
        public override int Count => Objects.Sum(x => x.Count(y => y != null && y.Type != ShortcutType.None));

        /// <summary>
        /// Gets the taskbar item max level capacity per slot.
        /// </summary>
        public int MaxLevelCapacity { get; }

        /// <summary>
        /// Creates a new <see cref="TaskbarItemContainerComponent"/> instance.
        /// </summary>
        /// <param name="maxCapacity">Taskbar max capacity (number of slot levels)</param>
        /// <param name="maxLevelCapacity">Taskbar max capacity per slot level.</param>
        public TaskbarItemContainerComponent(int maxCapacity, int maxLevelCapacity)
            : base(maxCapacity)
        {
            MaxLevelCapacity = maxLevelCapacity;
            _objects = new List<List<Shortcut>>(new List<Shortcut>[maxCapacity]);

            for (int i = 0; i < Objects.Count; i++)
            {
                _objects[i] = new List<Shortcut>(new Shortcut[maxLevelCapacity]);
            }
        }

        /// <summary>
        /// Adds a new shortcut to the item taskbar at a given level and slot.
        /// </summary>
        /// <param name="shortcut">Shortcut to add.</param>
        /// <param name="levelIndex">Item taskbar level.</param>
        /// <param name="slotIndex">Shortcut slot.</param>
        /// <returns>True if the shortchut is added successfuly; false otherwise.</returns>
        public bool Add(Shortcut shortcut, int levelIndex, int slotIndex) => SetObjectAt(levelIndex, slotIndex, shortcut);

        /// <summary>
        /// Removes a shortcut from the item taskbar at a given level and slot.
        /// </summary>
        /// <param name="levelIndex">Item taskbar level.</param>
        /// <param name="slotIndex">Shortcut slot.</param>
        /// <returns>True if the shortchut is removed successfuly; false otherwise.</returns>
        public bool RemoveAt(int levelIndex, int slotIndex) => SetObjectAt(levelIndex, slotIndex, null);

        /// <inheritdoc />
        public override void Serialize(INetPacketStream packet)
        {
            packet.Write(Count);

            for (int level = 0; level < MaxCapacity; level++)
            {
                for (int slot = 0; slot < MaxLevelCapacity; slot++)
                {
                    if (Objects[level][slot] != null && Objects[level][slot].Type != ShortcutType.None)
                    {
                        packet.Write(level);
                        Objects[level][slot].Serialize(packet);
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the given level index and slot are out of range.
        /// </summary>
        /// <param name="levelIndex">Item taskbar level index.</param>
        /// <param name="slotIndex">Shortcut slot index.</param>
        /// <returns>True if the level or slot is out of range; false otherwise.</returns>
        private bool IsOutOfRange(int levelIndex, int slotIndex)
        {
            if (levelIndex < 0 || levelIndex >= MaxLevelCapacity)
            {
                return true;
            }

            if (slotIndex < 0 || slotIndex >= MaxCapacity)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets a shortcut object at a given level and slot index.
        /// </summary>
        /// <param name="levelIndex">Item taskbar level.</param>
        /// <param name="slotIndex">Shortcut slot.</param>
        /// <param name="shortcut">Shortcut object.</param>
        /// <returns>True if the operation succeded; false otherwise.</returns>
        private bool SetObjectAt(int levelIndex, int slotIndex, Shortcut shortcut)
        {
            if (IsOutOfRange(levelIndex, slotIndex))
            {
                return false;
            }

            _objects[levelIndex][slotIndex] = shortcut;

            return true;
        }
    }
}