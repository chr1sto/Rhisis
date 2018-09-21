namespace Rhisis.Core.Services
{
    /// <summary>
    /// Cryptography service.
    /// </summary>
    public interface ICryptographyService
    {
        /// <summary>
        /// Decrypt data using the Rijndael algorithm.
        /// </summary>
        /// <param name="input">Input to decrypt</param>
        /// <param name="key">Decrypt key</param>
        /// <returns>Decrypted data</returns>
        string Decrypt(byte[] input, byte[] key);

        /// <summary>
        /// Encrypt data using the Rijndael algorithm.
        /// </summary>
        /// <param name="input">Input to encrypt</param>
        /// <param name="key">Encrypt key.</param>
        /// <returns>Encrypted data</returns>
        byte[] Encrypt(byte[] input, byte[] key);

        /// <summary>
        /// Gets a MD5 hash.
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Hashed input</returns>
        string GetMD5Hash(string input);

        /// <summary>
        /// Computes Crc32 checksum.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        uint Crc32ComputeChecksum(byte[] input);

        /// <summary>
        /// Comptures Crc32 checksum as a byte array.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        byte[] Crc32ComputeChecksumBytes(byte[] input);
    }
}
