using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utility.Extensions
{
    public static class ArrayExtensions
    {
        /*public static Vector3[] GetTransformPositions(this Transform[] transforms)
        {
            if (transforms.Length == 0)
                throw new Exception("Array is empty");
            
            if (transforms == null)
                throw new NullReferenceException("Array is not initialized");
            
            Vector3[] positions = new Vector3[transforms.Length];

            for (int i = 0; i < transforms.Length; i++)
            {
                positions[i] = transforms[i].position;
            }

            return positions;
        }

        public static T GetRandom<T>(this T[] array)
        {
            if (array.Length == 0)
                throw new Exception("Array is empty");
            
            if (array == null)
                throw new NullReferenceException("Array is not initialized");

            return array[Random.Range(0, array.Length)];
        }

        public static Vector3 FindClosest(this Vector3[] points, Vector3 position)
        {
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

        public static Vector3 FindFarthest(this Vector3[] points, Vector3 position)
        {
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

        public static Transform[] GetTransforms(this MonoBehaviour[] monoBehaviours)
        {
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

        public static Vector3 GetIntermediatePosition(this Vector3[] positions)
        {
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
        }*/
    }
}