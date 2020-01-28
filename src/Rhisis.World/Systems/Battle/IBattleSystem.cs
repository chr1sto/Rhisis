using Rhisis.Core.Data;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Systems.Battle
{
    /// <summary>
    /// Provides a mechanism to fight other entities like monsters (PVE) or players (PVP).
    /// </summary>
    public interface IBattleSystem
    {
        /// <summary>
        /// Process a melee attack on an ennemy.
        /// </summary>
        /// <param name="attacker">Attacker.</param>
        /// <param name="defender">Defender.</param>
        /// <param name="attackType">Attack type.</param>
        /// <param name="attackSpeed">Attack speed.</param>
        void MeleeAttack(ILivingEntity attacker, ILivingEntity defender, ObjectMessageType attackType, float attackSpeed);

        /// <summary>
        /// Casts a melee skill on an ennemy.
        /// </summary>
        /// <param name="attacker">Attacker.</param>
        /// <param name="defender">Defender.</param>
        /// <param name="skill">Skill to cast.</param>
        void CastMeleeSkill(ILivingEntity attacker, ILivingEntity defender, SkillInfo skill);
    }
}
