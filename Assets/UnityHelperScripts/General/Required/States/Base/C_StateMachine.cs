using UnityEngine;

//TODO: PRogram to interface
public class C_StateMachine {

    I_StateBehaviour currentState;
    bool m_isActive = false;
    public bool IsActive { get { return m_isActive; } }

    public void StartStateMachine(I_StateBehaviour initialState)
    {
        currentState = initialState;
        currentState.OnStateEnter();
        m_isActive = true;
    }

    public void TransitionStateTo(I_StateBehaviour nextState)
    {
        if(currentState == null)
        {
            Debug.Log("no state");
            StartStateMachine(nextState);
        }
        else if (nextState.StateName != currentState.StateName)
        {
            CustomDebug.DebugLogState(() => Debug.Log("Transitioning from " + currentState.StateName + " to " + nextState.StateName));
            currentState.OnStateExit();
            currentState = nextState;
            currentState.OnStateEnter();
        }
        else
        {
            CustomDebug.DebugLogState(() => Debug.LogWarning("Warning, state already in state: " + nextState.StateName));
        }
    }

    public void StopStateMachine()
    {
        currentState.OnStateExit();
        m_isActive = false;
    }

    public string StateCurrent
    {
        get { return currentState.StateName; }
    }
}
