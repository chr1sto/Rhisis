using Rhisis.Core.Data;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Core;

namespace Rhisis.World.Packets
{
    public static partial class WorldPacketFactory
    {
        public static void SendSpeedFactor(IEntity entity, float speedFactor)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(entity.Id, SnapshotType.SET_SPEED_FACTOR);
                packet.Write(speedFactor);

                SendToVisible(packet, entity);
            }
        }

        public static void SendMotion(IEntity entity, ObjectMessageType objectMessage)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(entity.Id, SnapshotType.MOTION);
                packet.Write((int)objectMessage);

                SendToVisible(packet, entity, sendToPlayer: true);
            }
        }
    }
}
