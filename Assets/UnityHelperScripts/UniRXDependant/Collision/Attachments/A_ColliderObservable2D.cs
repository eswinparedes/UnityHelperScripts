using SUHScripts.Functional;
using System;
using UnityEngine;

namespace SUHScripts
{
    using Functional;
    public abstract class A_ColliderObservable2D : MonoBehaviour
    {
        public abstract IObservable<(Option<Collision2D> col, Collider2D other)> OnEnter { get; }
        public abstract IObservable<(Option<Collision2D> col, Collider2D other)> OnExit { get; }
    }
}

