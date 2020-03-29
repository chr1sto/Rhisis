using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.Core.Helpers;
using Rhisis.World.Game.Common;
using Rhisis.World.Game.Entities;
using System;

namespace Rhisis.World.Systems.Battle.Arbiters
{
    public abstract class AttackArbiterBase : IAttackArbiter
    {
        /// <summary>
        /// Gets the attacker entity.
        /// </summary>
        protected ILivingEntity Attacker { get; }

        /// <summary>
        /// Gets the defender entity.
        /// </summary>
        protected ILivingEntity Defender { get; }

        /// <summary>
        /// Creates a new <see cref="AttackArbiterBase"/> instance.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        protected AttackArbiterBase(ILivingEntity attacker, ILivingEntity defender)
        {
            Attacker = attacker;
            Defender = defender;
        }

        /// <summary>
        /// Calculates damages.
        /// </summary>
        /// <returns></returns>
        public abstract AttackResult CalculateDamages();

        /// <summary>
        /// Gets the defender defense.
        /// </summary>
        /// <param name="attackResult"></param>
        /// <returns></returns>
        protected int GetDefenderDefense(AttackResult attackResult)
        {
            if (attackResult.Flags.HasFlag(AttackFlags.AF_MAGICSKILL))
            {
                return Defender.Attributes[DefineAttributes.RESIST_MAGIC];
            }

            var defense = 0;
            var isGenericAttack = attackResult.Flags.HasFlag(AttackFlags.AF_GENERIC);

            if (Attacker.Type == WorldEntityType.Player && Defender.Type == WorldEntityType.Player)
                isGenericAttack = true;

            if (isGenericAttack)
            {
                var factor = 1f;

                if (Defender is IPlayerEntity player)
                    factor = player.PlayerData.JobData.DefenseFactor;

                var stamina = Defender.Attributes[DefineAttributes.STA];
                var level = Defender.Object.Level;

                defense = (int)((level * 2 + stamina / 2) / 2.8f - 4 + (stamina - 14) * factor);
                // TODO: add defense armor
                // TODO: add DST_ADJDEF

                defense = Math.Max(defense, 0);
            }

            if (Defender is IMonsterEntity monster)
            {
                defense = GetMonsterDefense(monster, attackResult.Flags);
            }
            else if (Defender is IPlayerEntity player)
            {
                defense = GetPlayerDefense(player, attackResult.Flags);
            }

            return (int)(defense * GetDefenseMultiplier());
        }

        /// <summary>
        /// Gets the defender defense multiplier.
        /// </summary>
        /// <param name="defenseFactor">Defense factor. (default is 1.0f)</param>
        /// <returns></returns>
        protected virtual float GetDefenseMultiplier(float defenseFactor = 1.0f)
        {
            defenseFactor *= 1.0f + Defender.Attributes[DefineAttributes.ADJDEF_RATE] / 100.0f;

            return defenseFactor;
        }

        /// <summary>
        /// Calculates the given monster defense.
        /// </summary>
        /// <param name="defenderMonster">Monster entity defending an attack.</param>
        /// <param name="attackFlags">Attack flags.</param>
        /// <returns>Monster's defense.</returns>
        private int GetMonsterDefense(IMonsterEntity defenderMonster, AttackFlags attackFlags)
        {
            var monsterDefenseArmor = attackFlags.HasFlag(AttackFlags.AF_MAGIC) ? defenderMonster.Data.MagicResitance : defenderMonster.Data.NaturalArmor;

            return (int)(monsterDefenseArmor / 7f + 1);
        }

        /// <summary>
        /// Calculates the player's defense.
        /// </summary>
        /// <param name="defenderPlayer">Current defender player.</param>
        /// <param name="flags">Attack flags.</param>
        /// <returns>Player's defense.</returns>
        private int GetPlayerDefense(IPlayerEntity defenderPlayer, AttackFlags flags)
        {
            var defense = 0;

            if (Attacker.Type == WorldEntityType.Player)
            {
                if (flags.HasFlag(AttackFlags.AF_MAGIC))
                {
                    defense = (int)(defenderPlayer.Attributes[DefineAttributes.INT] * 9.04f + defenderPlayer.Object.Level * 35.98f);
                }
                else
                {
                    // TODO: GetDefenseByItem()
                }
            }
            else
            {
                // TODO: GetDefenseByItem()
            }

            return Math.Max(0, defense);
        }

        /// <summary>
        /// Gets the defender blocking factor.
        /// </summary>
        /// <returns>Defender blocking factor</returns>
        protected float GetDefenderBlockFactor()
        {
            if (Defender.Type == WorldEntityType.Player)
            {
                var blockRandomProbability = RandomHelper.Random(0, 80);

                if (blockRandomProbability <= 5)
                    return 1f;
                if (blockRandomProbability >= 75f)
                    return 0.1f;

                var defenderLevel = Defender.Object.Level;
                var defenderDexterity = Defender.Attributes[DefineAttributes.DEX];

                var blockProbabilityA = defenderLevel / ((defenderLevel + Attacker.Object.Level) * 15.0f);
                var blockProbabilityB = (defenderDexterity + Attacker.Attributes[DefineAttributes.DEX] + 2) * ((defenderDexterity - Attacker.Attributes[DefineAttributes.DEX]) / 800.0f);

                if (blockProbabilityB > 10.0f)
                    blockProbabilityB = 10.0f;
                var blockProbability = blockProbabilityA + blockProbabilityB;
                if (blockProbability < 0.0f)
                    blockProbability = 0.0f;

                // TODO: range attack probability

                var player = Defender as IPlayerEntity;
                if (player is null)
                    return 0f;

                var blockRate = (int)(defenderDexterity / 8.0f * player.PlayerData.JobData.Blocking + blockProbability);

                if (blockRate < 0)
                    blockRate = 0;

                if (blockRate > blockRandomProbability)
                    return 0f;
            }
            else if (Defender.Type == WorldEntityType.Monster)
            {
                var blockRandomProbability = RandomHelper.Random(0, 100);

                if (blockRandomProbability <= 5)
                    return 1f;
                if (blockRandomProbability >= 95)
                    return 0f;

                var blockRate = (int)((GetEspaceRating(Defender) - Defender.Object.Level) * 0.5f);

                if (blockRate < 0)
                    blockRate = 0;

                if (blockRate > blockRandomProbability)
                    return 0.2f;
            }

            return 1f;
        }

        /// <summary>
        /// Gets the escape rating of an entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GetEspaceRating(ILivingEntity entity)
        {
            if (entity is IPlayerEntity player)
            {
                return (int)(player.Attributes[DefineAttributes.DEX] * 0.5f); // TODO: add dex bonus and DST_PARRY
            }
            else if (entity is IMonsterEntity monster)
            {
                return monster.Data.EscapeRating;
            }

            return 0;
        }

        /// <summary>
        /// Gets attacker attack multiplier.
        /// </summary>
        /// <returns></returns>
        public float GetAttackMultiplier()
        {
            var multiplier = 1.0f + Attacker.Attributes[DefineAttributes.ATKPOWER_RATE] / 100f;

            if (Attacker is IPlayerEntity)
            {
                // TODO: check SM mode SM_ATTACK_UP or SM_ATTACK_UP1 => multiplier *= 1.2f;

                var damages = Attacker.Attributes[Defender.Type == WorldEntityType.Player ? DefineAttributes.PVP_DMG : DefineAttributes.MONSTER_DMG];

                if (damages > 0)
                {
                    multiplier += multiplier * damages / 100;
                }
            }

            return multiplier;
        }
    }
}
