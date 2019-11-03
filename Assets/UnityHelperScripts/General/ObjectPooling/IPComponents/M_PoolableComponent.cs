using UnityEngine;
using UnityEngine.Events;


public class M_PoolableComponent : MonoBehaviour, IPoolableComponent {

    [SerializeField] UnityEvent m_onSpawned = default;
    [SerializeField] UnityEvent m_onDespawned = default;
    public void Despawned()
    {
        m_onDespawned.Invoke();
    }

    public void Spawned()
    {
        m_onSpawned.Invoke();
    }
}
