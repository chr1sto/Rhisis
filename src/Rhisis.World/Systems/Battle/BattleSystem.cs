using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Common;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Drop;
using Rhisis.World.Systems.Experience;
using Rhisis.World.Systems.Projectile;
using System.Linq;

namespace Rhisis.World.Systems.Battle
{
    [Injectable]
    public class BattleSystem : IBattleSystem
    {
        private readonly ILogger<BattleSystem> _logger;
        private readonly IGameResources _gameResources;
        private readonly IDropSystem _dropSystem;
        private readonly IExperienceSystem _experienceSystem;
        private readonly IProjectileSystem _projectileSystem;
        private readonly IBattlePacketFactory _battlePacketFactory;
        private readonly IMoverPacketFactory _moverPacketFactory;
        private readonly WorldConfiguration _worldConfiguration;

        /// <summary>
        /// Creates a new <see cref="BattleSystem"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="worldConfiguration">World server configuration.</param>
        /// <param name="gameResources">Game resources.</param>
        /// <param name="dropSystem">Drop system.</param>
        /// <param name="experienceSystem">Experience system.</param>
        /// <param name="projectileSystem">Projectile system.</param>
        /// <param name="battlePacketFactory">Battle packet factory.</param>
        /// <param name="moverPacketFactory">Mover packet factory.</param>
        public BattleSystem(ILogger<BattleSystem> logger, IOptions<WorldConfiguration> worldConfiguration, IGameResources gameResources, IDropSystem dropSystem, IExperienceSystem experienceSystem, IProjectileSystem projectileSystem, IBattlePacketFactory battlePacketFactory, IMoverPacketFactory moverPacketFactory)
        {
            _logger = logger;
            _worldConfiguration = worldConfiguration.Value;
            _gameResources = gameResources;
            _dropSystem = dropSystem;
            _experienceSystem = experienceSystem;
            _projectileSystem = projectileSystem;
            _battlePacketFactory = battlePacketFactory;
            _moverPacketFactory = moverPacketFactory;
        }

        /// <inheritdoc />
        public AttackResult DamageTarget(ILivingEntity attacker, ILivingEntity defender, IAttackArbiter attackArbiter, ObjectMessageType attackType)
        {
            if (!CanAttack(attacker, defender))
            {
                ClearBattleTargets(defender);
                ClearBattleTargets(attacker);
                return null;
            }

            if (defender is IPlayerEntity undyingPlayer)
            {
                if (undyingPlayer.PlayerData.Mode == ModeType.MATCHLESS_MODE)
                {
                    _logger.LogDebug($"{attacker.Object.Name} wasn't able to inflict any damages to {defender.Object.Name} because he is in undying mode");
                    return null;
                }
            }

            AttackResult attackResult;

            if (attacker is IPlayerEntity player && player.PlayerData.Mode.HasFlag(ModeType.ONEKILL_MODE))
            {
                attackResult = new AttackResult
                {
                    Damages = defender.Attributes[DefineAttributes.HP],
                    Flags = AttackFlags.AF_GENERIC
                };
            }
            else
            {
                attackResult = attackArbiter.CalculateDamages();
            }

            if (attackResult.Flags.HasFlag(AttackFlags.AF_MISS) || attackResult.Damages <= 0)
            {
                _battlePacketFactory.SendAddDamage(defender, attacker, attackResult.Flags, attackResult.Damages);

                return attackResult;
            }

            attacker.Battle.Target = defender;
            defender.Battle.Target = attacker;

            if (attackResult.Flags.HasFlag(AttackFlags.AF_FLYING))
            {
                BattleHelper.KnockbackEntity(defender);
            }

            _battlePacketFactory.SendAddDamage(defender, attacker, attackResult.Flags, attackResult.Damages);

            defender.Attributes[DefineAttributes.HP] -= attackResult.Damages;
            _moverPacketFactory.SendUpdateAttributes(defender, DefineAttributes.HP, defender.Attributes[DefineAttributes.HP]);

            CheckIfDefenderIsDead(attacker, defender, attackType);

            return attackResult;
        }

        /// <inheritdoc />
        public void MeleeAttack(ILivingEntity attacker, ILivingEntity defender, ObjectMessageType attackType, float attackSpeed)
        {
            IAttackArbiter attackArbiter = new MeleeAttackArbiter(attacker, defender);
            AttackResult meleeAttackResult = DamageTarget(attacker, defender, attackArbiter, attackType);

            if (meleeAttackResult != null)
            {
                _battlePacketFactory.SendMeleeAttack(attacker, attackType, defender.Id, unknwonParam: 0, meleeAttackResult.Flags);
            }
        }

        /// <inheritdoc />
        public void MagicAttack(ILivingEntity attacker, ILivingEntity defender, ObjectMessageType attackType, int magicAttackPower)
        {
            var projectile = new MagicProjectileInfo(attacker, defender, magicAttackPower, onArrived: () =>
            {
                IAttackArbiter magicAttackArbiter = new MagicAttackArbiter(attacker, defender, magicAttackPower);
                
                DamageTarget(attacker, defender, magicAttackArbiter, ObjectMessageType.OBJMSG_ATK_MAGIC1);
            });
            int projectileId = _projectileSystem.CreateProjectile(projectile);

            _battlePacketFactory.SendMagicAttack(attacker, ObjectMessageType.OBJMSG_ATK_MAGIC1, defender.Id, magicAttackPower, projectileId);
        }

        /// <summary>
        /// Check if the attacker entity can attack a defender entity.
        /// </summary>
        /// <param name="attacker">Attacker living entity.</param>
        /// <param name="defender">Defender living entity.</param>
        /// <returns></returns>
        private bool CanAttack(ILivingEntity attacker, ILivingEntity defender)
        {
            if (attacker == defender)
            {
                _logger.LogError($"{attacker} cannot attack itself.");
                return false;
            }

            if (attacker.IsDead)
            {
                _logger.LogError($"{attacker} cannot attack because its dead.");
                return false;
            }

            if (defender.IsDead)
            {
                _logger.LogError($"{attacker} cannot attack {defender} because target is already dead.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the defender has been killed by the attacker.
        /// </summary>
        /// <param name="attacker">Attacker living entity.</param>
        /// <param name="defender">Defender living entity.</param>
        /// <param name="attackType">Killing attack type.</param>
        private void CheckIfDefenderIsDead(ILivingEntity attacker, ILivingEntity defender, ObjectMessageType attackType)
        {
            if (defender.IsDead)
            {
                _logger.LogDebug($"{attacker.Object.Name} killed {defender.Object.Name}.");
                defender.Attributes[DefineAttributes.HP] = 0;
                _moverPacketFactory.SendUpdateAttributes(defender, DefineAttributes.HP, defender.Attributes[DefineAttributes.HP]);
                ClearBattleTargets(defender);
                ClearBattleTargets(attacker);

                if (defender is IMonsterEntity deadMonster && attacker is IPlayerEntity player)
                {
                    _battlePacketFactory.SendDie(player, defender, attacker, attackType);

                    deadMonster.Timers.DespawnTime = Time.TimeInSeconds() + 5; // Configure this timer on world configuration

                    // Drop items
                    int itemCount = 0;
                    foreach (DropItemData dropItem in deadMonster.Data.DropItems)
                    {
                        if (itemCount >= deadMonster.Data.MaxDropItem)
                            break;

                        long dropChance = RandomHelper.LongRandom(0, DropSystem.MaxDropChance);

                        if (dropItem.Probability * _worldConfiguration.Rates.Drop >= dropChance)
                        {
                            var item = new Item(dropItem.ItemId, 1, -1, -1, -1, (byte)RandomHelper.Random(0, dropItem.ItemMaxRefine));

                            _dropSystem.DropItem(defender, item, attacker);
                            itemCount++;
                        }
                    }

                    // Drop item kinds
                    foreach (DropItemKindData dropItemKind in deadMonster.Data.DropItemsKind)
                    {
                        var itemsDataByItemKind = _gameResources.Items.Values.Where(x => x.ItemKind3 == dropItemKind.ItemKind && x.Rare >= dropItemKind.UniqueMin && x.Rare <= dropItemKind.UniqueMax);

                        if (!itemsDataByItemKind.Any())
                            continue;

                        var itemData = itemsDataByItemKind.ElementAt(RandomHelper.Random(0, itemsDataByItemKind.Count() - 1));

                        int itemRefine = RandomHelper.Random(0, 10);

                        for (int i = itemRefine; i >= 0; i--)
                        {
                            long itemDropProbability = (long)(_gameResources.ExpTables.GetDropLuck(itemData.Level > 120 ? 119 : itemData.Level, itemRefine) * (deadMonster.Data.CorrectionValue / 100f));
                            long dropChance = RandomHelper.LongRandom(0, DropSystem.MaxDropChance);

                            if (dropChance < itemDropProbability * _worldConfiguration.Rates.Drop)
                            {
                                var item = new Item(itemData.Id, 1, -1, -1, -1, (byte)itemRefine);

                                _dropSystem.DropItem(defender, item, attacker);
                                break;
                            }
                        }
                    }

                    // Drop gold
                    int goldDropped = RandomHelper.Random(deadMonster.Data.DropGoldMin, deadMonster.Data.DropGoldMax);
                    _dropSystem.DropGold(deadMonster, goldDropped, attacker);

                    // Give experience
                    _experienceSystem.GiveExeperience(player, deadMonster.Data.Experience * _worldConfiguration.Rates.Experience);
                }
                else if (defender is IPlayerEntity deadPlayer)
                {
                    _battlePacketFactory.SendDie(deadPlayer, defender, attacker, attackType);
                }

                attacker.Behavior.OnTargetKilled(defender);
            }
        }

        /// <summary>
        /// Clears the battle targets.
        /// </summary>
        /// <param name="entity">Current entity.</param>
        private void ClearBattleTargets(ILivingEntity entity)
        {
            entity.Follow.Reset();
            entity.Battle.Reset();
        }
    }
}
