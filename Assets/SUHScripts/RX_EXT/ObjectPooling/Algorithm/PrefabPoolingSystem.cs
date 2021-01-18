﻿using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts
{
    public static class PrefabPoolingSystem {

        static Dictionary<GameObject, PrefabPool> _prefabPoolMap = new Dictionary<GameObject, PrefabPool>();

        static Dictionary<GameObject, PrefabPool> _goToPoolMap = new Dictionary<GameObject, PrefabPool>();

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (!_prefabPoolMap.ContainsKey(prefab))
            {
                _prefabPoolMap.Add(prefab, new PrefabPool());
            }

            PrefabPool pool = _prefabPoolMap[prefab];
            GameObject go = pool.Spawn(prefab, position, rotation);
            _goToPoolMap.Add(go, pool);
            return go;
        }

        public static GameObject Spawn(GameObject prefab)
        {
            return Spawn(prefab, Vector3.zero, Quaternion.identity);
        }

        public static bool GetIsObjectMangedByPool(GameObject obj)
        {
            return _goToPoolMap.ContainsKey(obj);
        }

        public static bool Despawn(GameObject obj)
        {
            if (!_goToPoolMap.ContainsKey(obj))
            {
                Debug.LogError(string.Format(" Object {0} not managed by pool system!", obj.name));
                return false;
            }

            PrefabPool pool = _goToPoolMap[obj];
            if (pool.Despawn(obj))
            {
                _goToPoolMap.Remove(obj);
                return true;
            }

            return false;
        }

        public static void Prespawn(GameObject prefab, int numToSpawn)
        {
            List<GameObject> spawnedObjects = new List<GameObject>();

            for(int i = 0; i < numToSpawn; i++)
            {
                spawnedObjects.Add(Spawn(prefab));
            }

            for(int i = 0; i < numToSpawn; i++)
            {
                Despawn(spawnedObjects[i]);
            }

            spawnedObjects.Clear();
        }

        public static void Reset()
        {
            foreach (var kvp in _goToPoolMap)
            {

            }

            _prefabPoolMap.Clear();
            _goToPoolMap.Clear();

            
        }
    }

}