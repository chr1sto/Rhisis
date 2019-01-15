using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets
{
    public partial class WorldPacketFactory
    {
        public static void SendChangeFace(IPlayerEntity entity, uint faceId)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(entity.Id, SnapshotType.CHANGEFACE);
                packet.Write(entity.PlayerData.Id);
                packet.Write(faceId);

                SendToVisible(packet, entity, true);
            }
        }

        public static void SendSetHair(IPlayerEntity entity, byte hairId, byte r, byte g, byte b)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(entity.Id, SnapshotType.SET_HAIR);
                packet.Write(hairId);
                packet.Write(r);
                packet.Write(g);
                packet.Write(b);

                SendToVisible(packet, entity, true);
            }
        }
    }
}
