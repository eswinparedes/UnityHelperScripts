using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    public static class TransformExtensions
    {
        public static Ray GetRayFromTransform(this Transform t, Vector3 relativeDirection) =>
            new Ray(t.position, t.InverseTransformDirection(relativeDirection));

        public static Ray GetRayFromTransform(this Transform t) =>
            new Ray(t.position, t.forward);

        public static Transform ClosestTransformTo(this IEnumerable<Transform> transforms, Vector3 position)
        {
            Transform closest = null;
            float dist = Mathf.Infinity;
            foreach (Transform item in transforms)
            {
                if ((item.position - position).sqrMagnitude < dist)
                {
                    closest = item;
                }
            }
            return closest;
        }

        public static Transform ClosestTransformTo(this IEnumerable<Transform> transforms, Transform otherTransform) =>
            transforms.ClosestTransformTo(otherTransform.position);

        public static Quaternion ConstrainedLookTo(this Transform lookFrom, Transform lookTo, Vector3 mask)
        {
            Vector3 lookToPos = Vector3.Scale(lookTo.position, mask);
            Vector3 lookFromPos = Vector3.Scale(lookFrom.position, mask);
            Vector3 lookToDiff = lookToPos - lookFromPos;
            return Quaternion.LookRotation(lookToDiff);
        }

        public static float DotProductLookToNormalized(this Transform lookFrom, Transform lookTo, Vector3? relativeLHS = null)
        {
            var vector = relativeLHS ?? Vector3.forward;

            Vector3 lhs = relativeLHS?? lookFrom.rotation * vector;
            Vector3 rhs = (lookTo.position - lookFrom.position).normalized;
            return Vector3.Dot(lhs, rhs);
        }
        public static float DotProductLookTo(this Transform lookFrom, Transform lookTo)
        {
            Vector3 lhs = lookFrom.forward;
            Vector3 rhs = (lookTo.position - lookFrom.position);
            return Vector3.Dot(lhs, rhs);
        }

        #region Offset
        public static Vector3 GetOffsetTo(this Transform from, Transform to) =>
        to.position - from.position;

        public static Vector3 GetOffsetTo(this Transform from, Transform to, Vector3 offsetMask) =>
            Vector3.Scale(from.GetOffsetTo(to), offsetMask);

        public static Vector3 GetOffsetTo(this Transform from, Vector3 to) =>
            to - from.position;

        public static Vector3 GetOffsetTo(this Transform from, Vector3 to, Vector3 offsetMask) =>
            Vector3.Scale(from.GetOffsetTo(to), offsetMask);

        public static Vector3 GetOffsetToLocal(this Transform from, Transform to) =>
            to.position - from.localPosition;

        public static Vector3 GetOffsetToLocal(this Transform from, Transform to, Vector3 offsetMask) =>
            Vector3.Scale(from.GetOffsetToLocal(to), offsetMask);

        public static Vector3 GEtOffsetToLocal(this Transform from, Vector3 to) =>
            to - from.localPosition;

        public static Vector3 GetOffsetToLocal(this Transform from, Vector3 to, Vector3 offsetMask) =>
            Vector3.Scale(from.GEtOffsetToLocal(to), offsetMask);
        #endregion
    }
}