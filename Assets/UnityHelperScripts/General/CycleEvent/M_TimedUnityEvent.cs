using UnityEngine;
using UnityEngine.Events;

public class M_TimedUnityEvent : A_Component
{
    [Header("Timing Settings")]
    [SerializeField] SO_A_Float m_time = default;
    [SerializeField] UnityEvent m_onTimerCompleteEvent = new UnityEvent();
    [SerializeField] bool m_doesLoop = false;

    C_Timer m_timer;

    private void Awake()
    {
        m_timer = new C_Timer();
        m_timer.SetTimerLength(m_time.Value);
        m_timer.OnTimerComplete = () =>
        {
            m_onTimerCompleteEvent.Invoke();
            if (m_doesLoop)
            {
                m_timer.SetTimerLength(m_time.Value);
            }
            else
            {
                m_componentManager.RemoveComponent(this);
            }
        };
    }
    public override void Execute()
    {
        m_timer.UpdateTimer(m_componentManager.DeltaTime);
    }

    public void Restart()
    {
        m_componentManager.AddComponent(this);
        m_timer.SetTimerLength(m_time.Value);
    }
}
