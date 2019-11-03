using UnityEngine;

[System.Serializable]
public struct PerlinNoiseData_Constant : I_NoiseGeneratorData
{
    [SerializeField] float frequency;
    [SerializeField] float amplitude;

    public PerlinNoiseData_Constant(float freq, float amp)
    {
        frequency = freq;
        amplitude = amp;
    }

    public I_NoiseGeneratorVector3 GetGenerator() =>
        new PerlinNoiseGenerator_Constant(this);

    public float Frequency => frequency;
    public float Amplitude => amplitude;
}
