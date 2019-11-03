using UnityEngine;
using UnityEngine.Events;

public class M_BasicState : A_State
{
    [Header("State Events")]
    [SerializeField] UnityEvent m_onStateEnter = new UnityEvent();
    [SerializeField] UnityEvent m_onStateExit = new UnityEvent();
    [SerializeField] UnityEvent m_executeMain = new UnityEvent();

    public override void OnStateEnter()
    {
        m_onStateEnter.Invoke();
    }

    public override void OnStateExit()
    {
        m_onStateExit.Invoke();
    }
}
