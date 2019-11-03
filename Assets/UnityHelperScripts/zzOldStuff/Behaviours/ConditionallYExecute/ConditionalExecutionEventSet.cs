using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct ConditionalExecutionEventSet
{
    [SerializeField] SO_A_Bool m_executionCondition;
    [SerializeField] UnityEvent m_onConditionMetEvent;

    public ConditionalExecutionEventSet(SO_A_Bool executeCondition, UnityEvent conditionMetEvent)
    {
        this.m_executionCondition = executeCondition;
        this.m_onConditionMetEvent = conditionMetEvent;
    }
    public void ConditionallyExecute()
    {
        if (m_executionCondition.IsTrue)
        {
            m_onConditionMetEvent.Invoke();
        }
    }
}