using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Systems.Battle
{
    /// <summary>
    /// Provides a mechanism to calculate a melee skill attack result based on the attacker and defender statistics.
    /// </summary>
    public class MeleeSkillAttackArbiter : IAttackArbiter
    {
        private readonly ILivingEntity _attacker;
        private readonly ILivingEntity _defender;
        private readonly SkillInfo _meleeSkill;

        /// <summary>
        /// Creates a new <see cref="MeleeAttackArbiter"/> instance.
        /// </summary>
        /// <param name="attacker">Attacker entity</param>
        /// <param name="defender">Defender entity</param>
        public MeleeSkillAttackArbiter(ILivingEntity attacker, ILivingEntity defender, SkillInfo meleeSkill)
        {
            _attacker = attacker;
            _defender = defender;
            _meleeSkill = meleeSkill;
        }

        /// <inheritdoc />
        public AttackResult OnDamage()
        {
            int referStatistic1 = _attacker.Attributes[_meleeSkill.Data.ReferStat1];
            int referStatistic2 = _attacker.Attributes[_meleeSkill.Data.ReferStat2];

            if (_meleeSkill.Data.ReferTarget1 == SkillReferTargetType.Attack && referStatistic1 != 0)
            {
                referStatistic1 = (int)(((_meleeSkill.Data.ReferValue1 / 10f) * referStatistic1) + (_meleeSkill.Level * (referStatistic1 / 50f)));
            }

            if (_meleeSkill.Data.ReferTarget2 == SkillReferTargetType.Attack && referStatistic2 != 0)
            {
                referStatistic2 = (int)(((_meleeSkill.Data.ReferValue2 / 10f) * referStatistic2) + (_meleeSkill.Level * (referStatistic2 / 50f)));
            }

            int referStatistic = referStatistic1 + referStatistic2;
            int attackMin = 0;
            int attackMax = 0;

            if (_attacker.Type == WorldEntityType.Player && _defender.Type == WorldEntityType.Player)
            {
                attackMin = _meleeSkill.LevelData.AbilityMinPVP;
                attackMax = _meleeSkill.LevelData.AbilityMaxPVP;
            }
            else
            {
                attackMin = _meleeSkill.LevelData.AbilityMin;
                attackMax = _meleeSkill.LevelData.AbilityMax;
            }



            return null;
        }
    }
}
