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
    public FPSSignals FPSSignals { get; private set; }

    ReplaySubject<Unit> m_onInitialize;
    Subject<PlayerData> m_playerDataUpdate;

    public IObservable<float> OnUpdate { get; private set; }
    public IObservable<float> OnFixedUpdate { get; private set; }
    public IObservable<PlayerData> PlayerDataUpdate { get; private set; }

    private void Awake()
    {
        m_playerDataUpdate = new Subject<PlayerData>().AddTo(this);
        PlayerDataUpdate = m_playerDataUpdate.AsObservable();

        OnFixedUpdate =
            M_UpdateManager
            .OnFixedUpdate_0
            .Select(_ => Time.fixedDeltaTime);

        OnUpdate =
            M_UpdateManager
            .OnUpdate_0
            .Select(_ => Time.deltaTime);

        OnUpdate
            .Subscribe(tick => m_playerDataUpdate.OnNext(new PlayerData(Character.transform.ExtractData(), FPSCamera.Camera.transform.ExtractData())))
            .AddTo(this);

        FPSSignals = new FPSSignals();

        view.Initialize(this);
        FPSSignals.Initialize(this);
        inputs.Initialize();
    }
}
