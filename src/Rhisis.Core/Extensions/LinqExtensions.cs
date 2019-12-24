using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Core.Extensions
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Groups a defined number of elements into a single array.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="itemsPerGroup"></param>
        /// <returns></returns>
        public static IEnumerable<IGrouping<int, TSource>> GroupBy<TSource>(this IEnumerable<TSource> source, int itemsPerGroup)
        {
            return source.Zip(Enumerable.Range(0, source.Count()), (s, r) => new { Group = r / itemsPerGroup, Item = s })
                        .GroupBy(i => i.Group, g => g.Item)
                        .ToList();
        }

        /// <summary>
        /// Check if there is any duplicate in a collection based on an expression.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool HasDuplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> expression)
        {
            return source.GroupBy(expression).Any(x => x.Count() > 1);
        }
    }
}
