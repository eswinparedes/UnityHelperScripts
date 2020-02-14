using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace SUHScripts
{
    using Functional;

    public class M_ToggleColliderObservables : A_ColliderObservable
    {
        [SerializeField] A_ColliderObservable m_onEnterCollider = default;
        [SerializeField] A_ColliderObservable m_onExitCollider = default;

        Subject<ICollisionObservation> m_onEnter = new Subject<ICollisionObservation>();
        Subject<ICollisionObservation> m_onExit = new Subject <ICollisionObservation>();
   
        public override IObservable<ICollisionObservation> OnEnter => m_onEnter;
        public override IObservable<ICollisionObservation> OnExit => m_onExit;

        private void Awake()
        {
            var collidersEntered = new HashSet<Collider>();

            m_onEnterCollider
                .OnEnter
                .Where(result => !collidersEntered.Contains(result.CollidingOther))
                .Subscribe(result =>
                {
                    collidersEntered.Add(result.CollidingOther);
                    m_onEnter.OnNext(result);
                })
                .AddTo(this);

            m_onExitCollider
                .OnEnter
                .Where(result => collidersEntered.Contains(result.CollidingOther))
                .Subscribe(result =>
                {
                    collidersEntered.Remove(result.CollidingOther);
                    m_onExit.OnNext(result);
                })
                .AddTo(this);
        }
    }
}
