using UnityEngine;

namespace SUHScripts
{
    [CreateAssetMenu(menuName = "SUHS/Experimental/Perlin Noise Pulse")]
    public class SO_PerlinNoiseData_Pulse : SO_A_NoiseData
    {
        [SerializeField] PerlinNoisePulse m_noiseData = default;

        public override INoiseGenerator Generator => m_noiseData;
    }
}

