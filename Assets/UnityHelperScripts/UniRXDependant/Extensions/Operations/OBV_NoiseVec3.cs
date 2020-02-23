using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using SUHScripts;
using SUHScripts.Functional;
using System;
using static SUHScripts.NoiseGenerationOperations;

namespace SUHScripts
{
    public static class OBV_NoiseVec3
    {
        /// <summary>
        /// Observes A Stream of NoiseGenerators and returns an update of those generated from "sourceTick" ticks.  Terminates with either
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceTick"></param>
        /// <param name="noiseStream"></param>
        /// <returns></returns>
        public static IObservable<Vector3> ObserveNoiseGenerators(this IObservable<float> sourceTick, IObservable<INoiseGenerator> noiseStream, Func<IEnumerable<Vector3>, Vector3> agg) =>
            Observable.Create<Vector3>(observable =>
            {
                List<NoiseGenerator> noiseGenerators = new List<NoiseGenerator>();

                var noiseStreamSub =
                    noiseStream
                    .Subscribe(noiseProvider => noiseGenerators.Add(noiseProvider.BuildGenerator()));

                var tickStreamSub =
                    sourceTick
                    .TakeDuring(noiseStream)
                    .Subscribe(
                        onNext: deltaTime =>
                        {
                            var input = new NoiseInput(deltaTime, 1, 1);
                            var output = noiseGenerators.YieldEvaluationsWithKill(input);
                            observable.OnNext(agg(output));
                        },
                        observable.OnError,
                        observable.OnCompleted);

                return Disposable.Create(() =>
                {
                    noiseGenerators.Clear();
                    noiseStreamSub.Dispose();
                    tickStreamSub.Dispose();
                });
            });
    }
}
