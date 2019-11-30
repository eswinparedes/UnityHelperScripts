using System;
using System.Collections.Generic;
using System.Linq;
using SUHScripts.Functional;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using static SUHScripts.Functional.Functional;

public class M_ColliderObservable2DGrouped : A_ColliderObservable2D
{
    [SerializeField] List<Collider2D> m_colliderGroup = default;

    Subject<(Option<Collision2D> col, Collider2D other)> m_onEnter = new Subject<(Option<Collision2D> col, Collider2D other)>();
    Subject<(Option<Collision2D> col, Collider2D other)> m_onExit = new Subject<(Option<Collision2D> col, Collider2D other)>();

    public override IObservable<(Option<Collision2D> col, Collider2D other)> OnEnter => m_onEnter;
    public override IObservable<(Option<Collision2D> col, Collider2D other)> OnExit => m_onExit;

    private void Awake()
    {
        //SUHS TODO: Instead of using list, possibly use Hashset since lists allow for duplicate references
        List<M_ColliderObservable2D> m_attachedObservables = new List<M_ColliderObservable2D>();

        m_colliderGroup
            .ForEach(col =>
            {
                var colObservable =
                    col.gameObject
                    .GetComponentOption<M_ColliderObservable2D>()
                    .Reduce(col.gameObject.AddComponent<M_ColliderObservable2D>());

                m_attachedObservables.Add(colObservable);
            });

        var collisionsSustained = new Dictionary<Collider2D, int>();

        var anyEnterObserved =
            m_attachedObservables
            .OnEnterAny()
            .Subscribe(output =>
            {
                if (!collisionsSustained.Any(output.other))
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
