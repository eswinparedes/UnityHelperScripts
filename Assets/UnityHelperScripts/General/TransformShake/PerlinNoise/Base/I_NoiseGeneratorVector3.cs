using UnityEngine;

public interface I_NoiseGeneratorVector3 
{
    void UpdateNoise(float deltaTime, float ampMult, float freqMult);
    bool IsAlive { get; }
    Vector3 Noise { get; }
}


