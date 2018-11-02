using Ether.Network.Packets;
using NLog;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Follow;

namespace Rhisis.World.Handlers
{
    public static class BattleHandler
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        [PacketHandler(PacketType.MELEE_ATTACK)]
        public static void OnMeleeAttack(WorldClient client, INetPacketStream packet)
        {
            var meleePacket = new MeleeAttackPacket(packet);
            var target = client.Player.Object.CurrentLayer.FindEntity<IMonsterEntity>(meleePacket.ObjectId);

            if (target.MovableComponent.SpeedFactor != 2f)
            {
                target.MovableComponent.SpeedFactor = 2f;
                WorldPacketFactory.SendSpeedFactor(target, target.MovableComponent.SpeedFactor);
            }

            target.NotifySystem<FollowSystem>(new FollowEventArgs(client.Player.Id, 1f));

            Logger.Debug($"message: {meleePacket.AttackMessage}; Target: {meleePacket.ObjectId}; AttackSpeed: {meleePacket.WeaponAttackSpeed}");
            Logger.Debug($"{client.Player.Object.Name} is attacking {target.Object.Name}");
        }
    }
}
