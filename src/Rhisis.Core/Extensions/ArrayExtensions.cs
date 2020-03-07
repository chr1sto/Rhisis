using System;
using System.Collections.Generic;

namespace Rhisis.Core.Extensions
{
    public static class ArrayExtensions
    {
        public static void Swap<T>(this T[] array, int source, int dest)
        {
            var temp = array[source];

            array[source] = array[dest];
            array[dest] = temp;
        }

        public static void Swap<T>(this IList<T> array, int source, int dest)
        {
            var temp = array[source];

            array[source] = array[dest];
            array[dest] = temp;
        }

        public static IEnumerable<T> GetRange<T>(this T[] array, int start, int count)
        {
            var rangedArray = new T[count];

            Array.Copy(array, start, rangedArray, 0, count);

            return rangedArray;
        }
    }
}
