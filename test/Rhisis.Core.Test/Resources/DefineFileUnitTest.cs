using Rhisis.Core.Resources;
using System.Collections.Generic;
using Xunit;

namespace Rhisis.Core.Test.Resources
{
    public class DefineFileUnitTest
    {
        private static readonly string DefineFilePath = "Common/defineText.h";
        private static readonly int DefinesCount = 4;
        private static readonly string TextTwoKey = "TEXT_TWO";
        private static readonly int TextTwoValue = 1;
        private static readonly string TextCommentKey = "TEXT_COMMENT";

        /// <summary>
        /// Opens and reads a <see cref="DefineFile"/>.
        /// </summary>
        [Fact]
        public void OpenDefineFile()
        {
            var defines = new DefineFile(DefineFilePath);

            Assert.NotNull(defines);
            Assert.NotEqual(0, defines.Count);

            defines.Dispose();
        }

        /// <summary>
        /// Count the number of defines in the <see cref="DefineFile"/>.
        /// </summary>
        [Fact]
        public void CountDefines()
        {
            var defines = new DefineFile(DefineFilePath);

            Assert.NotNull(defines);
            Assert.Equal(DefinesCount, defines.Count);

            defines.Dispose();
        }

        /// <summary>
        /// Gets a numeric define value.
        /// </summary>
        [Fact]
        public void GetDefineValueAsNumber()
        {
            var defines = new DefineFile(DefineFilePath);

            Assert.NotNull(defines);

            int defineValueFromIndexer = (int)defines[TextTwoKey];
            int defineValueFromMethod = defines.GetValue<int>(TextTwoKey);

            Assert.Equal(TextTwoValue, defineValueFromIndexer);
            Assert.Equal(TextTwoValue, defineValueFromMethod);

            defines.Dispose();
        }

        /// <summary>
        /// Try to get a define value that is commented.
        /// </summary>
        [Fact]
        public void TryGetCommentedDefineValue()
        {
            var defines = new DefineFile(DefineFilePath);

            Assert.NotNull(defines);

            var exception = Assert.Throws<KeyNotFoundException>(() =>
            {
                object defineValueFromIndexer = defines[TextCommentKey];
            });

            Assert.NotNull(exception);
            Assert.Equal(typeof(KeyNotFoundException), exception.GetType());

            defines.Dispose();
        }
    }
}
