using UnityEngine;

public class M_ConditionalExecuteUnityEvent : A_Component
{
    [SerializeField] ConditionallyExecuteActions m_actions = default;

    public override void Execute()
    {
        m_actions.ExecuteConditions();
    }
}