using UnityEngine;
using UniRx;
using System;
using System.Collections.Generic;

[System.Serializable]
public class PlayerAudio
{
    [SerializeField] AudioClip[] m_footsteps = default;
    [SerializeField] AudioClip m_jump = default;
    [SerializeField] AudioClip m_land = default;
    [SerializeField] AudioSource m_audioSource = default;

    Component m_attachedBehaviour;
    I_CharacterStrideSignals m_strideLogger;
    IObservable<CharacterMovementOutput> m_observablePlayerOutput;

    List<IDisposable> m_subscriptions = new List<IDisposable>();

    public void Initialize(Component attachedBehaviour, IObservable<CharacterMovementOutput> player, I_CharacterStrideSignals strideLogger)
    {
        this.m_attachedBehaviour = attachedBehaviour;
        this.m_strideLogger = strideLogger;
        this.m_observablePlayerOutput = player;
    }

    public void Subscribe()
    {
        var steppedSubscription =
        m_strideLogger.StrideLogged
               .SelectRandom(m_footsteps)
               .Subscribe(clip => m_audioSource.PlayOneShot(clip))
               .AddTo(m_attachedBehaviour);

        var jumpedSubscription =
            m_observablePlayerOutput
            .Where(output => output.OutputControllerState.HasJustLeftGround() && output.DidJump())
            .Subscribe(_ => m_audioSource.PlayOneShot(m_jump))
            .AddTo(m_attachedBehaviour);

        var landedSubscription =
        m_observablePlayerOutput
            .Where(output => output.OutputControllerState.HasJustLanded())
            .Subscribe(_ => m_audioSource.PlayOneShot(m_land))
            .AddTo(m_attachedBehaviour);

        m_subscriptions.Add(steppedSubscription);
        m_subscriptions.Add(jumpedSubscription);
        m_subscriptions.Add(landedSubscription);
    }

    public void Dispose()
    {
        m_subscriptions.ForEach(sub => sub.Dispose());
        m_subscriptions.Clear();
    }

}
