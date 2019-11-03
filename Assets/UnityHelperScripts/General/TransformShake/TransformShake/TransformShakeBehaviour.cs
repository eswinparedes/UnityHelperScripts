using UnityEngine;

[System.Serializable]
public class TransformShakeBehaviour : I_TransformShakeSubscriber
{
    [SerializeField] Transform m_shakeTransform = null;
    [SerializeField] SO_A_NoiseData[] m_startData = null;
    [SerializeField] SO_TransfromShakeData[] m_startShakeData = null;
    [SerializeField] SO_TransformShakeEvent[] m_eventSubscribers = null;
    [SerializeField] SO_A_Float m_amplitudeMultiplier = null;
    [SerializeField] SO_A_Float m_frequencyMultiplier = null;
    [SerializeField] Vector3 m_positionScale = Vector3.one;
    [SerializeField] Vector3 m_rotationScale = Vector3.one;
    [SerializeField] Vector3 m_scaleScale = Vector3.one;

    Vector3 m_baseScale;
    Vector3 m_baseEulerAngles;
    Vector3 m_baseRotation;

    PerlinNoiseBehaviour m_positionMovement = new PerlinNoiseBehaviour();
    PerlinNoiseBehaviour m_rotationMovement = new PerlinNoiseBehaviour();
    PerlinNoiseBehaviour m_scaleMovement = new PerlinNoiseBehaviour();

    public void Start()
    {
        m_baseScale = m_shakeTransform.localScale;
        m_baseEulerAngles = m_shakeTransform.localPosition;
        m_baseRotation = m_shakeTransform.localEulerAngles;

        for (int i = 0; i < m_startData.Length; i++)
        {
            AddShakeEvent(m_startData[i]);
        }

        for(int i = 0; i < m_startShakeData.Length; i++)
        {
            AddShakeEvent(m_startShakeData[i]);
        }

        RegisterToEvents();
    }

    public void AddShakeEvent(SO_TransfromShakeData data)
    {
        m_positionMovement.AddNoiseGenerator(data.PositionData);
        m_rotationMovement.AddNoiseGenerator(data.RotationData);
        m_scaleMovement.AddNoiseGenerator(data.ScaleData);
    }

    public void AddShakeEvent(I_NoiseGeneratorData data)
    {
        m_positionMovement.AddNoiseGenerator(data);
        m_rotationMovement.AddNoiseGenerator(data);
        m_scaleMovement.AddNoiseGenerator(data);
    }

    public void AddShakeEvent(SO_A_NoiseData data)
    {
        m_positionMovement.AddNoiseGenerator(data);
        m_rotationMovement.AddNoiseGenerator(data);
        m_scaleMovement.AddNoiseGenerator(data);
    }

    public void Execute(float deltaTime)
    {
        Vector3 positionMovement = m_positionMovement.UpdateNoise(deltaTime, m_amplitudeMultiplier.Value, m_frequencyMultiplier.Value);
        Vector3 rotationMovement = m_rotationMovement.UpdateNoise(deltaTime, m_amplitudeMultiplier.Value, m_frequencyMultiplier.Value);
        Vector3 scaleMovement = m_scaleMovement.UpdateNoise(deltaTime, m_amplitudeMultiplier.Value, m_frequencyMultiplier.Value);

        m_shakeTransform.localPosition = m_baseEulerAngles + Vector3.Scale(m_positionScale, positionMovement);
        m_shakeTransform.localEulerAngles = m_baseRotation + Vector3.Scale(m_rotationScale, rotationMovement);
        m_shakeTransform.localScale = m_baseScale + Vector3.Scale(m_scaleScale, scaleMovement);
    }

    public void RegisterToEvents()
    {
        for (int i = 0; i < m_eventSubscribers.Length; i++)
        {
            m_eventSubscribers[i].AddTransformShake(this);
        }
    }

    public void UnRegisterFromEvents()
    {
        for (int i = 0; i < m_eventSubscribers.Length; i++)
        {
            m_eventSubscribers[i].RemoveTransformShake(this);
        }
    }
}
