using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AesEncryption = System.Security.Cryptography.Aes;

namespace Rhisis.Core.Cryptography
{
    public static class Aes
    {
        /// <summary>
        /// Encrypt data using the Rijndael algorithm.
        /// </summary>
        /// <param name="input">Input to encrypt</param>
        /// <param name="key">Encrypt key.</param>
        /// <returns>Encrypted data</returns>
        public static byte[] EncryptByteArray(byte[] input, byte[] key)
        {
            byte[] encrypted = null;

            using (var aes = AesEncryption.Create())
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

        /// <summary>
        /// Encrypts a string.
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="key">Encryption key</param>
        /// <returns>Encrypted string converted in base 64</returns>
        public static string EncryptString(string input, byte[] key) 
            => Convert.ToBase64String(EncryptByteArray(Encoding.UTF8.GetBytes(input), key));

        /// <summary>
        /// Decrypt a byte array into a string using the Rijndael algorithm.
        /// </summary>
        /// <param name="input">Input to decrypt</param>
        /// <param name="key">Decrypt key</param>
        /// <returns>Decrypted data</returns>
        public static string DecryptByteArray(byte[] input, byte[] key)
        {
            string decrypted = string.Empty;

            using (var aes = AesEncryption.Create())
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

        /// <summary>
        /// Build the encryption key from a string value using default encoding.
        /// </summary>
        /// <param name="encryptionKey">Encryption key as string</param>
        /// <param name="keySize">Encryption key size</param>
        /// <returns></returns>
        public static byte[] BuildEncryptionKeyFromString(string encryptionKey, int keySize)
            => BuildEncryptionKeyFromString(encryptionKey, keySize, Encoding.Default);

        /// <summary>
        /// Build the encryption key from a string value.
        /// </summary>
        /// <param name="encryptionKey">Encryption key as string</param>
        /// <param name="keySize">Encryption key size</param>
        /// <param name="encoding">Encryption key encoding</param>
        /// <returns></returns>
        public static byte[] BuildEncryptionKeyFromString(string encryptionKey, int keySize, Encoding encoding)
            => AdjustEncryptionKey(encoding.GetBytes(encryptionKey), keySize);

        /// <summary>
        /// Adjust the encryption key to the desired keySize.
        /// </summary>
        /// <param name="encryptionKey">Encryption key</param>
        /// <param name="keySize">Encryption key size</param>
        /// <returns></returns>
        public static byte[] AdjustEncryptionKey(byte[] encryptionKey, int keySize)
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

        /// <summary>
        /// Generates an encryption key.
        /// </summary>
        /// <param name="keySize"></param>
        /// <returns></returns>
        public static string GenerateKey()
        {
            var crypto = new AesCryptoServiceProvider
            {
                KeySize = 128,
                BlockSize = 128
            };

            crypto.GenerateKey();

            return Convert.ToBase64String(crypto.Key);
        }
    }
}
