using UnityEngine;

[System.Serializable]
public class FPSCameraSettings
{
    [SerializeField] float m_minViewAngle = -60;
    [SerializeField] float m_maxViewAngle = 60;
    [SerializeField] float m_sensitivity = 500;

    public float MinViewAngle => m_minViewAngle;
    public float MaxViewAngle => m_maxViewAngle;
    public float Sensitivity => m_sensitivity;

    public FPSCameraSettings(float minViewAngle, float maxViewAngle, float sensitivity)
    {
        this.m_minViewAngle = minViewAngle;
        this.m_maxViewAngle = maxViewAngle;
        this.m_sensitivity = sensitivity;
    }
}
