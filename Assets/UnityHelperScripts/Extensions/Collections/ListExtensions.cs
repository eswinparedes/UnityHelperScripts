using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{

    public static class ListExtensions
    {
        /// <summary>
        /// Returns True if Item was added
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        public static bool AddItemNotPresent<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns True if Item Was Removed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool RemoveItemPresent<T>(this List<T> list, T item)
        {
            if (list.Contains(item))
            {
                list.Remove(item);
                return true;
            }
            return false;
        }

        public static T RandomElement<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
    }
}