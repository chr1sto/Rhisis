using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World;
using Rhisis.World.Client;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Systems.Battle;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.World.Handlers
{
    [Handler]
    public class BattleHandler
    {
        private readonly ILogger<BattleHandler> _logger;
        private readonly IBattleSystem _battleSystem;

        /// <summary>
        /// Creates a new <see cref="BattleHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="battleSystem">Battle system.</param>
        public BattleHandler(ILogger<BattleHandler> logger, IBattleSystem battleSystem)
        {
            _logger = logger;
            _battleSystem = battleSystem;
        }

        /// <summary>
        /// On melee attack.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.MELEE_ATTACK)]
        public void OnMeleeAttack(IWorldClient client, MeleeAttackPacket packet)
        {
            var target = client.Player.FindEntity<IMonsterEntity>(packet.ObjectId);

            if (target == null)
            {
                _logger.LogError($"Cannot find target with object id {packet.ObjectId}");
                return;
            }

            Item weaponItem = client.Player.Inventory.GetEquipedItem(ItemPartType.RightWeapon) ?? client.Player.Hand;

            if (weaponItem != null && weaponItem.Data?.AttackSpeed != packet.WeaponAttackSpeed)
            {
                _logger.LogCritical($"Player {client.Player.Object.Name} has changed his weapon speed.");
                return;
            }

            _battleSystem.MeleeAttack(client.Player, target, packet.AttackMessage, packet.WeaponAttackSpeed);
        }

        /// <summary>
        /// On magic attack.
        /// </summary>
        /// <param name="client">Current client.</param>
        /// <param name="packet">Magic attack incoming packet.</param>
        [HandlerAction(PacketType.MAGIC_ATTACK)]
        public void OnMagicAttack(IWorldClient client, MagicAttackPacket packet)
        {
            var target = client.Player.FindEntity<IMonsterEntity>(packet.TargetObjectId);

            if (target == null)
            {
                _logger.LogError($"Cannot find target with object id {packet.TargetObjectId}");
                return;
            }

            if (packet.MagicPower < 0)
            {
                _logger.LogWarning($"Magic attack power cannot be less than 0.");
            }

            _battleSystem.MagicAttack(client.Player, target, packet.AttackMessage, Math.Max(0, packet.MagicPower));
        }

        /// <summary>
        /// On ranged attack.
        /// </summary>
        /// <param name="client">Current client.</param>
        /// <param name="packet">Range attack incoming packet.</param>
        [HandlerAction(PacketType.RANGE_ATTACK)]
        public void OnRangeAttack(IWorldClient client, RangeAttackPacket packet)
        {
            var target = client.Player.FindEntity<IMonsterEntity>(packet.ObjectId);

            if (target == null)
            {
                _logger.LogError($"Cannot find target with object id {packet.ObjectId}");
                return;
            }

            if (packet.Power < 0)
            {
                _logger.LogWarning($"Range attack power cannot be less than 0.");
            }

            _battleSystem.RangeAttack(client.Player, target, packet.AttackMessage, Math.Max(0, packet.Power));
        }
    }
}
