using Rhisis.Core.Data;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets
{
    public static partial class WorldPacketFactory
    {
        public static void SendUpdateState(IPlayerEntity player)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.Id, SnapshotType.SETSTATE);

                packet.Write(player.Attributes[DefineAttributes.STR]);
                packet.Write(player.Attributes[DefineAttributes.STA]);
                packet.Write(player.Attributes[DefineAttributes.DEX]);
                packet.Write(player.Attributes[DefineAttributes.INT]);
                packet.Write(0);
                packet.Write<uint>(player.Statistics.StatPoints);

                player.Connection.Send(packet);
            }
        }

        public static void SendPlayerStatsPoints(IPlayerEntity player)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.Id, SnapshotType.SET_GROWTH_LEARNING_POINT);
                packet.Write((long)player.Statistics.StatPoints);
                packet.Write<long>(0);

                player.Connection.Send(packet);
            }
        }
    }
}
