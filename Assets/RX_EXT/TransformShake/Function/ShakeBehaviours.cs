using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using static SUHScripts.NoiseGenerationOperations;

namespace SUHScripts
{
    public static class ShakeBehaviours
    {
        public static IDisposable SubscribeTo(this TransformShakeSubscriber @this, Transform shakeTransform, IObservable<float> tickStream, IObservable<SO_TransfromShakeData> shakeStream)
        {
            var composite = new CompositeDisposable();

            var baseScale = shakeTransform.localScale;
            var basePosition = shakeTransform.localPosition;
            var baseRotation = shakeTransform.localEulerAngles;

            var posObvs = tickStream.ObserveNoiseGenerators(shakeStream.Select(d => d.PositionData.Generator), vs => vs.Sum());
            var eulerObvs = tickStream.ObserveNoiseGenerators(shakeStream.Select(d => d.RotationData.Generator), vs => vs.Sum());
            var scaleObvs = tickStream.ObserveNoiseGenerators(shakeStream.Select(d => d.ScaleData.Generator), vs => vs.Sum());

            var posSub = posObvs.Subscribe(v => shakeTransform.localPosition = basePosition + v).AddTo(composite);
            var eulerSub = eulerObvs.Subscribe(v => shakeTransform.localEulerAngles = baseRotation + v).AddTo(composite);
            var scaleSub = scaleObvs.Subscribe(v => shakeTransform.localScale = baseScale + v).AddTo(composite);

            return composite;
        }
    }
}

