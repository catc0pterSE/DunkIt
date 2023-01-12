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
    }
}