using UnityEngine;

[System.Serializable]
public struct NoiseConstant3D
{
    [SerializeField] float m_frequency;
    [SerializeField] float m_amplitude;
    [SerializeField] Vector3 m_runningOffset;

    public NoiseConstant3D(float frequency, float amplitude, Vector3 runningOffset)
    {
        m_frequency = frequency;
        m_amplitude = amplitude;
        m_runningOffset = runningOffset;
        CurrentValue = Vector3.zero;
    }

    public Vector3 CurrentValue { get; private set; }
    public Vector3 Update(float delta)
    {
        float noiseOffsetDelta = delta * m_frequency;

        m_runningOffset.x += noiseOffsetDelta;
        m_runningOffset.y += noiseOffsetDelta;
        m_runningOffset.z += noiseOffsetDelta;

        float x = Mathf.PerlinNoise(m_runningOffset.x, 0.0f);
        float y = Mathf.PerlinNoise(m_runningOffset.y, 1.0f);
        float z = Mathf.PerlinNoise(m_runningOffset.z, 2.0f);

        CurrentValue = new Vector3(x, y, z);

        CurrentValue -= Vector3.one * 0.5f;
        CurrentValue *= m_amplitude;

        return CurrentValue;
    }
}
