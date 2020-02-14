using SUHScripts.Functional;
using System;
using UnityEngine;

namespace SUHScripts
{
    using Functional;
    public abstract class A_ColliderObservable : MonoBehaviour
    {
        public abstract IObservable<ICollisionObservation> OnEnter { get; }
        public abstract IObservable<ICollisionObservation> OnExit { get; }
    }
}
