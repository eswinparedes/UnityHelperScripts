using System.Collections.Generic;
using UnityEngine;

namespace SUHScripts.Tests
{
    public class M_Test_Pool : MonoBehaviour {

        [SerializeField] KeyCode m_spawnRandomKeycode = KeyCode.S;
        [SerializeField] KeyCode m_despawnRandom = KeyCode.D;

        [SerializeField] GameObject _prefab = default;

        List<GameObject> _prefabs = new List<GameObject>();

        private void Start()
        {
            PrefabPoolingSystem.Prespawn(_prefab, 11);
        }

        void Update()
        {
            if (Input.GetKeyDown(m_spawnRandomKeycode))
            {
                SpawnObject(_prefab, _prefabs);
            }
            if (Input.GetKeyDown(m_despawnRandom))
            {
                DespawnRandomObject(_prefabs);
            }
        }

        void SpawnObject(GameObject prefab, List<GameObject> list)
        {
            GameObject obj = PrefabPoolingSystem.Spawn(prefab, 5.0f * Random.insideUnitSphere, Random.rotationUniform);
            list.Add(obj);
        }

        void DespawnRandomObject(List<GameObject> list)
        {
            if(list.Count == 0)
            {
                return;
            }

            int i = Random.Range(0, list.Count);
            PrefabPoolingSystem.Despawn(list[i]);
            list.RemoveAt(i);
        }
    }


}
