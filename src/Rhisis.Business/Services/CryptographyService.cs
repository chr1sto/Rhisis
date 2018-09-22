using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Services;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Rhisis.Core.Business.Services
{
    [Injectable]
    public class CryptographyService : ICryptographyService
    {
        private readonly uint[] _crc32Table = new uint[256];

        /// <summary>
        /// Creates and initializes a new <see cref="CryptographyService"/> instance.
        /// </summary>
        public CryptographyService()
        {
            this.InitializeCrc32Table();
        }

        /// <inheritdoc />
        public string Decrypt(byte[] input, byte[] key)
        {
            string decrypted = string.Empty;

            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.Zeros;
                aes.Key = key;
                aes.IV = Enumerable.Repeat<byte>(0, 16).ToArray();

                using (var memoryStream = new MemoryStream(input))
                using (var crypto = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (var sr = new StreamReader(crypto))
                    decrypted = sr.ReadToEnd().Trim('\0');
            }

            return decrypted;
        }

        /// <inheritdoc />
        public byte[] Encrypt(byte[] input, byte[] key)
        {
            byte[] encrypted = null;

            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.Zeros;
                aes.Key = key;
                aes.IV = Enumerable.Repeat<byte>(0, 16).ToArray();

                using (var memoryStream = new MemoryStream())
                {
                    using (var crypto = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        crypto.Write(input, 0, input.Length);
                    }
                    encrypted = memoryStream.ToArray();
                }
            }

            return encrypted;
        }

        /// <inheritdoc />
        public byte[] AdjustEncryptionKey(byte[] encryptionKey, int keySize)
        {
            if (encryptionKey.Length < keySize)
            {
                int missingBytes = keySize - encryptionKey.Length;

                return encryptionKey.Concat(Enumerable.Repeat((byte)0, missingBytes)).ToArray();
            }
            else
            {
                return encryptionKey.Take(keySize).ToArray();
            }
        }

        /// <inheritdoc />
        public byte[] BuildEncryptionKeyFromString(string encryptionKey, int keySize) 
            => this.BuildEncryptionKeyFromString(encryptionKey, keySize, Encoding.Default);

        /// <inheritdoc />
        public byte[] BuildEncryptionKeyFromString(string encryptionKey, int keySize, Encoding encoding) 
            => this.AdjustEncryptionKey(encoding.GetBytes(encryptionKey), keySize);

        /// <inheritdoc />
        public string GetMD5Hash(string input)
        {
            byte[] data = null;
            var sBuilder = new StringBuilder();

            using (MD5 md5Hash = MD5.Create())
                data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            for (var i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            return sBuilder.ToString();
        }

        /// <inheritdoc />
        public uint Crc32ComputeChecksum(byte[] input)
        {
            uint crc = 0xffffffff;

            for (int i = 0; i < input.Length; ++i)
            {
                var index = (byte)(((crc) & 0xff) ^ input[i]);
                crc = ((crc >> 8) ^ this._crc32Table[index]);
            }

            return ~crc;
        }

        /// <inheritdoc />
        public byte[] Crc32ComputeChecksumBytes(byte[] input) => BitConverter.GetBytes(this.Crc32ComputeChecksum(input));

        /// <summary>
        /// Initialize the Crc32 table.
        /// </summary>
        private void InitializeCrc32Table()
        {
            uint poly = 0xedb88320;
            uint temp = 0;

            for (uint i = 0; i < this._crc32Table.Length; ++i)
            {
                temp = i;

                for (int j = 8; j > 0; --j)
                {
                    if ((temp & 1) == 1)
                        temp = ((temp >> 1) ^ poly);
                    else
                        temp >>= 1;
                }

                this._crc32Table[i] = temp;
            }
        }
    }
}
