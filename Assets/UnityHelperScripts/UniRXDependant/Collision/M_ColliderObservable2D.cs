using System;
using System.Collections.Generic;
using System.Linq;
using SUHScripts.Functional;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using static SUHScripts.Functional.Functional;

public class M_ColliderObservable2D: A_ColliderObservable2D
{

    Subject<(Option<Collision2D> col, Collider2D other)> m_onEnter = new Subject<(Option<Collision2D> col, Collider2D other)>();
    Subject<(Option<Collision2D> col, Collider2D other)> m_onExit = new Subject<(Option<Collision2D> col, Collider2D other)>();


    public override IObservable<(Option<Collision2D> col, Collider2D other)> OnEnter => m_onEnter;
    public override IObservable<(Option<Collision2D> col, Collider2D other)> OnExit => m_onExit;


    private void Awake()
    {
        m_onEnter.AddTo(this);
        m_onExit.AddTo(this);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_onEnter.OnNext((collision, collision.collider));
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        m_onExit.OnNext((collision, collision.collider));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        m_onEnter.OnNext((NONE, other));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        m_onExit.OnNext((NONE, other));
    }
}
