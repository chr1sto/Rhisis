using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;
using System;

namespace Rhisis.World.Packets
{
    public static partial class WorldPacketFactory
    {
        [Flags]
        public enum AttackFlags : uint
        {
            AF_GENERIC = 0x00000001,
            AF_MISS = 0x00000002,
            AF_RESERVED = 0x00000004,
            AF_MAGIC = 0x00000008,
            AF_MELEESKILL = 0x00000010,
            AF_MAGICSKILL = 0x00000020,
            AF_CRITICAL1 = 0x00000040,
            AF_CRITICAL2 = 0x00000080,
            AF_CRITICAL = 0x000000c0,
            AF_PUSH = 0x00000100,
            AF_PARRY = 0x00000200,
            AF_RESIST = 0x00000400,
            AF_STUN = 0x00000800,
            AF_BLOCKING = 0x00001000,
            AF_FORCE = 0x00002000,
            AF_RANGE = 0x00004000,
            AF_MONSTER_SP_CLIENT = 0x00008000,
            AF_FLYING = 0x10000000
        }

        public static void SendAddDamage(IPlayerEntity player, ILivingEntity damageReceiver, ILivingEntity damageSender, AttackFlags atkFlags, uint damage)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(damageReceiver.Id, SnapshotType.DAMAGE);
                packet.Write(damageSender.Id);
                packet.Write(damage);
                packet.Write((uint)atkFlags);

                if(atkFlags.HasFlag(AttackFlags.AF_FLYING))
                {
                    packet.Write(damageReceiver.Object.Position);
                    packet.Write(damageReceiver.Object.Angle);
                }

                player.Connection.Send(packet);
                SendToVisible(packet, player);
            }
        }
    }
}