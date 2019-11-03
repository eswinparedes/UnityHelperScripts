using System;
using System.Collections.Generic;
using System.Linq;
using SUHScripts.Functional;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using static SUHScripts.Functional.Functional;

public class M_GroupedColliderObservable : A_ColliderObservable
{
    [SerializeField] List<Collider> m_colliderGroup = default;

    Subject<(Option<Collision> col, Collider other)> m_onEnter = new Subject<(Option<Collision> col, Collider other)>();
    Subject<(Option<Collision> col, Collider other)> m_onExit = new Subject<(Option<Collision> col, Collider other)>();
    
    public override IObservable<(Option<Collision> col, Collider other)> OnEnter => m_onEnter;
    public override IObservable<(Option<Collision> col, Collider other)> OnExit => m_onExit;

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
                if(!collisionsSustained.Any(output.other))
                    m_onEnter.OnNext(output);
        
                collisionsSustained.Increment(output.other);
                
            })
            .AddTo(this);

        var anyExitObserved =
            m_attachedObservables
            .OnExitAny()
            .Subscribe(output =>
            {
                collisionsSustained.Decrement(output.other);

                if (!collisionsSustained.Any(output.other))
                    m_onExit.OnNext(output);

            })
            .AddTo(this);
    }
}
