using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets
{
    public static partial class WorldPacketFactory
    {
        public static void SendItemMove(IPlayerEntity entity, byte sourceSlot, byte destinationSlot)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(entity.Id, SnapshotType.MOVEITEM);
                packet.Write<byte>(0);
                packet.Write(sourceSlot);
                packet.Write(destinationSlot);

                entity.Connection.Send(packet);
            }
        }
    }
}
