using UnityEngine;
using UniRx;
using System;
using System.Collections.Generic;
using SUHScripts.Functional;

[System.Serializable]
public class CameraBob
{
    public float walkBobMagnitude = 0.05f;
    public float runBobMagnitude = 0.1f;
    public AnimationCurve bob;
    public float walkRunInterpolation = 7f;
    Vector3 initialPosition;

    FPSRoot components;
    Component attachedBehaviour;
    I_CharacterStrideSignals strideLogger;

    List<IDisposable> subscriptions = new List<IDisposable>();

    public void Initialize(Component attachedBehaviour, FPSRoot components, I_CharacterStrideSignals strideLogger)
    {
        this.attachedBehaviour = attachedBehaviour;
        this.strideLogger = strideLogger;
        this.components = components;

        initialPosition = components.FPSCamera.RootTransform.localPosition;
    }

    public void Subscribe()
    {
        components.FPSCamera.RequestAddOffsetProvider(this);

        bool run = false;
        float magCurrent = 0;

        var moveInputSubscription =
            components
            .Inputs
            .MoveInputs
            .Subscribe(i => run = i.sprint.IsTrue)
            .AddTo(attachedBehaviour);

        var walkedSubscription =
            strideLogger
            .StrideAlphaUpdate
            .Subscribe(w =>
            {
                var magnitude = run ? runBobMagnitude : walkBobMagnitude;
                magCurrent = Mathf.Lerp(magCurrent, magnitude, walkRunInterpolation * Time.fixedDeltaTime);
                var deltaPos = magCurrent * bob.Evaluate(w) * Vector3.up;

                components.FPSCamera.RequestOffset(this, deltaPos);
            })
            .AddTo(attachedBehaviour);

        subscriptions.Add(walkedSubscription);
        subscriptions.Add(moveInputSubscription);
    }

    public void Dispose()
    {
        components.FPSCamera.RequestRemoveOffsetProvider(this);
        subscriptions.ForEach(sub => sub.Dispose());
        subscriptions.Clear();
    }
}
