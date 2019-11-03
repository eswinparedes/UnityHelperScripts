using UnityEngine;

public class M_BasicStateHub : MonoBehaviour
{
    [SerializeField] A_State m_initialState = default;

    C_StateMachine m_stateMachine = new C_StateMachine();

    string m_currentState = "";

    private void Start()
    {
        m_stateMachine.StartStateMachine(m_initialState);
        m_currentState = m_initialState.StateName;
    }

    public void TransitionState(A_State state)
    {
        m_stateMachine.TransitionStateTo(state);
        m_currentState = state.StateName;
    }
    public void StartStateMachine()
    {
        m_stateMachine.StartStateMachine(m_initialState);
    }
    public void StartStateMachine(A_State state)
    {
        m_stateMachine.StartStateMachine(state);
    }
    public void StopStateMachine()
    {
        m_stateMachine.StopStateMachine();
    }
}
