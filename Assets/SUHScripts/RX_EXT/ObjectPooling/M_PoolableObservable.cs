using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace SUHScripts
{
    public class M_PoolableObservable : MonoBehaviour, IPoolableComponent
    {
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
            m_onDespawn.OnNext(Unit.Default);
        }

        public void Spawned()
        {
            m_onSpawn.OnNext(Unit.Default);
        }
    }
}