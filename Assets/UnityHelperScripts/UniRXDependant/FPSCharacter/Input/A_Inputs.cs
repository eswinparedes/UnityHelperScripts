using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class A_Inputs : MonoBehaviour
{
    public abstract void Initialize();
    public abstract IObservable<MoveInputs> MoveInputs { get; protected set; }
    public abstract IObservable<Vector2> CameraLook { get; protected set; }

    public abstract IObservable<Unit> OnInteractStart { get; protected set; }
    public abstract IObservable<Unit> OnInteractEnd { get; protected set; }

    public abstract IObservable<Unit> OnFireStart { get; protected set; }
    public abstract IObservable<Unit> OnFireEnd { get; protected set; }

    public abstract IObservable<Unit> OnADSStart { get; protected set; }
    public abstract IObservable<Unit> OnADSEnd { get; protected set; }
}
