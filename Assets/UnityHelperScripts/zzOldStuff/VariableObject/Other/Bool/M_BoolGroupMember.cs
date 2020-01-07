using UnityEngine;

public class M_BoolGroupMember : MonoBehaviour, I_Bool
{
    [SerializeField] SO_BoolGroup m_conditionGroup = default;

    public virtual void OnEnable()
    {
        m_conditionGroup.AddCondition(this);
    }

    public virtual  void OnDisable()
    {
        m_conditionGroup.RemoveCondition(this);
    }
    public bool IsTrue { get; set; }

    public virtual void SetIsTrue(bool isTrue)
    {
        IsTrue = isTrue;
    }
}
