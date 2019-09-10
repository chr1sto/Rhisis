using System;
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
        public static bool IsValidEmail(this string source) 
            => new EmailAddressAttribute().IsValid(source);

        /// <summary>
        /// Converts a string to a given enumeration type.
        /// </summary>
        /// <typeparam name="T">Enumeration type.</typeparam>
        /// <param name="source">Source</param>
        /// <returns>String source converted as given enum.</returns>
        public static T ToEnum<T>(this string source) where T : struct, IConvertible, IComparable, IFormattable 
            => Enum.TryParse(source, true, out T result) ? result : default;
    }
}
