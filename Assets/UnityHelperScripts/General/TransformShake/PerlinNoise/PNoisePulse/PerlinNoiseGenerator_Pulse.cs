using UnityEngine;

public class PerlinNoiseGenerator_Pulse : I_NoiseGeneratorVector3
{
    float duration;
    float timeRemaining;
    PerlinNoiseData_Pulse data;

    Vector3 noiseOffset;

    public PerlinNoiseGenerator_Pulse(PerlinNoiseData_Pulse data)
    {
        this.data = data;
        duration = data.Duration;
        timeRemaining = duration;

        float rand = 32.0f;

        noiseOffset.x = Random.Range(0.0f, rand);
        noiseOffset.y = Random.Range(0.0f, rand);
        noiseOffset.z = Random.Range(0.0f, rand);
    }

    public void UpdateNoise(float deltaTime, float ampMult, float freqMult)
    {
        timeRemaining -= deltaTime;

        float noiseOffsetDelta = deltaTime * data.Frequency * freqMult;

        noiseOffset.x += noiseOffsetDelta;
        noiseOffset.y += noiseOffsetDelta;
        noiseOffset.z += noiseOffsetDelta;

        Vector3 noise = new Vector3();

        noise.x = Mathf.PerlinNoise(noiseOffset.x, 0.0f);
        noise.y = Mathf.PerlinNoise(noiseOffset.x, 1.0f);
        noise.z = Mathf.PerlinNoise(noiseOffset.x, 2.0f);

        noise -= Vector3.one * 0.5f;

        noise *= data.Amplitude * ampMult;

        float agePercent = 1.0f - (timeRemaining / duration);
        noise *= data.BlendOverLifeTime.Evaluate(agePercent);

        Noise = noise;
    }

    public bool IsAlive { get { return timeRemaining > 0.0f; } }
    public Vector3 Noise { get; private set; }
}
