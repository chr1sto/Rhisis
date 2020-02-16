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
using System;
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
        private readonly IBattlePacketFactory _battlePacketFactory;
        private readonly IMoverPacketFactory _moverPacketFactory;
        private readonly ISpecialEffectPacketFactory _specialEffectPacketFactory;
        private readonly ISkillPacketFactory _skillPacketFactory;
        private readonly WorldConfiguration _worldConfiguration;

        /// <summary>
        /// Creates a new <see cref="BattleSystem"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="worldConfiguration">World server configuration.</param>
        /// <param name="gameResources">Game resources.</param>
        /// <param name="dropSystem">Drop system.</param>
        /// <param name="experienceSystem">Experience system.</param>
        /// <param name="battlePacketFactory">Battle packet factory.</param>
        /// <param name="moverPacketFactory">Mover packet factory.</param>
        /// <param name="specialEffectPacketFactory">Special effect factory.</param>
        /// <param name="skillPacketFactory">Skill packet factory.</param>
        public BattleSystem(ILogger<BattleSystem> logger, IOptions<WorldConfiguration> worldConfiguration, IGameResources gameResources, IDropSystem dropSystem, IExperienceSystem experienceSystem, IBattlePacketFactory battlePacketFactory, IMoverPacketFactory moverPacketFactory, ISpecialEffectPacketFactory specialEffectPacketFactory, ISkillPacketFactory skillPacketFactory)
        {
            _logger = logger;
            _worldConfiguration = worldConfiguration.Value;
            _gameResources = gameResources;
            _dropSystem = dropSystem;
            _experienceSystem = experienceSystem;
            _battlePacketFactory = battlePacketFactory;
            _moverPacketFactory = moverPacketFactory;
            _specialEffectPacketFactory = specialEffectPacketFactory;
            _skillPacketFactory = skillPacketFactory;
        }

        /// <inheritdoc />
        public void MeleeAttack(ILivingEntity attacker, ILivingEntity defender, ObjectMessageType attackType, float attackSpeed)
        {
            if (!CanAttack(attacker, defender))
            {
                ClearBattleTargets(defender);
                ClearBattleTargets(attacker);
                return;
            }

            attacker.Battle.Target = defender;
            defender.Battle.Target = attacker;

            AttackResult meleeAttackResult = new MeleeAttackArbiter(attacker, defender).CalculateDamages();

            _logger.LogDebug($"{attacker.Object.Name} inflicted {meleeAttackResult.Damages} to {defender.Object.Name}");

            if (meleeAttackResult.Flags.HasFlag(AttackFlags.AF_FLYING))
                BattleHelper.KnockbackEntity(defender);

            _battlePacketFactory.SendAddDamage(defender, attacker, meleeAttackResult.Flags, meleeAttackResult.Damages);
            _battlePacketFactory.SendMeleeAttack(attacker, attackType, defender.Id, unknwonParam: 0, meleeAttackResult.Flags);

            defender.Attributes[DefineAttributes.HP] -= meleeAttackResult.Damages;
            _moverPacketFactory.SendUpdateAttributes(defender, DefineAttributes.HP, defender.Attributes[DefineAttributes.HP]);

            CheckIfDefenderIsDead(attacker, defender, attackType);
        }

        /// <inheritdoc />
        public void CastMeleeSkill(ILivingEntity attacker, ILivingEntity defender, SkillInfo skill, int skillCastingTime, SkillUseType skillUseType)
        {
            if (!CanAttack(attacker, defender))
            {
                ClearBattleTargets(defender);
                ClearBattleTargets(attacker);
                return;
            }

            // TODO: check range attacks (AOE)

            attacker.Delayer.DelayAction(TimeSpan.FromMilliseconds(skill.LevelData.ComboSkillTime), () =>
            {
                AttackResult meleeSkillAttackResult = new MeleeSkillAttackArbiter(attacker, defender, skill).CalculateDamages();

                _battlePacketFactory.SendAddDamage(defender, attacker, meleeSkillAttackResult.Flags, meleeSkillAttackResult.Damages);
                _skillPacketFactory.SendUseSkill(attacker, defender, skill, skillCastingTime, skillUseType);
            });
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
                ClearBattleTargets(defender);
                ClearBattleTargets(attacker);
                _moverPacketFactory.SendUpdateAttributes(defender, DefineAttributes.HP, defender.Attributes[DefineAttributes.HP]);

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
            entity.Follow.Target = null;
            entity.Battle.Target = null;
            entity.Battle.Targets.Clear();
        }
    }
}
