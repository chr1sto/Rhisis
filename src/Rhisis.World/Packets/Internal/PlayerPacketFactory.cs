using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;
using System.Linq;

namespace Rhisis.World.Packets.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class PlayerPacketFactory : PacketFactoryBase, IPlayerPacketFactory
    {
        /// <inheritdoc />
        public void SendPlayerTeleport(IPlayerEntity player)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(player.Id, SnapshotType.SETPOS);
            packet.Write(player.Object.Position.X);
            packet.Write(player.Object.Position.Y);
            packet.Write(player.Object.Position.Z);
            packet.Write(player.Object.MapId);

            packet.StartNewMergedPacket(player.Id, SnapshotType.WORLD_READINFO);
            packet.Write(player.Object.MapId);
            packet.Write(player.Object.Position.X);
            packet.Write(player.Object.Position.Y);
            packet.Write(player.Object.Position.Z);

            SendToVisible(packet, player, sendToPlayer: true);
        }

        /// <inheritdoc />
        public void SendPlayerReplace(IPlayerEntity player)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(player.Id, SnapshotType.REPLACE);
            packet.Write(player.Object.MapId);
            packet.Write(player.Object.Position.X);
            packet.Write(player.Object.Position.Y);
            packet.Write(player.Object.Position.Z);

            SendToPlayer(player, packet);
        }

        /// <inheritdoc />
        public void SendPlayerUpdateState(IPlayerEntity player)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(player.Id, SnapshotType.SETSTATE);

            packet.Write(player.Attributes[DefineAttributes.STR]);
            packet.Write(player.Attributes[DefineAttributes.STA]);
            packet.Write(player.Attributes[DefineAttributes.DEX]);
            packet.Write(player.Attributes[DefineAttributes.INT]);
            packet.Write(0);
            packet.Write<uint>(player.Statistics.StatPoints);

            SendToPlayer(player, packet);
        }

        /// <inheritdoc />
        public void SendPlayerStatsPoints(IPlayerEntity player)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(player.Id, SnapshotType.SET_GROWTH_LEARNING_POINT);
            packet.Write((long)player.Statistics.StatPoints);
            packet.Write<long>(0);

            SendToPlayer(player, packet);
        }

        /// <inheritdoc />
        public void SendPlayerSetLevel(IPlayerEntity player, int level)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(player.Id, SnapshotType.SETLEVEL);
            packet.Write((short)level);

            SendToVisible(packet, player);
        }

        /// <inheritdoc />
        public void SendPlayerExperience(IPlayerEntity player)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(player.Id, SnapshotType.SETEXPERIENCE);
            packet.Write(player.PlayerData.Experience);
            packet.Write((short)player.Object.Level);
            packet.Write(0);
            packet.Write((int)player.Statistics.SkillPoints);
            packet.Write(long.MaxValue); // death exp
            packet.Write((short)player.PlayerData.DeathLevel);

            SendToPlayer(player, packet);
        }

        /// <inheritdoc />
        public void SendPlayerRevival(IPlayerEntity player)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(player.Id, SnapshotType.REVIVAL_TO_LODESTAR);

            SendToVisible(packet, player, sendToPlayer: true);
        }

        /// <inheritdoc />
        public void SendPlayerJobUpdate(IPlayerEntity player)
        {
            using var packet = new FFPacket();

            packet.StartNewMergedPacket(player.Id, SnapshotType.SET_JOB_SKILL);
            packet.Write((int)player.PlayerData.Job);
            player.SkillTree.Serialize(packet);
            packet.Write(Enumerable.Repeat((byte)0, 128).ToArray(), 0, 128);

            SendToVisible(packet, player, sendToPlayer: true);
        }
    }
}
