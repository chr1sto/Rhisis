using Rhisis.Core.Common;
using Rhisis.Core.Common.Game.Structures;
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
            Objects = new List<List<Shortcut>>(new List<Shortcut>[maxCapacity]);

            for (int i = 0; i < Objects.Count; i++)
                Objects[i] = new List<Shortcut>(new Shortcut[maxLevelCapacity]);
        }

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
    }
}