using Rhisis.Core.Common;
using Rhisis.Core.Common.Game.Structures;
using Sylver.Network.Data;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Components
{
    public class TaskbarAppletContainerComponent : ObjectContainerComponent<Shortcut>
    {
        /// <summary>
        /// Gets the number of applets in the container.
        /// </summary>
        public override int Count => Objects.Count(x => x != null && x.Type != ShortcutType.None);

        /// <summary>
        /// Creates a new <see cref="TaskbarAppletContainerComponent"/> instance.
        /// </summary>
        /// <param name="maxCapacity">Taskbar applet max capacity.</param>
        public TaskbarAppletContainerComponent(int maxCapacity)
            : base(maxCapacity)
        {
            Objects = new List<Shortcut>(new Shortcut[maxCapacity]);
        }

        /// <inheritdoc />
        public override void Serialize(INetPacketStream packet)
        {
            packet.Write(Count);

            for (int i = 0; i < MaxCapacity; i++)
            {
                if (Objects[i] != null && Objects[i].Type != ShortcutType.None)
                {
                    Objects[i].Serialize(packet);
                }
            }
        }
    }
}