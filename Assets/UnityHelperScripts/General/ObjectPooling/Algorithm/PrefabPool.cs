using System.Collections.Generic;
using UnityEngine;

public class PrefabPool {

    Dictionary<GameObject, PoolablePrefabData> _activeList = new Dictionary<GameObject, PoolablePrefabData>();

    Queue<PoolablePrefabData> _inactiveList = new Queue<PoolablePrefabData>();


	public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        PoolablePrefabData data;

        if(_inactiveList.Count > 0)
        {
            data = _inactiveList.Dequeue();
        }
        else
        {
            GameObject newGo = GameObject.Instantiate(prefab, position, rotation);
            data = new PoolablePrefabData();
            data.go = newGo;
            data.poolableComponents = newGo.GetComponentsInChildren<IPoolableComponent>();
        }

        data.go.SetActive(true);
        data.go.transform.position = position;
        data.go.transform.rotation = rotation;

        if(data.poolableComponents != null)
        {
            for(int i =0; i < data.poolableComponents.Length; i++)
            {
                data.poolableComponents[i].Spawned();
            }
        }

        _activeList.Add(data.go, data);

        return data.go;
    }

    public bool Despawn(GameObject obj)
    {
        if (!_activeList.ContainsKey(obj))
        {
            Debug.LogError("This object is not managed by this pool!");
            return false;
        }

        PoolablePrefabData data = _activeList[obj];

        if(data.poolableComponents != null)
        {
            for (int i = 0; i < data.poolableComponents.Length; i++)
            {
                data.poolableComponents[i].Despawned();
            }
        }

        data.go.SetActive(false);
        _activeList.Remove(obj);
        _inactiveList.Enqueue(data);
        return true;
    }
}
