using UnityEngine;

[System.Serializable]
public struct PerlinNoiseData_Pulse : I_NoiseGeneratorData
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
    
    public I_NoiseGeneratorVector3 GetGenerator() =>
        new PerlinNoiseGenerator_Pulse(this);
}
