using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Common;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class BattlePacketFactory : PacketFactoryBase, IBattlePacketFactory
    {
        /// <inheritdoc />
        public void SendAddDamage(ILivingEntity defender, ILivingEntity attacker, AttackFlags attackFlags, int damage)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(defender.Id, SnapshotType.DAMAGE);
            packet.Write(attacker.Id);
            packet.Write(damage);
            packet.Write((int)attackFlags);

            if (attackFlags.HasFlag(AttackFlags.AF_FLYING))
            {
                packet.Write(defender.Moves.DestinationPosition.X);
                packet.Write(defender.Moves.DestinationPosition.Y);
                packet.Write(defender.Moves.DestinationPosition.Z);
                packet.Write(defender.Object.Angle);
            }

            SendToVisible(packet, defender, sendToPlayer: true);
        }

        /// <inheritdoc />
        public void SendMeleeAttack(ILivingEntity attacker, ObjectMessageType motion, uint targetId, int unknwonParam, AttackFlags attackFlags)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(attacker.Id, SnapshotType.MELEE_ATTACK);
            packet.Write((int)motion);
            packet.Write(targetId);
            packet.Write(unknwonParam);
            packet.Write((int)attackFlags);

            SendToVisible(packet, attacker);
        }

        /// <inheritdoc />
        public void SendMagicAttack(ILivingEntity attacker, ObjectMessageType motion, uint target, int magicAttackPower, int projectileId)
        {
            using var packet = new FFPacket();

            packet.StartNewMergedPacket(attacker.Id, SnapshotType.MAGIC_ATTACK);
            packet.Write((int)motion);
            packet.Write(target);
            packet.Write(0); // unused parameter, always 0
            packet.Write(0); // unused parameter, always 0
            packet.Write(magicAttackPower);
            packet.Write(projectileId);

            SendToVisible(packet, attacker);
        }

        /// <inheritdoc />
        public void SendRangeAttack(ILivingEntity attacker, ObjectMessageType motion, uint targetId, int power, int projectileId)
        {
            using var packet = new FFPacket();

            packet.StartNewMergedPacket(attacker.Id, SnapshotType.RANGE_ATTACK);
            packet.Write((int)motion);
            packet.Write(targetId);
            packet.Write(power);
            packet.Write(0); // unused parameter, always 0
            packet.Write(projectileId);

            SendToVisible(packet, attacker);
        }

        /// <inheritdoc />
        public void SendDie(IPlayerEntity player, ILivingEntity deadEntity, ILivingEntity killerEntity, ObjectMessageType motion)
        {
            using var packet = new FFPacket();
            
            packet.StartNewMergedPacket(deadEntity.Id, SnapshotType.MOVERDEATH);
            packet.Write((int)motion);
            packet.Write(killerEntity.Id);

            SendToVisible(packet, player, sendToPlayer: true);
        }
    }
}