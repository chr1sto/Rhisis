using Rhisis.Core.Data;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems.Battle
{
    /// <summary>
    /// Provides a mechanism to fight other entities like monsters (PVE) or players (PVP).
    /// </summary>
    public interface IBattleSystem
    {
        /// <summary>
        /// Inflicts damages to a defender target based on the given attack arbiter.
        /// </summary>
        /// <param name="attacker">Attacker entity.</param>
        /// <param name="defender">Defender target entity.</param>
        /// <param name="attackArbiter">Attack arbiter to inflict damages to the target.</param>
        /// <param name="attackType">Attack type.</param>
        /// <returns>The attack result.</returns>
        AttackResult DamageTarget(ILivingEntity attacker, ILivingEntity defender, IAttackArbiter attackArbiter, ObjectMessageType attackType);

        /// <summary>
        /// Process a melee attack on an ennemy.
        /// </summary>
        /// <param name="attacker">Attacker.</param>
        /// <param name="defender">Defender.</param>
        /// <param name="attackType">Attack type.</param>
        /// <param name="attackSpeed">Attack speed.</param>
        void MeleeAttack(ILivingEntity attacker, ILivingEntity defender, ObjectMessageType attackType, float attackSpeed);

        /// <summary>
        /// Process a magic attack on an ennemy.
        /// </summary>
        /// <param name="attacker">Attacker.</param>
        /// <param name="defender">Defender.</param>
        /// <param name="attackType">Attack type.</param>
        /// <param name="magicAttackPower">Magic attack power.</param>
        void MagicAttack(ILivingEntity attacker, ILivingEntity defender, ObjectMessageType attackType, int magicAttackPower);

        /// <summary>
        /// Process a range attack on an ennemy.
        /// </summary>
        /// <param name="attacker">Attacker.</param>
        /// <param name="defender">Defender.</param>
        /// <param name="attackType">Attack type.</param>
        /// <param name="power">Range attack power.</param>
        void RangeAttack(ILivingEntity attacker, ILivingEntity defender, ObjectMessageType attackType, int power);
    }
}
