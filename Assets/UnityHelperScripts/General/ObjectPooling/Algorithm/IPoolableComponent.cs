using UnityEngine;

public interface IPoolableComponent
{
    void Spawned();
    void Despawned();
}

public struct PoolablePrefabData
{
    public GameObject go;
    public IPoolableComponent poolableComponent;
}
