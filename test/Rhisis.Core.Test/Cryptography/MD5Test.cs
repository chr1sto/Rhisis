using Rhisis.Core.Cryptography;
using Xunit;

namespace Rhisis.Core.Test.Cryptography
{
    public class MD5Test
    {
        private const string DecryptedMessage = "Hello world!";
        private const string Salt = "HelloSalt";
        private const string MD5MessageHash = "86fb269d190d2c85f6e0468ceca42a20";
        private const string MD5SaltMessageHash = "122e13d5233080c3a7d466120194a3ff";

        [Fact]
        public void HashMD5Test()
        {
            var hashed = MD5.GetMD5Hash(DecryptedMessage);

            Assert.NotNull(hashed);
            Assert.Equal(MD5MessageHash, hashed);
        }

        [Fact]
        public void HashMD5WithSaltTest()
        {
            var hashed = MD5.GetMD5Hash(Salt, DecryptedMessage);

            Assert.NotNull(hashed);
            Assert.Equal(MD5SaltMessageHash, hashed);
        }
    }
}
