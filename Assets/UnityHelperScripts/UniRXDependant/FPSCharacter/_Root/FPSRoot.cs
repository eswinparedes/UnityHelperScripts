using UnityEngine;
using UniRx;
using System;
using UniRx.Triggers;

public class FPSRoot : MonoBehaviour
{
    [SerializeField] A_Inputs inputs = default;
    [SerializeField] CharacterController character = default;
    [SerializeField] FPSCameraRoot view = default;
    [SerializeField] GravityData m_gravityData = default;

    public A_Inputs Inputs => inputs;
    public CharacterController Character => character;
    public FPSCameraRoot FPSCamera => view;
    public GravityData GravityData => m_gravityData;
    public Component AttachBehaviour => character;
    public FPSSignals FPSSignals { get; private set; } = new FPSSignals();
    public TransformData SourceTransformData => character.transform.ExtractData();
    public TransformData SourceViewData => view.Camera.transform.ExtractData();

    ReplaySubject<Unit> m_onInitialize;

    public IObservable<float> OnUpdate { get; private set; }
    public IObservable<float> OnFixedUpdate { get; private set; }

    private void Awake()
    {
        OnFixedUpdate =
            M_UpdateManager
            .OnFixedUpdate_0
            .Select(_ => Time.fixedDeltaTime);

        OnUpdate =
            M_UpdateManager
            .OnUpdate_0
            .Select(_ => Time.deltaTime);

        view.Initialize(this);
        FPSSignals.Initialize(this);
        inputs.Initialize();
    }
}
