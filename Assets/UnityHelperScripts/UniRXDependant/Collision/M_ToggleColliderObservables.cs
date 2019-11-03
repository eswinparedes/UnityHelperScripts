using SUHScripts.Functional;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using static SUHScripts.Functional.Functional;

public class M_ToggleColliderObservables : A_ColliderObservable
{
    [SerializeField] A_ColliderObservable m_onEnterCollider = default;
    [SerializeField] A_ColliderObservable m_onExitCollider = default;

    Subject<(Option<Collision> col, Collider other)> m_onEnter = new Subject<(Option<Collision> col, Collider other)>();
    Subject<(Option<Collision> col, Collider other)> m_onExit = new Subject<(Option<Collision> col, Collider other)>();
   
    public override IObservable<(Option<Collision> col, Collider other)> OnEnter => m_onEnter;
    public override IObservable<(Option<Collision> col, Collider other)> OnExit => m_onExit;

    private void Awake()
    {
        var collidersEntered = new HashSet<Collider>();

        m_onEnterCollider
            .OnEnter
            .Where(result => !collidersEntered.Contains(result.other))
            .Subscribe(result =>
            {
                collidersEntered.Add(result.other);
                m_onEnter.OnNext(result);
            })
            .AddTo(this);

        m_onExitCollider
            .OnEnter
            .Where(result => collidersEntered.Contains(result.other))
            .Subscribe(result =>
            {
                collidersEntered.Remove(result.other);
                m_onExit.OnNext(result);
            })
            .AddTo(this);
    }
}