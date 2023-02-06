using System;
using UnityEngine;

namespace Utility.Extensions
{
    public static class ArrayExtensions
    {
        public static Vector3[] GetTransformPositions(this Transform[] transforms)
        {
            Vector3[] positions = new Vector3[transforms.Length];
            
            for (int i=0; i<transforms.Length; i++)
            {
                positions[i] = transforms[i].position;
            }

            return positions;
        }
        
        public static Transform FindClosest(this Transform[] transforms, Vector3 position)
        {
           float minDistance = Single.PositiveInfinity;
           Transform closestTransform = null;

           foreach (Transform transform in transforms)
           {
               if (Vector3.Distance(transform.position, position) < minDistance)
                   closestTransform = transform;
           }
           
           return closestTransform;
        }
        
        public static Transform[] GetTransforms(this MonoBehaviour[] monoBehaviours)
        {
            Transform[] transforms = new Transform[monoBehaviours.Length];
            
            for (int i=0; i<monoBehaviours.Length; i++)
            {
                transforms[i] = monoBehaviours[i].transform;
            }

            return transforms;
        }

        public static Vector3 GetIntermediatePosition(this Vector3[] positions)
        {
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