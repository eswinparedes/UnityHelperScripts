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
        public static IObservable<Vector3> ObserveNoiseGenerators(this IObservable<float> sourceTick, IObservable<INoiseGenerator> noiseStream, Func<List<Vector3>, Vector3> agg) =>
            Observable.Create<Vector3>(observer =>
            {
                List<NoiseGenerator> noiseGenerators = new List<NoiseGenerator>();
                List<Vector3> noise = new List<Vector3>();

                var noiseStreamSub =
                    noiseStream
                    .Subscribe(
                        noiseProvider =>
                        {
                            noiseGenerators.Add(noiseProvider.BuildGenerator());
                            noise.Add(Vector3.zero);
                        }, 
                        observer.OnError, 
                        observer.OnCompleted);

                var tickStreamSub = sourceTick.Subscribe(
                        onNext: deltaTime =>
                        {
                            var input = new NoiseInput(deltaTime, 1, 1);

                            for (int i = noiseGenerators.Count - 1; i >= 0; i--)
                            {
                                var result = noiseGenerators[i](input);
                                if (!result.Noise.IsSome)
                                {
                                    noiseGenerators.RemoveAt(i);
                                    noise.RemoveAt(i);
                                }
                                else
                                {
                                    noise[i] = result.Noise.Value;
                                }
                            }

                            if(noiseGenerators.Count > 0)
                            {
                                observer.OnNext(agg(noise));
                            }   
                        },
                        observer.OnError,
                        observer.OnCompleted);

                return Disposable.Create(() =>
                {
                    noiseGenerators.Clear();
                    noiseStreamSub.Dispose();
                    tickStreamSub.Dispose();
                });
            });
    }
}
