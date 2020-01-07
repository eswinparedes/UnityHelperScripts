using UnityEngine;

namespace SUHScripts
{
    public interface IPoolableComponent
    {
        void Spawned();
        void Despawned();
    }

    public struct PoolablePrefabData
    {
        public GameObject go;
        public IPoolableComponent[] poolableComponents;
    }
}

