using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

[System.Serializable]
public class StrideLogger : I_CharacterStrideSignals
{
    [SerializeField] float m_strideLength = 1;

    Subject<Unit> m_strideLogged;
    Subject<float> m_strideUpdate;

    public IObservable<Unit> StrideLogged => m_strideLogged;
    public IObservable<float> StrideAlphaUpdate => m_strideUpdate;
    public float StrideLength => m_strideLength;

    Component attachedBehaviour;
    IObservable<CharacterMovementOutput> m_observablePlayerOutput;

    List<IDisposable> m_subscriptions = new List<IDisposable>();

    public void Initialize(Component attachedBehaviour, IObservable<CharacterMovementOutput> player)
    {
        this.attachedBehaviour = attachedBehaviour;
        this.m_observablePlayerOutput = player;

        m_strideLogged = new Subject<Unit>();
        m_strideUpdate = new Subject<float>();
    }

    public void Subscribe()
    {
        var MUT_StepDistance = 0f;

        var walkedSubscription =
            m_observablePlayerOutput
            .Where(inputs => inputs.OutputControllerState.HasBeenGrounded())
            .Where(inputs => inputs.IsWalkRun())
            .Select(inputs => inputs.OutputMovementRequest.HorizontalMovement.Movement * inputs.OutputMovementRequest.deltaTime)
            .Subscribe(walked =>
            {
                MUT_StepDistance += walked.magnitude;
                if (MUT_StepDistance > m_strideLength)
                    m_strideLogged.OnNext(Unit.Default);

                MUT_StepDistance %= m_strideLength;

                m_strideUpdate.OnNext(MUT_StepDistance / m_strideLength);

            }).AddTo(attachedBehaviour);

        m_subscriptions.Add(walkedSubscription);
    }

    public void Dispose()
    {
        m_subscriptions.ForEach(sub => sub.Dispose());
        m_subscriptions.Clear();
    }
}
