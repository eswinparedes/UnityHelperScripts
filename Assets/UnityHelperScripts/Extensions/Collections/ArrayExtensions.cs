using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace SUHScripts
{
    public static class ArrayExtensions
    {
        public static T RandomElement<T>(this T[] arr)
        {
            return arr[UnityEngine.Random.Range(0, arr.Length)];
        }
    }

}
