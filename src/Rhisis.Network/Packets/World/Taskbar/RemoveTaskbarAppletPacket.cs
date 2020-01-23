using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Taskbar
{
    public class RemoveTaskbarAppletPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the slot index from the applet taskbar to be removed.
        /// </summary>
        public int SlotIndex { get; private set; }

        /// <inheritdoc />
        public virtual void Deserialize(INetPacketStream packet)
        {
            SlotIndex = packet.Read<byte>();
        }
    }
}