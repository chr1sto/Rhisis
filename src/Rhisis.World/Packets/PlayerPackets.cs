using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets
{
    public static partial class WorldPacketFactory
    {
        /// <summary>
        /// Sends player revival packet.
        /// </summary>
        /// <param name="entity"></param>
        public static void SendPlayerRevival(IEntity entity)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(entity.Id, SnapshotType.REVIVAL_TO_LODESTAR);

                SendToVisible(packet, entity, sendToPlayer: true);
            }
        }

        public static void SendReplaceObject(IPlayerEntity player)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.Id, SnapshotType.REPLACE);
                packet.Write(player.Object.MapId);
                packet.Write(player.Object.Position.X);
                packet.Write(player.Object.Position.Y);
                packet.Write(player.Object.Position.Z);

                player.Connection.Send(packet);
            }
        }
    }
}
