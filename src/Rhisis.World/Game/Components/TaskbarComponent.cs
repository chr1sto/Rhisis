using Sylver.Network.Data;

namespace Rhisis.World.Game.Components
{
    public class TaskbarComponent
    {
        public TaskbarAppletContainerComponent Applets { get; set; }

        public TaskbarItemContainerComponent Items { get; set; }

        public TaskbarQueueContainerComponent Queue { get; set; }

        public int ActionPoints { get; set; } = 100;

        public void Serialize(INetPacketStream packet)
        {
            Applets.Serialize(packet);
            Items.Serialize(packet);
            Queue.Serialize(packet);
            packet.Write(ActionPoints);
        }
    }
}