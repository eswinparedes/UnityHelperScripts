using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Experimental/State Event")]
public class SO_StateEvent : ScriptableObject
{
    List<I_StateBehaviour> m_stateListeners = new List<I_StateBehaviour>();

    
    public void RaiseOnStateEnter()
    {
        
        for (int i = m_stateListeners.Count - 1; i >= 0; i--)
        {
            m_stateListeners[i].OnStateEnter();
            CustomDebug.DebugLogState(() => Debug.Log($"{name} Raising State Enter on {m_stateListeners[i].StateName}"));
        }
    }

    public void RaiseOnStateExit()
    {
        
        for (int i = m_stateListeners.Count - 1; i >= 0; i--)
        {
            m_stateListeners[i].OnStateExit();
            CustomDebug.DebugLogState(() => Debug.Log($"{name} Raising State Exit on {m_stateListeners[i].StateName}"));
        }
    }

    public void AddStateBehaviour(I_StateBehaviour listener)
    {
        if (!m_stateListeners.Contains(listener))
        {
            m_stateListeners.Add(listener);
        }
    }

    public void RemoveStateBehaviour(I_StateBehaviour listener)
    {
        if (m_stateListeners.Contains(listener))
        {
            m_stateListeners.Remove(listener);
        }
    }

    public void Clear()
    {
        m_stateListeners.Clear();
    }
}
