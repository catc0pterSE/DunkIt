using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility.Extensions
{
    public static partial class EnumerableExtension
    {
        public static IEnumerable<T> Map<T>(this IEnumerable<T> array, Action<T> function)
        {
            var enumerable = array.ToArray();

            foreach (var item in enumerable)
                function(item);

            return enumerable;
        }
    }
}