using UnityEngine;

public class PerlinNoiseGenerator_Constant : I_NoiseGeneratorVector3
{
    PerlinNoiseData_Constant data;
    Vector3 noiseOffset;

    public PerlinNoiseGenerator_Constant(PerlinNoiseData_Constant data)
    {
        this.data = data;

        float rand = 32.0f;

        noiseOffset.x = Random.Range(0.0f, rand);
        noiseOffset.y = Random.Range(0.0f, rand);
        noiseOffset.z = Random.Range(0.0f, rand);
    }

    public void UpdateNoise(float deltaTime, float ampMult, float freqMult)
    {
        float noiseOffsetDelta = deltaTime * data.Frequency * freqMult;

        noiseOffset.x += noiseOffsetDelta;
        noiseOffset.y += noiseOffsetDelta;
        noiseOffset.z += noiseOffsetDelta;

        float x = Mathf.PerlinNoise(noiseOffset.x, 0.0f);
        float y = Mathf.PerlinNoise(noiseOffset.x, 1.0f);
        float z = Mathf.PerlinNoise(noiseOffset.x, 2.0f);

        Vector3 noise = new Vector3(x, y, z);

        noise -= Vector3.one * 0.5f;
        noise *= data.Amplitude * ampMult;

        Noise = noise;
    }

    public bool IsAlive { get { return true; } set { } }
    public Vector3 Noise { get; private set; }
}
