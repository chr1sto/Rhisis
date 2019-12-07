using Rhisis.Core.Common.Formulas;
using Xunit;

namespace Rhisis.Core.Test.Formulas
{
    public class HealthFormulasTest
    {
        [Theory]
        [InlineData(1, 15, 0.9f, 230)]
        public void MaxHpFormulaTest(int level, int stamina, float factor, int expected)
        {
            var hp = HealthFormulas.GetMaxOriginHp(level, stamina, factor);

            Assert.Equal(expected, hp);
        }

        [Theory]
        [InlineData(1, 15, 0.3f, true, 63)]
        public void MaxMpFormulaTest(int level, int inteligence, float factor, bool isPlayer, int expected)
        {
            var mp = HealthFormulas.GetMaxOriginMp(level, inteligence, factor, isPlayer);

            Assert.Equal(expected, mp);
        }

        [Theory]
        [InlineData(1, 15, 15, 15, 0.3f, true, 32)]
        public void MaxFpFormulaTest(int level, int stamina, int dexterity, int strength, float factor, bool isPlayer, int expected)
        {
            var fp = HealthFormulas.GetMaxOriginFp(level, stamina, dexterity, strength, factor, isPlayer);

            Assert.Equal(expected, fp);
        }
    }
}
