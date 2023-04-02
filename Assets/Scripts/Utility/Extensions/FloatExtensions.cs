using UnityEngine;

namespace Utility.Extensions
{
    public static class FloatExtensions
    {
        public static float Map(this float value, float from1, float to1, float from2, float to2) =>
            (value - from1) / (to1 - from1) * (to2 - from2) + from2;

        public static float MapClamped(this float value, float from1, float to1, float from2, float to2) =>
            Mathf.Clamp(value.Map(from1, to1, from2, to2), from2, to2);
        
    }
}