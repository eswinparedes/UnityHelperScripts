using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_TransformIdentityCustomInputModuleData : A_Component
{
    [SerializeField] SO_CustomInputModuleData m_data = default;
    [SerializeField] SO_A_Bool m_pressedCondition = default;
    [SerializeField] SO_A_Bool m_releasedCondition = default;
    [SerializeField] Transform m_inputRaycastIdentity = default;

    public override void Execute()
    {
        m_data.PressedCondition = m_pressedCondition.IsTrue;
        m_data.ReleasedCondition = m_releasedCondition.IsTrue;
        m_data.InputRay = m_inputRaycastIdentity.GetRayFromTransform();
    }
}
