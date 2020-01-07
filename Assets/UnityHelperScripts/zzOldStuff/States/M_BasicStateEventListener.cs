using UnityEngine;
using UnityEngine.Events;

public class M_BasicStateEventListener : A_StateEventListener
{
    [SerializeField] UnityEvent m_onStateEnterEvent = null;
    [SerializeField] UnityEvent m_onStateExitEvent = null;
    [SerializeField] UnityEvent m_onExecuteMainEvent = null;

    public override void ExecuteMain()
    {
        m_onExecuteMainEvent.Invoke();
    }

    public override void OnStateEnter()
    {
        m_onStateEnterEvent.Invoke();
    }

    public override void OnStateExit()
    {
        m_onStateExitEvent.Invoke();
    }
}
