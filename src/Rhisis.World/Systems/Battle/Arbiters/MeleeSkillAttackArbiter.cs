using Rhisis.Core.Data;
using Rhisis.World.Game.Common;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Battle.Arbiters
{
    /// <summary>
    /// Provides a mechanism to calculate a melee skill attack result based on the attacker and defender statistics.
    /// </summary>
    public class MeleeSkillAttackArbiter : SkillAttackArbiter, IAttackArbiter
    {
        private static readonly IEnumerable<int> SkillsIgnoringDefense = new int[]
        {
            DefineSkill.SI_BIL_PST_ASALRAALAIKUM,
            DefineSkill.SI_JST_YOYO_HITOFPENYA
        };

        /// <summary>
        /// Creates a new <see cref="MeleeAttackArbiter"/> instance.
        /// </summary>
        /// <param name="attacker">Attacker entity</param>
        /// <param name="defender">Defender entity</param>
        /// <param name="meleeSkill">Melee skill.</param>
        public MeleeSkillAttackArbiter(ILivingEntity attacker, ILivingEntity defender, SkillInfo meleeSkill)
            : base(attacker, defender, meleeSkill)
        {
        }

        /// <inheritdoc />
        public override AttackResult CalculateDamages()
        {
            var damages = (int)(GetAttackerSkillPower() * GetAttackMultiplier()) + Attacker.Attributes[DefineAttributes.ATKPOWER];

            var attackResult = new AttackResult
            {
                Flags = AttackFlags.AF_MELEESKILL,
                Damages = damages
            };

            // Post calculation
            if (damages > 0 && !SkillsIgnoringDefense.Any(x => x == Skill.SkillId))
            {
                damages -= Math.Max(GetDefenderDefense(attackResult), 0);
            }

            attackResult.Damages = Math.Max(0, damages);

            if (attackResult.Damages > 0)
            {
                // TODO: GetDamageMultiplier
            }

            return attackResult;
        }
    }
}
