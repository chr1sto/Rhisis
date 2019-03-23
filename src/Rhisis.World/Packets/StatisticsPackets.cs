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

                packet.Write<uint>(player.Statistics.Strength);
                packet.Write<uint>(player.Statistics.Stamina);
                packet.Write<uint>(player.Statistics.Dexterity);
                packet.Write<uint>(player.Statistics.Intelligence);
                packet.Write(0);
                packet.Write<uint>(player.Statistics.StatPoints);

                player.Connection.Send(packet);
            }
        }
    }
}
