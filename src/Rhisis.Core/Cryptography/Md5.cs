using System.Security.Cryptography;
using System.Text;

namespace Rhisis.Core.Cryptography
{
    public static class Md5
    {
        /// <summary>
        /// Gets a MD5 hash from a string.
        /// </summary>
        /// <param name="input">string input</param>
        /// <returns>MD5 hash</returns>
        public static string GetMd5Hash(string input)
        {
            byte[] data = null;
            var sBuilder = new StringBuilder();

            using (MD5 md5Hash = MD5.Create())
            {
                data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            }

            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}