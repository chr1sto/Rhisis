using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets
{
    public static partial class WorldPacketFactory
    {
        public static void SendChat(IPlayerEntity player, string message)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.Id, SnapshotType.CHAT);
                packet.Write(message);

                player.PlayerComponent.Connection.Send(packet);
                SendToVisible(packet, player);
            }
        }
    }
}
