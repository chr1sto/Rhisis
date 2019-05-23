namespace Rhisis.Core.Common.Formulas
{
    /// <summary>
    /// Health formulas.
    /// </summary>
    public static class HealthFormulas
    {
        /// <summary>
        /// Gets the max origin HP for a living entity.
        /// </summary>
        /// <param name="level">Entity level.</param>
        /// <param name="stamina">Entity stamina.</param>
        /// <param name="maxHpFactor">Max hp factor.</param>
        /// <returns>Max origin HP without bonuses.</returns>
        public static int GetMaxOriginHp(int level, int stamina, float maxHpFactor)
        {
            float a = (maxHpFactor * level) / 2.0f;
            float b = a * ((level + 1) / 4.0f) * (1.0f + (stamina / 50.0f)) + (stamina * 10f);

            return (int)(b + 80f);
        }

        /// <summary>
        /// Gets the max origin MP for a living entity.
        /// </summary>
        /// <param name="level">Entity level.</param>
        /// <param name="inteligence">Entity inteligence.</param>
        /// <param name="maxMpFactor">Max mp factor.</param>
        /// <param name="isPlayer">Flag that indicates if the entity is a player.</param>
        /// <returns>Max origin MP without bonuses.</returns>
        public static int GetMaxOriginMp(int level, int inteligence, float maxMpFactor, bool isPlayer)
        {
            if (isPlayer)
            {
                return (int)(((((level * 2.0f) + (inteligence * 8.0f)) * maxMpFactor) + 22.0f) + (inteligence * maxMpFactor));
            }

            return (level * 2) + (inteligence * 8) + 22;
        }

        /// <summary>
        /// Gets the max origin FP for a living entity.
        /// </summary>
        /// <param name="level">Entity level.</param>
        /// <param name="stamina">Entity stamina.</param>
        /// <param name="dexterity">Entity dexterity.</param>
        /// <param name="strength">Entity strength.</param>
        /// <param name="maxFpFactor">Max fp factor.</param>
        /// <param name="isPlayer">Flag that indicates if the entity is a player.</param>
        /// <returns>Max origin FP without bonuses.</returns>
        public static int GetMaxOriginFp(int level, int stamina, int dexterity, int strength, float maxFpFactor, bool isPlayer)
        {
            if (isPlayer)
            {
                return (int)((((level * 2.0f) + (stamina * 6.0f)) * maxFpFactor) + (stamina * maxFpFactor));
            }

            return ((level * 2) + (strength * 7) + (stamina * 2) + (dexterity * 4));
        }

        /// <summary>
        /// Gets the recovery MP for a living entity.
        /// </summary>
        /// <param name="maxHp">Origin max HP.</param>
        /// <param name="level">Entity level.</param>
        /// <param name="stamina">Entity stamina.</param>
        /// <param name="hpRecoveryFactor">Hp recovery factor.</param>
        /// <returns>HP recovery.</returns>
        public static int GetHpRecovery(int maxHp, int level, int stamina, float hpRecoveryFactor = 1f)
        {
            int recoveredHp = (int)((level / 3.0f) + (maxHp / (500.0f * level)) + (stamina * hpRecoveryFactor));

            return ReduceRecoveryPercent(recoveredHp);
        }

        /// <summary>
        /// Gets the recovery MP for a living entity.
        /// </summary>
        /// <param name="maxMp">Origin max MP.</param>
        /// <param name="level">Entity level.</param>
        /// <param name="inteligence">Entity inteligence.</param>
        /// <param name="mpRecoveryFactor">Mp recovery factor.</param>
        /// <returns>MP recovery.</returns>
        public static int GetMpRecovery(int maxMp, int level, int inteligence, float mpRecoveryFactor = 1f)
        {
            int recoveredMp = (int)(((level * 1.5f) + (maxMp / (500.0f * level)) + (inteligence * mpRecoveryFactor)) * 0.2f);

            return ReduceRecoveryPercent(recoveredMp);
        }

        /// <summary>
        /// Gets the recovery FP for a living entity.
        /// </summary>
        /// <param name="maxFp">Origin max FP.</param>
        /// <param name="level">Entity level.</param>
        /// <param name="stamina">Entity stamina.</param>
        /// <param name="fpRecoverFactory">FP recovery factor.</param>
        /// <returns>FP recovery.</returns>
        public static int GetFpRecovery(int maxFp, int level, int stamina, float fpRecoverFactory = 1f)
        {
            int recoveredFp = (int)(((level * 2.0f) + (maxFp / (500.0f * level)) + (stamina * fpRecoverFactory)) * 0.2f);

            return ReduceRecoveryPercent(recoveredFp);
        }

        /// <summary>
        /// Remove 10% to the recovery amount.
        /// </summary>
        /// <param name="recovery">Recovery amount.</param>
        /// <returns>Recovery - 10%</returns>
        private static int ReduceRecoveryPercent(int recovery) => (int)(recovery - (recovery * 0.1f));
    }
}
