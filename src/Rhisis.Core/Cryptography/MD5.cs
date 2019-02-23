using System.Text;
using MD5Hasher = System.Security.Cryptography.MD5;

namespace Rhisis.Core.Cryptography
{
    public static class MD5
    {
        /// <summary>
        /// Gets a MD5 hash.
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Hashed input</returns>
        public static string GetMD5Hash(string input)
        {
            byte[] data = null;
            var sBuilder = new StringBuilder();

            using (var md5Hash = MD5Hasher.Create())
                data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            for (var i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            return sBuilder.ToString();
        }

        /// <summary>
        /// Gets a MD5 hash.
        /// </summary>
        /// <param name="salt">Salt</param>
        /// <param name="input">Input</param>
        /// <returns>Hashed Salt+Input</returns>
        public static string GetMD5Hash(string salt, string input) => GetMD5Hash(salt + input);
    }
}
