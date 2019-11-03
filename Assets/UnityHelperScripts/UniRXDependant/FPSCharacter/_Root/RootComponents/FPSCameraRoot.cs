using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

[Serializable]
public class FPSCameraRoot
{
    [SerializeField] Camera m_camera = default;
    [SerializeField] Transform m_cameraRoot = default;
    [SerializeField] float m_zoomTime = .5f;
    [SerializeField] FPSCameraSettings m_cameraSettings = new FPSCameraSettings(-60, 60, 350);

    public Transform RootTransform => m_cameraRoot;
    public float DefaultFOV => m_sourceFOV;
    public FPSCameraSettings FPSCameraSettings => m_cameraSettings;
    public Camera Camera => m_camera;

    Dictionary<object, Vector3> m_offsets = new Dictionary<object, Vector3>();

    Vector3 m_sourceLocalPosition;
    Vector3 m_offset = Vector3.zero;

    FTimer m_zoomTimer;
    float m_sourceFOV = 60;
    float m_lastFOV = 60;
    float m_targetFOV = 60;

    FPSRoot m_fpsRoot;

    IDisposable m_onUpdateSubscription;

    public void Initialize(FPSRoot root)
    {
        m_fpsRoot = root;

        m_onUpdateSubscription = Disposable.Empty;

        m_sourceLocalPosition = m_cameraRoot.localPosition;
        m_sourceFOV = m_camera.fieldOfView;

        m_onUpdateSubscription =
        m_fpsRoot
            .OnUpdate
            .Where(_ => !m_zoomTimer.HasCompleted())
            .Subscribe(deltaTime =>
            {
                m_camera.fieldOfView = Mathf.Lerp(m_lastFOV, m_targetFOV, m_zoomTimer.TimeAlpha());
                m_zoomTimer = m_zoomTimer.Tick(deltaTime);
            })
            .AddTo(m_fpsRoot.AttachBehaviour);
    }

    public void RequestOffset(object requestee, Vector3 offset)
    {
        if (m_offsets.ContainsKey(requestee))
        {
            m_offsets[requestee] = offset;
        }
    }

    public void RequestAddOffsetProvider(object provider)
    {
        if (!m_offsets.ContainsKey(provider))
            m_offsets.Add(provider, Vector3.zero);
    }

    public void RequestRemoveOffsetProvider(object provider)
    {
        if (m_offsets.ContainsKey(provider))
            m_offsets.Remove(provider);
    }

    public void ApplyOffset()
    {
        var offset = Vector3.zero;

        foreach(var kvp in m_offsets)
            offset += kvp.Value;

        m_cameraRoot.localPosition = m_sourceLocalPosition + offset;
    }

    public void RequestSetFOV(float fov, float? time = null)
    {
        m_lastFOV = m_camera.fieldOfView;
        m_targetFOV = fov;
        m_zoomTimer = new FTimer(time?? m_zoomTime, 0);
    }

    public void RequestResetFOV()
    {
        RequestSetFOV(m_sourceFOV);
    }
}
