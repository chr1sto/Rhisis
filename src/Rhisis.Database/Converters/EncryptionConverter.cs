using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rhisis.Core.Cryptography;
using System;

namespace Rhisis.Database.Converters
{
    internal sealed class EncryptionConverter : ValueConverter<string, string>
    {
        public EncryptionConverter(string encryptionKey, ConverterMappingHints mappingHints = null) 
            : base(x => Encrypt(x, encryptionKey), x => Decrypt(x, encryptionKey), mappingHints)
        {
        }

        private static string Encrypt(string value, string key) 
            => Aes.EncryptString(value, Convert.FromBase64String(key));

        private static string Decrypt(string value, string key) 
            => Aes.DecryptByteArray(Convert.FromBase64String(value), Convert.FromBase64String(key));
    }
}
