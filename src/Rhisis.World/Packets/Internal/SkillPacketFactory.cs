using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class SkillPacketFactory : PacketFactoryBase, ISkillPacketFactory
    {
        /// <inheritdoc />
        public void SendSkillTreeUpdate(IPlayerEntity player)
        {
            using var packet = new FFPacket();

            packet.StartNewMergedPacket(player.Id, SnapshotType.DOUSESKILLPOINT);
            player.SkillTree.Serialize(packet);
            packet.Write((int)player.Statistics.SkillPoints);

            SendToPlayer(player, packet);
        }
    }
}