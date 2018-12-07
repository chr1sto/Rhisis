using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Common;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets
{
    public static partial class WorldPacketFactory
    {
        public static void SendAddDamage(IPlayerEntity player, ILivingEntity damageReceiver, ILivingEntity damageSender, AttackFlags atkFlags, int damage)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(damageReceiver.Id, SnapshotType.DAMAGE);
                packet.Write(damageSender.Id);
                packet.Write((uint)damage);
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