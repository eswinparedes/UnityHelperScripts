﻿using SUHScripts.Functional;
using System;
using UnityEngine;

namespace SUHScripts
{
    using Functional;
    public abstract class A_ColliderObservable : MonoBehaviour
    {
        public abstract IObservable<CollisionObserved> OnEnter { get; }
        public abstract IObservable<CollisionObserved> OnExit { get; }
    }
}
