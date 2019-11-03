using System.Collections.Generic;
using UnityEngine;

public class M_Test_Pool : MonoBehaviour {

    [SerializeField] GameObject _prefab = default;

    List<GameObject> _prefabs = new List<GameObject>();

    private void Start()
    {
        PrefabPoolingSystem.Prespawn(_prefab, 11);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnObject(_prefab, _prefabs);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            DespawnRandomObject(_prefabs);
        }
    }

    void SpawnObject(GameObject prefab, List<GameObject> list)
    {
        GameObject obj = PrefabPoolingSystem.Spawn(prefab, 5.0f * Random.insideUnitSphere, Quaternion.identity);
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

