using SUHScripts.Functional;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NoiseInput
{
    public NoiseInput(float delta, float amplitudeMultiplier, float frequencyMultiplier) : this()
    {
        Delta = delta;
        AmplitudeMultiplier = amplitudeMultiplier;
        FrequencyMultiplier = frequencyMultiplier;
    }

    public float Delta { get; }
    public float AmplitudeMultiplier { get; }
    public float FrequencyMultiplier { get; }
}
public struct NoiseOutput 
{
    public NoiseOutput(Option<Vector3> noise)
    {
        Noise = noise;
    }

    public Option<Vector3> Noise { get; }
}

public interface INoiseGenerator { }

[System.Serializable]
public class PerlinNoiseData_Pulse : INoiseGenerator
{
    [SerializeField] float frequency;
    [SerializeField] float amplitude;
    [SerializeField] float duration;
    [SerializeField] AnimationCurve blendOverLifeTime;

    public PerlinNoiseData_Pulse(float freq, float amp, float duration, AnimationCurve curve)
    {
        this.frequency = freq;
        this.amplitude = amp;
        this.duration = duration;
        this.blendOverLifeTime = curve;
    }

    public float Duration => duration;
    public float Frequency => frequency;
    public float Amplitude => amplitude;
    public AnimationCurve BlendOverLifeTime => blendOverLifeTime;
}

[System.Serializable]
public class PerlinNoiseData_Constant : INoiseGenerator
{
    [SerializeField] float frequency;
    [SerializeField] float amplitude;

    public PerlinNoiseData_Constant(float freq, float amp)
    {
        frequency = freq;
        amplitude = amp;
    }

    public float Frequency => frequency;
    public float Amplitude => amplitude;
}

[System.Serializable]
public class TransformShakeSubscriber
{
    [SerializeField] float m_amplitudeMultiplier = 1;
    [SerializeField] float m_frequencyMultiplier = 1;
    [SerializeField] Vector3 m_positionScale = Vector3.one;
    [SerializeField] Vector3 m_rotationScale = Vector3.one;
    [SerializeField] Vector3 m_scaleScale = Vector3.one;

    public float AmplitudeMultiplier => m_amplitudeMultiplier;
    public float FrequencyMultiplier => m_frequencyMultiplier;
    public Vector3 PositionScale => m_positionScale;
    public Vector3 RotationScale => m_rotationScale;
    public Vector3 ScaleScale => m_scaleScale;

}