using System;
using UnityEngine;
using UniRx;

namespace SUHScripts
{
    using Functional;
    using static Functional.Functional;
    [DisallowMultipleComponent]
    public class M_ColliderObservable : A_ColliderObservable
    {
        Subject<ICollisionObservation> m_onEnter = new Subject<ICollisionObservation>();
        Subject<ICollisionObservation> m_onExit = new Subject<ICollisionObservation>();
   
        public override IObservable<ICollisionObservation> OnEnter => m_onEnter;
        public override IObservable<ICollisionObservation> OnExit => m_onExit;

        private void Awake()
        {
            m_onEnter.AddTo(this);
            m_onExit.AddTo(this);
        }
        private void OnCollisionEnter(Collision collision)
        {
            var col = new CollisionObserved(collision, collision.collider);
            m_onEnter.OnNext(col);
        }

        private void OnCollisionExit(Collision collision)
        {
            var col = new CollisionObserved(collision, collision.collider);
            m_onExit.OnNext(col);
        }

        private void OnTriggerEnter(Collider other)
        {
            var col = new CollisionObserved(NONE, other);
            m_onEnter.OnNext(col);
        }

        private void OnTriggerExit(Collider other)
        {
            var col = new CollisionObserved(NONE, other);
            m_onExit.OnNext(col); 
        }
    }
}
