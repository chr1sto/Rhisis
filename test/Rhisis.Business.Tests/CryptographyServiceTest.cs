using Rhisis.Core.Business.Services;
using Rhisis.Core.Services;
using System.Linq;
using System.Text;
using Xunit;

namespace Rhisis.Business.Tests
{
    public class CryptographyServiceTest
    {
        private const string DecryptedMessage = "Hello world!";
        private const string MD5MessageHash = "86fb269d190d2c85f6e0468ceca42a20";
        private readonly byte[] EncryptedMessage = new byte[] { 25, 7, 55, 145, 111, 159, 200, 59, 9, 128, 203, 69, 187, 104, 233, 86 };
        private readonly byte[] Key = Encoding.ASCII.GetBytes("dldhsvmflvm").Concat(Enumerable.Repeat((byte)0, 5).ToArray()).ToArray();
        private readonly ICryptographyService _cryptographyService;

        public CryptographyServiceTest()
        {
            this._cryptographyService = new CryptographyService();
        }

        [Fact]
        public void EncryptData()
        {
            var encrypted = this._cryptographyService.Encrypt(Encoding.Default.GetBytes(DecryptedMessage), Key);

            Assert.NotNull(encrypted);
            Assert.Equal(EncryptedMessage.Length, encrypted.Length);

            for (int i = 0; i < EncryptedMessage.Length; i++)
            {
                Assert.Equal(EncryptedMessage[i], encrypted[i]);
            }
        }
        
        [Fact]
        public void DecryptData()
        {
            var decrypted = this._cryptographyService.Decrypt(EncryptedMessage, Key);

            Assert.NotNull(decrypted);
            Assert.Equal(DecryptedMessage.Length, decrypted.Length);
            Assert.Equal(DecryptedMessage, decrypted);
        }

        [Fact]
        public void EncryptDecryptData()
        {
            var encrypted = this._cryptographyService.Encrypt(Encoding.Default.GetBytes(DecryptedMessage), Key);
            var decrypted = this._cryptographyService.Decrypt(encrypted, Key);

            Assert.NotNull(encrypted);
            Assert.NotNull(decrypted);
            Assert.Equal(DecryptedMessage, decrypted);
        }

        [Fact]
        public void HashMD5()
        {
            var hashed = this._cryptographyService.GetMD5Hash(DecryptedMessage);

            Assert.NotNull(hashed);
            Assert.Equal(MD5MessageHash, hashed);
        }
    }
}
