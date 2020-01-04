using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class ChatPacketFactory : PacketFactoryBase, IChatPacketFactory
    {
        /// <inheritdoc />
        public void SendChat(IPlayerEntity player, string message)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(player.Id, SnapshotType.CHAT);
            packet.Write(message);

            SendToVisible(packet, player, sendToPlayer: true);
        }

        /// <inheritdoc />
        public void SendChatTo(IWorldEntity fromEntity, IPlayerEntity toPlayer, string message)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(fromEntity.Id, SnapshotType.CHAT);
            packet.Write(message);

            SendToPlayer(toPlayer, packet);
        }
    }
}
