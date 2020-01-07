using UnityEngine;
using UnityEngine.Events;

public class M_BasicStateEvent : A_State
{
    [Header("State Events")]
    [SerializeField] SO_StateEvent m_stateEvent = default;
    [SerializeField] UnityEvent m_onStateEnter = new UnityEvent();
    [SerializeField] UnityEvent m_onStateExit = new UnityEvent();
    [SerializeField] UnityEvent m_executeMain = new UnityEvent();


    public override void OnStateEnter()
    {
        m_onStateEnter.Invoke();
        m_stateEvent.RaiseOnStateEnter();
    }

    public override void OnStateExit()
    {
        m_onStateExit.Invoke();
        m_stateEvent.RaiseOnStateExit();
    }
}