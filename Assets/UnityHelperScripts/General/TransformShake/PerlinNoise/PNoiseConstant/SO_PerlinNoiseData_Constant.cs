using UnityEngine;

[CreateAssetMenu(menuName = "Experimental/Perlin Noise Constant")]
public class SO_PerlinNoiseData_Constant : SO_A_NoiseData
{
    [SerializeField] PerlinNoiseData_Constant m_noiseData;

    public override I_NoiseGeneratorVector3 GetGenerator() =>
        m_noiseData.GetGenerator();
}
