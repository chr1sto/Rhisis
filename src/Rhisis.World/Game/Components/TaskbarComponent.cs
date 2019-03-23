using Ether.Network.Packets;
using Rhisis.World.Game.Entities;
using Rhisis.World.Systems.Taskbar;

namespace Rhisis.World.Game.Components
{
    public class TaskbarComponent
    {
        public TaskbarAppletContainerComponent Applets { get; }

        public TaskbarItemContainerComponent Items { get; }

        public TaskbarQueueContainerComponent Queue { get; }

        public int ActionPoints { get; set; } = 100;

        public TaskbarComponent()
        {
            Applets = new TaskbarAppletContainerComponent(TaskbarSystem.MaxTaskbarApplets);
            Items = new TaskbarItemContainerComponent(TaskbarSystem.MaxTaskbarItems, TaskbarSystem.MaxTaskbarItemLevels);
            Queue = new TaskbarQueueContainerComponent(TaskbarSystem.MaxTaskbarQueue);
        }

        public void Serialize(INetPacketStream packet)
        {
            Applets.Serialize(packet);
            Items.Serialize(packet);
            Queue.Serialize(packet);
            packet.Write(ActionPoints);
        }
    }
}