using UnityEngine;
using System;
using UnityEngine.Events;

[System.Serializable]
public class TransitionScale
{
    [Header("Transition Settings")]
    [SerializeField] Transform m_transitionTransform = default;
    [SerializeField] SO_TransitionScaleSettings m_settings = default;
    [SerializeField] bool m_startExited = true;
    [SerializeField] bool m_eventsFireOnFullTransitionOnly = false;
    [Header("Events")]
    [SerializeField] UnityEvent m_onEnterStart = default;
    [SerializeField] UnityEvent m_onEnterEnd = default;
    [SerializeField] UnityEvent m_onExitStart = default;
    [SerializeField] UnityEvent m_onExitEnd = default;

    public UnityEvent OnEnterStart => m_onEnterStart;
    public UnityEvent OnEnterEnd => m_onEnterEnd;
    public UnityEvent OnExitStart => m_onExitStart;
    public UnityEvent OnExitEnd => m_onExitEnd;

    Vector3 m_lerpEnter;
    Vector3 m_lerpExit;

    FTimer m_timer;
    Action OnTimerComplete = () => { };

    bool m_isEntered;

    public void Start()
    {
        m_lerpEnter = Vector3.Scale(m_transitionTransform.localScale, m_settings.ScaleEnter);
        m_lerpExit = Vector3.Scale(m_transitionTransform.localScale, m_settings.ScaleExit);

        m_timer = new FTimer(m_settings.TransitionTime, m_settings.TransitionTime, !m_startExited);
        m_isEntered = !m_startExited;
    }

    public void Update(float deltaTime)
    {
        m_timer = m_timer.Tick(deltaTime, onComplete: OnTimerComplete.Invoke);
        m_transitionTransform.localScale = Vector3.Lerp(m_lerpExit, m_lerpEnter, m_settings.MovementCurve.Evaluate(m_timer.TimeAlpha()));
    }

    public void Enter()
    {
        OnEnterStarted();

        m_timer = m_timer.RestartedIncrementing(true);
        OnTimerComplete = OnEnterEnded;
    }

    public void Exit()
    {
        OnExitStarted();

        m_timer = m_timer.RestartedDecrementing(true);
        OnTimerComplete = OnExitEnded;
    }

    #region events
    void OnEnterStarted()
    {
        if (m_eventsFireOnFullTransitionOnly && !m_isEntered || !m_eventsFireOnFullTransitionOnly)
        {
            OnEnterStart.Invoke();
        }
    }

    void OnEnterEnded()
    {
        if (m_eventsFireOnFullTransitionOnly && !m_isEntered || !m_eventsFireOnFullTransitionOnly)
        {
            m_isEntered = true;
            OnEnterEnd.Invoke();
        }
    }

    void OnExitStarted()
    {
        if (m_eventsFireOnFullTransitionOnly && m_isEntered || !m_eventsFireOnFullTransitionOnly)
        {
            OnExitStart.Invoke();
        }
    }

    void OnExitEnded()
    {
        if (m_eventsFireOnFullTransitionOnly && m_isEntered || !m_eventsFireOnFullTransitionOnly)
        {
            m_isEntered = false;

            OnExitEnd.Invoke();
        }
    }
    #endregion
}
