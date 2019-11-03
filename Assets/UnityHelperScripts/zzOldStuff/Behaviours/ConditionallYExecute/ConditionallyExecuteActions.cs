using UnityEngine;

[System.Serializable]
public class ConditionallyExecuteActions
{
    [Header("Conditional Events")]
    [SerializeField] ConditionalExecutionEventSet[] m_conditionalExecutionEventSets = default;

    public void ExecuteConditions()
    {
        for (int i = 0; i < m_conditionalExecutionEventSets.Length; i++)
        {
            m_conditionalExecutionEventSets[i].ConditionallyExecute();
        }
    }
}
