using UnityEngine;
using System;
using UnityEngine.Events;

[System.Serializable]
public class TriggerPull
{
    [Header("Trigger Pull Settings")]
    [SerializeField] UnityEvent m_onFireEvent = new UnityEvent();
    [SerializeField] float m_shotsPerSecond = 1;
    [SerializeField] bool m_isSemiAutomatic = true;

    C_Timer m_fireRateTimer = new C_Timer();
    Action m_activeTriggerBehaviour = () => { };
    Action<float> m_updateBehaviour = x => { };
    bool m_isReleased = true;

    public UnityEvent OnFireEvent => m_onFireEvent;
    
    //Add Minimum Hold Time
    //Add Max HoldTime
    //Add Output Alpha

    public void Start()
    {
        m_fireRateTimer.SetTimerLength(1 / m_shotsPerSecond);
        m_activeTriggerBehaviour = Fire;
        m_updateBehaviour = 
            (deltaTime) => m_fireRateTimer.UpdateTimer(deltaTime);
    }

    public void TriggerDown() => 
        m_activeTriggerBehaviour();

    public void TriggerHold() => 
        m_activeTriggerBehaviour();

    public void TriggerRelease() => 
        m_isReleased = true;

    public void Update(float deltaTime) =>
        m_updateBehaviour(deltaTime);

    void FireSemi()
    {
        if (m_isReleased)
        {
            Fire();
        }
    }

    public void Fire()
    {
        m_activeTriggerBehaviour = () => { };

        m_fireRateTimer.OnTimerComplete = ResetFire;
        m_fireRateTimer.Restart();

        m_isReleased = false;

        m_onFireEvent.Invoke();
        m_updateBehaviour = 
            (deltaTime) => m_fireRateTimer.UpdateTimer(deltaTime);
    }

    void ResetFire()
    {
        m_updateBehaviour = (x) => { };
        m_activeTriggerBehaviour = m_isSemiAutomatic ? (Action)FireSemi : Fire;
    }
}

