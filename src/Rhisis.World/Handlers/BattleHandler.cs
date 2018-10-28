using Ether.Network.Packets;
using NLog;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World;

namespace Rhisis.World.Handlers
{
    public static class BattleHandler
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        [PacketHandler(PacketType.MELEE_ATTACK)]
        public static void OnMeleeAttack(WorldClient client, INetPacketStream packet)
        {
            var meleePacket = new MeleeAttackPacket(packet);

            Logger.Debug($"message: {meleePacket.AttackMessage}; Target: {meleePacket.ObjectId}; AttackSpeed: {meleePacket.WeaponAttackSpeed}");
        }
    }
}
