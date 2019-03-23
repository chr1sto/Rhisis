using System.ComponentModel.DataAnnotations;

namespace Rhisis.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Check if the string is a valid email address.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsValidEmail(this string source) => new EmailAddressAttribute().IsValid(source);
    }
}
