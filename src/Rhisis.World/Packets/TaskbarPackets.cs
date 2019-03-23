using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets
{
    public partial class WorldPacketFactory
    {
        public static void SendSetActionPoint(IPlayerEntity player, int actionPoint)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.Id, SnapshotType.SETACTIONPOINT);
                packet.Write(actionPoint);

                player.Connection.Send(packet);
            }
        }
    }
}