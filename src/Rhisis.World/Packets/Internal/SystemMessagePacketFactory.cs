using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Client;
using Rhisis.World.Game.Entities;
using Sylver.Network.Server;

namespace Rhisis.World.Packets.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class SystemMessagePacketFactory : PacketFactoryBase, ISystemMessagePacketFactory
    {
        /// <inheritdoc />
        public void SendSystemMessage(IPlayerEntity player, string message)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(player.Id, SnapshotType.GMCHAT);
            packet.Write(message.Length);
            packet.Write(message);

            SendToVisible(packet, player, true);
        }
    }
}
