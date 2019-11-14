using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using static NoiseGenerationOperations;

public static class ShakeOperations 
{
    public static Vector3 EvaluateAllWithKill(this List<NoiseGenerator> @this, NoiseInput input)
    {
        Vector3 positionOffset = Vector3.zero;

        for (int i = @this.Count - 1; i >= 0; i--)
        {
            var se = @this[i](input);

            if (se.Noise.IsSome)
                positionOffset += se.Noise.Value;
            else
                @this.RemoveAt(i);
        }
        return positionOffset;
    }


    public static IDisposable SubscribeTo(this TransformShakeSubscriber @this, IObservable<float> tickStream, IObservable<SO_TransfromShakeData> shakeStream)
    {
        var baseScale = @this.ShakeTransform.localScale;
        var baseEulerAngles = @this.ShakeTransform.localPosition;
        var baseRotation = @this.ShakeTransform.localEulerAngles;

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

                @this.ShakeTransform.localPosition = baseEulerAngles + Vector3.Scale(@this.PositionScale, positionMovement);
                @this.ShakeTransform.localEulerAngles = baseRotation + Vector3.Scale(@this.RotationScale, rotationMovement);
                @this.ShakeTransform.localScale = baseScale + Vector3.Scale(@this.ScaleScale, scaleMovement);
            });

        return Disposable.Create(() =>
        {
            shakeStreamSub.Dispose();
            tickStreamSub.Dispose();
        });
    }
}
