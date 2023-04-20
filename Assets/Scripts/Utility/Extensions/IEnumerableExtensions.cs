using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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
        
         public static Vector3[] GetTransformPositions(this IEnumerable<Transform> enumerable)
        {
            var transforms = enumerable as Transform[] ?? enumerable.ToArray();
            
            if (transforms.Length == 0)
                throw new Exception("Array is empty");
            
            if (enumerable == null)
                throw new NullReferenceException("Array is not initialized");
            
            Vector3[] positions = new Vector3[transforms.Length];

            for (int i = 0; i < transforms.Length; i++)
            {
                positions[i] = transforms[i].position;
            }

            return positions;
        }

        public static T GetRandom<T>(this IEnumerable<T> enumerable)
        {
            var array = enumerable as T[] ?? enumerable.ToArray();
            
            if (array.Length == 0)
                throw new Exception("Array is empty");
            
            if (array == null)
                throw new NullReferenceException("Array is not initialized");

            return array[Random.Range(0, array.Length)];
        }
        
        public static T FindClosestToDirection<T>(this IEnumerable<T> enumerable, Vector3 direction, Vector3 position) where T: MonoBehaviour
        {
            var array = enumerable as T[] ?? enumerable.ToArray();
            
            if (array.Length == 0)
                throw new Exception("Array is empty");
            
            if (array == null)
                throw new NullReferenceException("Array is not initialized");
            
            float minAngle = Single.MaxValue;
            T closest = null;

            foreach (T element in array)
            {
                
                float angle = Vector3.Angle(direction, element.transform.position - position);

                if (angle < minAngle)
                {
                    closest = element;
                    minAngle = angle;
                }
            }
            
            return closest;
        }
        
        public static T FindClosest<T>(this IEnumerable<T> enumerable, Vector3 position) where T: MonoBehaviour
        {
            var array = enumerable as T[] ?? enumerable.ToArray();
            
            if (array.Length == 0)
                throw new Exception("Array is empty");
            
            if (array == null)
                throw new NullReferenceException("Array is not initialized");
            
            float minDistance = Single.MaxValue;
            T closest = null;

            foreach (T element in array)
            {
                float distance = Vector3.Distance(element.transform.position, position);

                if (distance < minDistance)
                {
                    closest = element;
                    minDistance = distance;
                }
            }
            
            return closest;
        }

        public static Vector3 FindClosest(this IEnumerable<Vector3> enumerable, Vector3 position)
        {
            var points = enumerable as Vector3[] ?? enumerable.ToArray();
            
            if (points.Length == 0)
                throw new Exception("Array is empty");
            
            if (points == null)
                throw new NullReferenceException("Array is not initialized");
            
            float minDistance = Single.MaxValue;
            Vector3 closestPoint = Vector3.zero;

            foreach (Vector3 point in points)
            {
                float distance = Vector3.Distance(point, position);

                if (distance < minDistance)
                {
                    closestPoint = point;
                    minDistance = distance;
                }
            }

            return closestPoint;
        }

        public static Vector3 FindFarthest(this IEnumerable<Vector3> enumerable, Vector3 position)
        {
            var points = enumerable as Vector3[] ?? enumerable.ToArray();
            
            if (points.Length == 0)
                throw new Exception("Array is empty");
            
            if (points == null)
                throw new NullReferenceException("Array is not initialized");
            
            float maxDistance = Single.MinValue;
            Vector3 farthestPoint = Vector3.zero;

            foreach (Vector3 point in points)
            {
                float distance = Vector3.Distance(point, position);

                if (distance > maxDistance)
                {
                    farthestPoint = point;
                    maxDistance = distance;
                }
            }

            return farthestPoint;
        }

        public static Transform[] GetTransforms(this IEnumerable<MonoBehaviour> enumerable)
        {
            var monoBehaviours = enumerable as MonoBehaviour[] ?? enumerable.ToArray();
            
            if (monoBehaviours.Length == 0)
                throw new Exception("Array is empty");
            
            if (monoBehaviours == null)
                throw new NullReferenceException("Array is not initialized");
            
            Transform[] transforms = new Transform[monoBehaviours.Length];

            for (int i = 0; i < monoBehaviours.Length; i++)
            {
                transforms[i] = monoBehaviours[i].transform;
            }

            return transforms;
        }

        public static Vector3 GetIntermediatePosition(this IEnumerable<Vector3> enumerable)
        {
            var positions = enumerable as Vector3[] ?? enumerable.ToArray();
            
            if (positions.Length == 0)
                throw new Exception("Array is empty");
            
            if (positions == null)
                throw new NullReferenceException("Array is not initialized");
            
            Vector3 intermediatePosition = new Vector3();

            foreach (Vector3 position in positions)
            {
                intermediatePosition += position;
            }

            intermediatePosition /= positions.Length;

            return intermediatePosition;
        }
    }
}