using System.Collections.Generic;
using UnityEngine;
using static NoiseGenerationOperations;

[System.Serializable]
[System.Obsolete]
public class TransformShakeBehaviour : I_TransformShakeSubscriber
{
    [SerializeField] Transform m_shakeTransform = null;
    [SerializeField] SO_TransformShakeEvent[] m_eventSubscribers = null;
    [SerializeField] SO_A_Float m_amplitudeMultiplier = null;
    [SerializeField] SO_A_Float m_frequencyMultiplier = null;
    [SerializeField] Vector3 m_positionScale = Vector3.one;
    [SerializeField] Vector3 m_rotationScale = Vector3.one;
    [SerializeField] Vector3 m_scaleScale = Vector3.one;

    Vector3 m_baseScale;
    Vector3 m_baseEulerAngles;
    Vector3 m_baseRotation;

    List<NoiseGenerator> m_positionMovement = new List<NoiseGenerator>();
    List<NoiseGenerator> m_rotationMovement = new List<NoiseGenerator>();
    List<NoiseGenerator> m_scaleMovement    = new List<NoiseGenerator>();

    public void Start()
    {
        m_baseScale = m_shakeTransform.localScale;
        m_baseEulerAngles = m_shakeTransform.localPosition;
        m_baseRotation = m_shakeTransform.localEulerAngles;

        RegisterToEvents();
    }

    public void AddShakeEvent(SO_TransfromShakeData data)
    {
        m_positionMovement.Add(data.PositionData.Generator.BuildGenerator());
        m_rotationMovement.Add(data.RotationData.Generator.BuildGenerator());
        m_scaleMovement.Add(data.ScaleData.Generator.BuildGenerator());
    }

    public void AddShakeEvent(INoiseGenerator data)
    {
        m_positionMovement.Add(data.BuildGenerator());
        m_rotationMovement.Add(data.BuildGenerator());
        m_scaleMovement.Add(data.BuildGenerator());
    }

    public void AddShakeEvent(SO_A_NoiseData data)
    {
        var gen = data.Generator.BuildGenerator();
        m_positionMovement.Add(gen);
        m_rotationMovement.Add(gen);
        m_scaleMovement.Add(gen);
    }

    public void Execute(float deltaTime)
    {
        var input = new NoiseInput(deltaTime, m_amplitudeMultiplier.Value, m_frequencyMultiplier.Value);
        Vector3 positionMovement = m_positionMovement.EvaluateAllWithKill(input);
        Vector3 rotationMovement = m_rotationMovement.EvaluateAllWithKill(input);
        Vector3 scaleMovement = m_scaleMovement.EvaluateAllWithKill(input);

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
