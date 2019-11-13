using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public class FPSBasicGroundedSignals
{
    [Header("Signal Receivers")]
    [SerializeField] StrideLogger m_strideLogger = default;
    [SerializeField] CameraBob m_cameraBob = default;
    [SerializeField] PlayerAudio m_playerAudio = default;

    public void Initialize(Component attachedComponent, FPSRoot components)
    {
        m_strideLogger.Initialize(attachedComponent, components.FPSSignals.SourceProvider);

        m_cameraBob.Initialize(attachedComponent, components, m_strideLogger);

        m_playerAudio.Initialize(attachedComponent, components.FPSSignals.SourceProvider, m_strideLogger);
    }

    public void Subscribe()
    {
        m_strideLogger.Subscribe();

        m_cameraBob.Subscribe();
        m_playerAudio.Subscribe();
    }

    public void Dispose()
    {
        m_strideLogger.Dispose();

        m_cameraBob.Dispose();
        m_playerAudio.Dispose();
    }
}
