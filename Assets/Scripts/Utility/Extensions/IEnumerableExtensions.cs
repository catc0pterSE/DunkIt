using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility.Extensions
{
    public static class EnumerableExtensions
    {
        public static void Map<T>(this IEnumerable<T> array, Action<T> function)
        {
            foreach (var item in array)
                function(item);
        }


        public static T FindFirstOrNull<T>(this IEnumerable<T> array, Func<T, bool> prediction) where T : class
        {
            if (array.GetEnumerator().MoveNext() == false)
                return null;

            foreach (T item in array)
                if (prediction(item))
                    return item;

            return null;
        }
    }
}