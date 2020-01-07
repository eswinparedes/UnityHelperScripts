using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StateListener : I_StateBehaviour{

    [SerializeField] SO_StateEvent m_stateEvent = default;
    [SerializeField] UnityEvent m_onStateEnter = new UnityEvent();
    [SerializeField] UnityEvent m_onStateExit = new UnityEvent();
    [SerializeField] UnityEvent m_onExecuteMain = new UnityEvent();

    public UnityEvent StateEnter => m_onStateEnter;
    public UnityEvent StateExit => m_onStateExit;
    public UnityEvent StateExecuteMain => m_onExecuteMain;
    public string StateName => m_stateEvent.name;

    public void OnStateEnter()
    {
        m_onStateEnter.Invoke();
    }

    public void OnStateExit()
    {
        m_onStateExit.Invoke();
    }

    public void ExecuteMain()
    {
        m_onExecuteMain.Invoke();
    }

    public void StartListening() => m_stateEvent.AddStateBehaviour(this);
    public void StopListening() => m_stateEvent.RemoveStateBehaviour(this);

    
}
