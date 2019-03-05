using Rhisis.Core.Extensions;
using Xunit;

namespace Rhisis.Core.Test.Extensions
{
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData("foo@bar.com", true)]
        [InlineData("shade@rhisis-project.com", true)]
        [InlineData("rhisis-at-project.com", false)]
        [InlineData("rhisis-at-project.dotcom-", false)]
        public void IsValidEmailTest(string email, bool valid)
        {
            Assert.Equal(valid, email.IsValidEmail());
        }
    }
}
