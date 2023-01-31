using UnityEngine;

namespace Utility.Extensions
{
    public static class TransformExtensions
    {
        public static void Reset(this Transform transform, bool resetScale = true)
        {
            ResetParent(transform);
            ResetPosition(transform);
            ResetRotation(transform);

            if (resetScale)
                ResetScale(transform);
        }

        public static void CopyValuesFrom(this Transform transform, Transform other)
        {
            transform.position = other.position;
            transform.rotation = other.rotation;
            transform.localScale = other.localScale;
        }

        private static void ResetParent(UnityEngine.Transform transform)
        {
            if (transform.parent != null)
                transform.parent = null;
        }

        private static void ResetPosition(UnityEngine.Transform transform) =>
            transform.position = Vector3.zero;

        private static void ResetScale(UnityEngine.Transform transform) =>
            transform.localScale = Vector3.one;

        private static void ResetRotation(UnityEngine.Transform transform)
        {
            Quaternion quaternion = transform.rotation;
            quaternion.eulerAngles = Vector3.zero;
            transform.rotation = quaternion;
        }
    }
}