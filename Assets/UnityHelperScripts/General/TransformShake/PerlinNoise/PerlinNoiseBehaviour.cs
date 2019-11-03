using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PerlinNoiseBehaviour 
{
    List<I_NoiseGeneratorVector3> m_noiseGenerators = new List<I_NoiseGeneratorVector3>();

    public void AddNoiseGenerator(I_NoiseGeneratorData data)
    {
        I_NoiseGeneratorVector3 gen = data.GetGenerator();
        m_noiseGenerators.Add(gen);
    }

    public Vector3 UpdateNoise(float deltaTime, float ampMult, float freqMult)
    {
        Vector3 positionOffset = Vector3.zero;

        for (int i = 0; i < m_noiseGenerators.Count; i++)
        {
            I_NoiseGeneratorVector3 se = m_noiseGenerators[i];
            se.UpdateNoise(deltaTime, ampMult, freqMult);
            positionOffset += se.Noise;

            if (!se.IsAlive)
            {
                m_noiseGenerators.RemoveAt(i);
            }
        }

        return positionOffset;
    }
}
