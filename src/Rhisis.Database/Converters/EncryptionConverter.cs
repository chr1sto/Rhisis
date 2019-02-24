using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rhisis.Core.Cryptography;

namespace Rhisis.Database.Converters
{
    internal sealed class EncryptionConverter : ValueConverter<string, string>
    {
        /// <summary>
        /// Creates a new <see cref="EncryptionConverter"/> instance.
        /// </summary>
        /// <param name="encryptionKey">Encryption key</param>
        /// <param name="mappingHints">Mapping hints</param>
        public EncryptionConverter(string encryptionKey, ConverterMappingHints mappingHints = null) 
            : base(x => Encrypt(x, encryptionKey), x => Decrypt(x, encryptionKey), mappingHints)
        {
        }

        /// <summary>
        /// Encrypt a string.
        /// </summary>
        /// <param name="value">String to encrypt</param>
        /// <param name="key">Encryption key</param>
        /// <returns></returns>
        private static string Encrypt(string value, string key) => Aes.EncryptString(value, key);

        /// <summary>
        /// Decrypt a string.
        /// </summary>
        /// <param name="value">String to decrypt</param>
        /// <param name="key">Encryption key</param>
        /// <returns></returns>
        private static string Decrypt(string value, string key) => Aes.DecryptString(value, key);
    }
}
