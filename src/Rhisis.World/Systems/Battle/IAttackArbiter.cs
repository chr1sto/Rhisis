namespace Rhisis.World.Systems.Battle
{
    /// <summary>
    /// Provides an interface to calculate an attack result based on the attacker and defender statistics.
    /// </summary>
    public interface IAttackArbiter
    {
        /// <summary>
        /// Calculates the damages inflicted by an attacker to a defender.
        /// </summary>
        /// <returns><see cref="AttackResult"/>.</returns>
        AttackResult CalculateDamages();
    }
}
