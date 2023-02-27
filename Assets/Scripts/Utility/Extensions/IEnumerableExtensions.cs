using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            foreach (T item in array)
                if (prediction(item))
                    return item;

            return null;
        }

        public static bool FindFirstInactive<T>(this IEnumerable<T> array, out T component) where T : MonoBehaviour
        {
            component = null;

            foreach (T item in array)
                if (item.gameObject.activeSelf == false)
                {
                    component = item;
                    return true;
                }

            return false;
        }
    }
}