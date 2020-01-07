using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    public static class QuaternionExt 
    {
        public static Quaternion MaskedRotation(this Quaternion from, Vector3 mask)
        {
            var euler = from.eulerAngles;
            var eulerTarget = Vector3.Scale(euler, mask);
            return Quaternion.Euler(eulerTarget);
        }

        /// <summary>
        /// Transforms vector from world space to local space
        /// </summary>
        /// <param name="this"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector3 InverseTransformDirection(this Quaternion @this, Vector3 direction) =>
            @this * direction;
    }
}

