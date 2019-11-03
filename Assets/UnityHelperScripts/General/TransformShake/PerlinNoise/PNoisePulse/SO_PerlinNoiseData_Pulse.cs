using UnityEngine;

[CreateAssetMenu(menuName = "Experimental/Perlin Noise Pulse")]
public class SO_PerlinNoiseData_Pulse : SO_A_NoiseData
{
    [SerializeField] PerlinNoiseData_Pulse m_noiseData;

    public override I_NoiseGeneratorVector3 GetGenerator() =>
        m_noiseData.GetGenerator();
}
