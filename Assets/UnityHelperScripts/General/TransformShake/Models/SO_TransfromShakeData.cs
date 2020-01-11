using UnityEngine;

namespace SUHScripts
{
    [CreateAssetMenu(menuName = "SUHS/Experimental/Transform Shake Data")]
    public class SO_TransfromShakeData : ScriptableObject
    {
        [SerializeField] SO_A_NoiseData m_positionData = default;
        [SerializeField] SO_A_NoiseData m_rotationData = default;
        [SerializeField] SO_A_NoiseData m_scaleData = default;

        public SO_A_NoiseData PositionData => m_positionData;
        public SO_A_NoiseData RotationData => m_rotationData;
        public SO_A_NoiseData ScaleData => m_scaleData;
    }
}