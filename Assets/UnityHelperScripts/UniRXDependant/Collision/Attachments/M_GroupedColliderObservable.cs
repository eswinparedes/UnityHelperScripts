using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace SUHScripts
{
    using Functional;

    public class M_GroupedColliderObservable : A_ColliderObservable
    {
        [SerializeField] List<Collider> m_colliderGroup = default;

        Subject<ICollisionObservation> m_onEnter = new Subject<ICollisionObservation>();
        Subject<ICollisionObservation> m_onExit = new Subject<ICollisionObservation>();
    
        public override IObservable<ICollisionObservation> OnEnter => m_onEnter;
        public override IObservable<ICollisionObservation> OnExit => m_onExit;

        private void Awake()
        {
            //SUHS TODO: Instead of using list, possibly use Hashset since lists allow for duplicate references
            List<M_ColliderObservable> m_attachedObservables = new List<M_ColliderObservable>();

            m_colliderGroup
                .ForEach(col =>
                {
                    var colObservable =
                        col.gameObject
                        .GetComponentOption<M_ColliderObservable>()
                        .Reduce(col.gameObject.AddComponent<M_ColliderObservable>());

                    m_attachedObservables.Add(colObservable);
                });

            var collisionsSustained = new Dictionary<Collider, int>();
        
            var anyEnterObserved =
                m_attachedObservables
                .OnEnterAny()
                .Subscribe(output =>
                {
                    if(!collisionsSustained.Any(output.CollidingOther))
                        m_onEnter.OnNext(output);
        
                    collisionsSustained.Increment(output.CollidingOther);
                
                })
                .AddTo(this);

            var anyExitObserved =
                m_attachedObservables
                .OnExitAny()
                .Subscribe(output =>
                {
                    collisionsSustained.Decrement(output.CollidingOther);

                    if (!collisionsSustained.Any(output.CollidingOther))
                        m_onExit.OnNext(output);

                })
                .AddTo(this);
        }
    }
}

