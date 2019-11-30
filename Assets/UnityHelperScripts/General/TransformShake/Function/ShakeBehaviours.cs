using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using static NoiseGenerationOperations;

public static class ShakeBehaviours
{
    //SUHS TODO: Way to remove shake stream subscriptions???
    public static (IDisposable subscription, IObservable<Vector3> noiseFunction) SubscribeTo
        (IObservable<float> tickStream, IObservable<INoiseGenerator> noiseStream)
    {
        List<NoiseGenerator> noiseGenerators = new List<NoiseGenerator>();
        Vector3 output = Vector3.zero;

        var noiseStreamSub =
            noiseStream
            .Subscribe(noiseProvider => noiseGenerators.Add(noiseProvider.BuildGenerator()));

        var tickStreamSub =
            tickStream
            .Subscribe(deltaTime =>
            {
                var input = new NoiseInput(deltaTime, 1, 1);
                output = noiseGenerators.EvaluateAllWithKill(input);
            });

        var disp = Disposable.Create(() =>
        {
            noiseStreamSub.Dispose();
            tickStreamSub.Dispose();
        });

        return (disp, tickStream.Select(_ => output));
    }

    public static IDisposable SubscribeTo(this TransformShakeSubscriber @this, Transform shakeTransform, IObservable<float> tickStream, IObservable<SO_TransfromShakeData> shakeStream)
    {
        var baseScale = shakeTransform.localScale;
        var baseEulerAngles = shakeTransform.localPosition;
        var baseRotation = shakeTransform.localEulerAngles;

        List<NoiseGenerator> positionGenerators = new List<NoiseGenerator>();
        List<NoiseGenerator> rotationGenerators = new List<NoiseGenerator>();
        List<NoiseGenerator> scaleGenerators = new List<NoiseGenerator>();

        var shakeStreamSub =
            shakeStream
            .Subscribe(data =>
            {
                positionGenerators.Add(data.PositionData.Generator.BuildGenerator());
                rotationGenerators.Add(data.RotationData.Generator.BuildGenerator());
                scaleGenerators.Add(data.ScaleData.Generator.BuildGenerator());
            });

        var tickStreamSub =
            tickStream
            .Subscribe(deltaTime =>
            {
                var input = new NoiseInput(deltaTime, @this.AmplitudeMultiplier, @this.FrequencyMultiplier);

                Vector3 positionMovement = positionGenerators.EvaluateAllWithKill(input);
                Vector3 rotationMovement = rotationGenerators.EvaluateAllWithKill(input);
                Vector3 scaleMovement = scaleGenerators.EvaluateAllWithKill(input);

                shakeTransform.localPosition = baseEulerAngles + Vector3.Scale(@this.PositionScale, positionMovement);
                shakeTransform.localEulerAngles = baseRotation + Vector3.Scale(@this.RotationScale, rotationMovement);
                shakeTransform.localScale = baseScale + Vector3.Scale(@this.ScaleScale, scaleMovement);
            });

        return Disposable.Create(() =>
        {
            shakeStreamSub.Dispose();
            tickStreamSub.Dispose();
        });
    }
}
