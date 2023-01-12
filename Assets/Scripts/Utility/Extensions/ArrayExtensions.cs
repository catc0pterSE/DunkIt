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
        
        public static Transform[] GetTransforms(this MonoBehaviour[] monoBehaviours)
        {
            Transform[] transforms = new Transform[monoBehaviours.Length];
            
            for (int i=0; i<monoBehaviours.Length; i++)
            {
                transforms[i] = monoBehaviours[i].transform;
            }

            return transforms;
        }
    }
}