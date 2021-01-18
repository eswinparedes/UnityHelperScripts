﻿using UnityEngine;

namespace SUHScripts
{
    [CreateAssetMenu(menuName = "SUHS/Experimental/Perlin Noise Constant")]
    public class SO_PerlinNoiseData_Constant : SO_A_NoiseData
    {
        [SerializeField] PerlinNoiseConstant m_noiseData = default;

        public override INoiseGenerator Generator => m_noiseData;
    }

}