using Rhisis.Core.Cryptography;
using System;
using System.Linq;
using System.Text;
using Xunit;

namespace Rhisis.Core.Test.Cryptography
{
    public class AesTest
    {
        private const string DecryptedMessage = "Hello world!";
        private const string KeyText = "dldhsvmflvm";
        private readonly byte[] EncryptedMessage = new byte[] { 25, 7, 55, 145, 111, 159, 200, 59, 9, 128, 203, 69, 187, 104, 233, 86 };
        private readonly byte[] Key = Encoding.ASCII.GetBytes(KeyText).Concat(Enumerable.Repeat((byte)0, 5).ToArray()).ToArray();

        [Fact]
        public void EncryptData()
        {
            var encrypted = Aes.EncryptByteArray(Encoding.Default.GetBytes(DecryptedMessage), Key);

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
            var decrypted = Aes.DecryptByteArray(EncryptedMessage, Key);

            Assert.NotNull(decrypted);
            Assert.Equal(DecryptedMessage.Length, decrypted.Length);
            Assert.Equal(DecryptedMessage, decrypted);
        }

        [Fact]
        public void EncryptDecryptData()
        {
            var encrypted = Aes.EncryptByteArray(Encoding.Default.GetBytes(DecryptedMessage), Key);
            var decrypted = Aes.DecryptByteArray(encrypted, Key);

            Assert.NotNull(encrypted);
            Assert.NotNull(decrypted);
            Assert.Equal(DecryptedMessage, decrypted);
        }

        [Fact]
        public void BuildKeyFromString()
        {
            var encryptedKey = Aes.BuildEncryptionKeyFromString(KeyText, 16);

            Assert.Equal(Key.Length, encryptedKey.Length);
            Assert.Equal(Key, encryptedKey);
        }

        [Fact]
        public void AdjustEncryptionKey()
        {
            var encryptedKey = Encoding.ASCII.GetBytes(KeyText);
            encryptedKey = Aes.AdjustEncryptionKey(encryptedKey, 16);

            Assert.Equal(Key.Length, encryptedKey.Length);
            Assert.Equal(Key, encryptedKey);
        }

        [Theory]
        [InlineData(128, false)]
        [InlineData(192, false)]
        [InlineData(256, false)]
        [InlineData(512, true)]
        [InlineData(64, true)]
        [InlineData(243, true)]
        public void GenerateEncryptionKey(int keySize, bool shouldThrowException)
        {
            if (shouldThrowException)
                Assert.Throws<InvalidOperationException>(() => Aes.GenerateKey(keySize));
            else
            {
                var generatedKey = Aes.GenerateKey(keySize);

                Assert.NotNull(generatedKey);
            }
        }

        [Theory]
        [InlineData(128)]
        [InlineData(192)]
        [InlineData(256)]
        public void EncryptDecryptStringWithGeneratedKey(int keySize)
        {
            string generatedKey = Aes.GenerateKey(keySize);
            string encryptedData = Aes.EncryptString(DecryptedMessage, generatedKey);

            Assert.NotNull(encryptedData);

            string decryptedData = Aes.DecryptString(encryptedData, generatedKey);

            Assert.NotNull(decryptedData);
            Assert.Equal(DecryptedMessage, decryptedData);
        }

        [Theory]
        [InlineData(128)]
        [InlineData(192)]
        [InlineData(256)]
        public void EncryptDecryptByteArrayWithGeneratedKey(int keySize)
        {
            byte[] decryptedMessageBytes = Encoding.UTF8.GetBytes(DecryptedMessage);
            byte[] generatedKey = Convert.FromBase64String(Aes.GenerateKey(keySize));
            byte[] encryptedData = Aes.EncryptByteArray(decryptedMessageBytes, generatedKey);

            Assert.NotNull(encryptedData);

            string decryptedData = Aes.DecryptByteArray(encryptedData, generatedKey);

            Assert.NotNull(decryptedData);
            Assert.Equal(DecryptedMessage, decryptedData);
        }
    }
}
