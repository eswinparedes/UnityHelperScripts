using System;
using UnityEngine;
using SUHScripts.Functional;
using static SUHScripts.Functional.Functional;
using UniRx;

[DisallowMultipleComponent]
public class M_ColliderObservable : A_ColliderObservable
{
    Subject<(Option<Collision> col, Collider other)> m_onEnter = new Subject<(Option<Collision> col, Collider other)>();
    Subject<(Option<Collision> col, Collider other)> m_onExit = new Subject<(Option<Collision> col, Collider other)>();
   

    public override IObservable<(Option<Collision> col, Collider other)> OnEnter => m_onEnter;
    public override IObservable<(Option<Collision> col, Collider other)> OnExit => m_onExit;


    private void Awake()
    {
        m_onEnter.AddTo(this);
        m_onExit.AddTo(this);
    }
    private void OnCollisionEnter(Collision collision)
    {
        m_onEnter.OnNext((collision, collision.collider));
    }

    private void OnCollisionExit(Collision collision)
    {
        m_onExit.OnNext((collision, collision.collider));
    }

    private void OnTriggerEnter(Collider other)
    {
        m_onEnter.OnNext((NONE, other));
    }

    private void OnTriggerExit(Collider other)
    {
        m_onExit.OnNext((NONE, other));
    }
}