using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Variables/Other/Bool/BoolGroup")]
public class SO_BoolGroup : SO_A_BoolReadOnly
{
    List<I_Bool> m_conditions = new List<I_Bool>();

    public override bool IsTrue { get => GetAllConditionsMet(); set { } }

    bool GetAllConditionsMet()
    {
        return !m_conditions.Any(x => !x.IsTrue);
    }
    public void AddCondition(I_Bool condition)
    {
        if (!m_conditions.Contains(condition))
        {
            m_conditions.Add(condition);
        }
    }

    public void RemoveCondition(I_Bool condition)
    {
        if (m_conditions.Contains(condition))
        {
            m_conditions.Remove(condition);
        }
    }
}
