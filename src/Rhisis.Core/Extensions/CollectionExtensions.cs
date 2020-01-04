using System.Collections;

namespace Rhisis.Core.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Converts a <see cref="ICollection"/> into an array of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="collection">Collection.</param>
        /// <returns>Array of <typeparamref name="T"/>.</returns>
        public static T[] ToArray<T>(this ICollection collection)
        {
            var array = new T[collection.Count];

            collection.CopyTo(array, 0);

            return array;
        }
    }
}
