using SUHScripts.Functional;
using System;
using UnityEngine;

public abstract class A_ColliderObservable : MonoBehaviour
{
    public abstract IObservable<(Option<Collision> col, Collider other)> OnEnter { get; }
    public abstract IObservable<(Option<Collision> col, Collider other)> OnExit { get; }
}