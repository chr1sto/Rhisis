using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.Core.Common.Formulas
{
    public static class HealthFormulas
    {
        public static int GetMaxOriginHp(int level, int stamina, float maxHpFactor)
        {
            float a = (maxHpFactor * level) / 2.0f;
            float b = a * ((level + 1) / 4.0f) * (1.0f + (stamina / 50.0f)) + (stamina * 10f);

            return (int)(b + 80f);
        }

        public static int GetMaxOriginMp(int level, int inteligence, float maxMpFactor, bool isPlayer)
        {
            if (isPlayer)
            {
                return (int)(((((level * 2.0f) + (inteligence * 8.0f)) * maxMpFactor) + 22.0f) + (inteligence * maxMpFactor));
            }

            return (level * 2) + (inteligence * 8) + 22;
        }

        public static int GetMaxOriginFp(int level, int stamina, int dexterity, int strength, float maxFpFactor, bool isPlayer)
        {
            if (isPlayer)
            {
                return (int)((((level * 2.0f) + (stamina * 6.0f)) * maxFpFactor) + (stamina * maxFpFactor));
            }

            return ((level * 2) + (strength * 7) + (stamina * 2) + (dexterity * 4));
        }
    }
}
