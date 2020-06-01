using Rhisis.Core.Extensions;
using System;
using Xunit;

namespace Rhisis.Core.Test.Extensions
{
    public class StringExtensionsTest
    {
        public enum TestEnum
        {
            Unknown = 0,
            One = 1,
            Two = 2
        }

        [Theory]
        [InlineData("foo@bar.com", true)]
        [InlineData("shade@rhisis-project.com", true)]
        [InlineData("rhisis-at-project.com", false)]
        [InlineData("rhisis-at-project.dotcom-", false)]
        public void IsValidEmailTest(string email, bool valid)
        {
            Assert.Equal(valid, email.IsValidEmail());
        }

        [Theory]
        [InlineData("One", TestEnum.One)]
        [InlineData("tWo", TestEnum.Two)]
        [InlineData("Three", TestEnum.Unknown)]
        public void ToEnumTest(string enumAsText, TestEnum expected)
        {
            Assert.Equal(expected, enumAsText.ToEnum<TestEnum>());
        }
    }
}
