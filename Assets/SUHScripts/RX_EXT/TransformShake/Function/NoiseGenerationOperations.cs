using SUHScripts.Functional;
using System;
using System.Collections.Generic;
using UnityEngine;
using static SUHScripts.Functional.Functional;
using UniRx;

namespace SUHScripts
{
    
    public static class NoiseGenerationOperations
    {
        public static IObservable<Vector3> ObserveNoiseConstant(this IObservable<float> sourceTick, float frequency, float amplitude) =>
            Observable.Create<Vector3>(observer =>
            {
                Vector3 noiseOffset = Vector3Extensions.RandomComponents(new Vector2(0, 32));

                return sourceTick.Subscribe(deltaTime =>
                {
                    float noiseOffsetDelta = deltaTime * frequency;

                    noiseOffset.x += noiseOffsetDelta;
                    noiseOffset.y += noiseOffsetDelta;
                    noiseOffset.z += noiseOffsetDelta;

                    float x = Mathf.PerlinNoise(noiseOffset.x, 0.0f);
                    float y = Mathf.PerlinNoise(noiseOffset.x, 1.0f);
                    float z = Mathf.PerlinNoise(noiseOffset.x, 2.0f);

                    Vector3 noise = new Vector3(x, y, z);

                    noise -= Vector3.one * 0.5f;
                    noise *= amplitude;

                    observer.OnNext(noise);
                }, observer.OnError, observer.OnCompleted);
            });

        public static IObservable<Vector3> ObserveNoiseConstantStream(this IObservable<float> sourceTick, IObservable<PerlinNoiseConstant> noiseStream, Func<Vector3, Vector3, Vector3> agg, Func<int, float, Vector3, Vector3> resultCombiner) =>
            noiseStream.Select(noise => sourceTick.ObserveNoiseConstant(noise.Frequency, noise.Amplitude))
            .ReduceLatestBy(sourceTick, agg, resultCombiner);

        public static IObservable<Vector3> ObserveNoisePulse(this IObservable<float> sourceTick, float frequency, float amplitude, float duration, AnimationCurve blendOverLife) =>
            Observable.Create<Vector3>(observer =>
            {
                float timeRemaining = duration;
                Vector3 noiseOffset = Vector3Extensions.RandomComponents(new Vector2(0, 32));

                return sourceTick.Subscribe(delta =>
                    {
                        timeRemaining -= delta;

                        float noiseOffsetDelta = delta * frequency;

                        noiseOffset.x += noiseOffsetDelta;
                        noiseOffset.y += noiseOffsetDelta;
                        noiseOffset.z += noiseOffsetDelta;

                        Vector3 noise = new Vector3();

                        noise.x = Mathf.PerlinNoise(noiseOffset.x, 0.0f);
                        noise.y = Mathf.PerlinNoise(noiseOffset.x, 1.0f);
                        noise.z = Mathf.PerlinNoise(noiseOffset.x, 2.0f);

                        noise -= Vector3.one * 0.5f;

                        noise *= amplitude;

                        float agePercent = 1.0f - (timeRemaining / duration);
                        noise *= blendOverLife.Evaluate(agePercent);

                        if(timeRemaining > 0.0f)
                        {
                            observer.OnNext(noise);
                            
                        }
                        else
                        {
                            observer.OnCompleted();
                        }
                    }, observer.OnError, observer.OnCompleted);
            });

        public static IObservable<Vector3> ObserveNoisePulseStream(this IObservable<float> sourceTick, IObservable<PerlinNoisePulse> noiseStream, Func<Vector3, Vector3, Vector3> agg, Func<int, float, Vector3, Vector3> resultCombiner) =>
            noiseStream.Select(noise => sourceTick.ObserveNoisePulse(noise.Frequency, noise.Amplitude, noise.Duration, noise.BlendOverLifeTime))
            .ReduceLatestBy(sourceTick, agg, resultCombiner);
        /// <summary>
        /// /////////////////////
        /// </summary>
        /// <param name="noiseInput"></param>
        /// <returns></returns>

        public delegate NoiseOutput NoiseGenerator(NoiseInput noiseInput);

        public static NoiseGenerator BuildGenerator(this INoiseGenerator @this)
        {
            switch (@this)
            {
                case PerlinNoiseConstant pc: return pc.PerlinNoiseConstantFunction();
                case PerlinNoisePulse pp: return pp.PerlinNoisePulseFunction();
                default: throw new Exception("Pattern match not Exhausted");
            }
        }

        static NoiseGenerator PerlinNoisePulseFunction(this PerlinNoisePulse @this)
        {
            float timeRemaining = @this.Duration;
            Vector3 noiseOffset = Vector3Extensions.RandomComponents(new Vector2(0, 32));

            return
                inputs =>
                {
                    timeRemaining -= inputs.Delta;

                    float noiseOffsetDelta = inputs.Delta * @this.Frequency * inputs.FrequencyMultiplier;

                    noiseOffset.x += noiseOffsetDelta;
                    noiseOffset.y += noiseOffsetDelta;
                    noiseOffset.z += noiseOffsetDelta;

                    Vector3 noise = new Vector3();

                    noise.x = Mathf.PerlinNoise(noiseOffset.x, 0.0f);
                    noise.y = Mathf.PerlinNoise(noiseOffset.x, 1.0f);
                    noise.z = Mathf.PerlinNoise(noiseOffset.x, 2.0f);

                    noise -= Vector3.one * 0.5f;

                    noise *= @this.Amplitude * inputs.AmplitudeMultiplier;

                    float agePercent = 1.0f - (timeRemaining / @this.Duration);
                    noise *= @this.BlendOverLifeTime.Evaluate(agePercent);

                    return new NoiseOutput(timeRemaining > 0.0f ? noise.AsOption_UNSAFE() : None.Default);
                };
        }

        static NoiseGenerator PerlinNoiseConstantFunction(this PerlinNoiseConstant @this)
        {
            Vector3 noiseOffset = Vector3Extensions.RandomComponents(new Vector2(0, 32));

            return inputs =>
            {
                float noiseOffsetDelta = inputs.Delta * @this.Frequency * inputs.FrequencyMultiplier;

                noiseOffset.x += noiseOffsetDelta;
                noiseOffset.y += noiseOffsetDelta;
                noiseOffset.z += noiseOffsetDelta;

                float x = Mathf.PerlinNoise(noiseOffset.x, 0.0f);
                float y = Mathf.PerlinNoise(noiseOffset.x, 1.0f);
                float z = Mathf.PerlinNoise(noiseOffset.x, 2.0f);

                Vector3 noise = new Vector3(x, y, z);

                noise -= Vector3.one * 0.5f;
                noise *= @this.Amplitude * inputs.AmplitudeMultiplier;

                return new NoiseOutput(noise.AsOption_UNSAFE());
            };
        }
    }

}
