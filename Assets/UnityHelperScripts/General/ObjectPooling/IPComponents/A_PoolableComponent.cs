using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class A_PoolableComponent : MonoBehaviour, IPoolableComponent
{
    [SerializeField] UnityEvent m_onSpawned = default;
    [SerializeField] UnityEvent m_onDespawned = default;

    Subject<Unit> m_onSpawn;
    Subject<Unit> m_onDespawn;

    public IObservable<Unit> OnSpawn => m_onSpawn;
    public IObservable<Unit> OnDespawn => m_onDespawn;

    protected virtual void Awake()
    {
        m_onSpawn = new Subject<Unit>().AddTo(this);
        m_onDespawn = new Subject<Unit>().AddTo(this);
    }
    public void Despawned()
    {
        m_onDespawned.Invoke();
        m_onDespawn.OnNext(Unit.Default);
    }

    public void Spawned()
    {
        m_onSpawned.Invoke();
        m_onSpawn.OnNext(Unit.Default);
    }
}
