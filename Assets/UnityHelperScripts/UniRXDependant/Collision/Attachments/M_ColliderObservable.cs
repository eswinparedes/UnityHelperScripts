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

        CollisionObservationMutable m_enterColObvs = new CollisionObservationMutable();
        CollisionObservationMutable m_exitColObvs = new CollisionObservationMutable();

        private void Awake()
        {
            m_onEnter.AddTo(this);
            m_onExit.AddTo(this);
        }
        private void OnCollisionEnter(Collision collision)
        {
            m_enterColObvs.CollisionData = collision;
            m_enterColObvs.CollidingOther = collision.collider;
            m_onEnter.OnNext(m_enterColObvs);
        }

        private void OnCollisionExit(Collision collision)
        {
            m_exitColObvs.CollisionData = collision;
            m_exitColObvs.CollidingOther = collision.collider;
            m_onExit.OnNext(m_enterColObvs);
        }

        private void OnTriggerEnter(Collider other)
        {
            m_enterColObvs.CollisionData = NONE;
            m_enterColObvs.CollidingOther = other;
            m_onEnter.OnNext(m_enterColObvs);
        }

        private void OnTriggerExit(Collider other)
        {
            m_exitColObvs.CollisionData = NONE;
            m_exitColObvs.CollidingOther = other;
            m_onExit.OnNext(m_enterColObvs); ;
        }
    }
}
