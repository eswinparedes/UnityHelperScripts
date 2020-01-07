using SUHScripts.Functional;
using System;
using System.Collections.Generic;
using UnityEngine;
using static SUHScripts.Functional.Functional;

namespace SUHScripts
{
    public static class NoiseGenerationOperations
    {
        public delegate NoiseOutput NoiseGenerator(NoiseInput noiseInput);

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

        public static NoiseGenerator BuildGenerator(this INoiseGenerator @this)
        {
            switch (@this)
            {
                case PerlinNoiseData_Constant pc: return pc.PerlinNoiseConstantFunction();
                case PerlinNoiseData_Pulse pp: return pp.PerlinNoisePulseFunction();
                default: throw new Exception("Pattern match not Exhausted");
            }
        }

        static NoiseGenerator PerlinNoisePulseFunction
            (this PerlinNoiseData_Pulse @this)
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

                    return new NoiseOutput(timeRemaining > 0.0f ? noise.AsOption() : NONE);
                };
        }

        static NoiseGenerator PerlinNoiseConstantFunction(this PerlinNoiseData_Constant @this)
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

                return new NoiseOutput(noise);
            };
        }
    }

}
