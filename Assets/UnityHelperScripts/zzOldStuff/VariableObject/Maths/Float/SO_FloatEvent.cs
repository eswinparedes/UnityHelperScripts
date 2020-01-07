using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Variables/Maths/Float/Float Event")]
public class SO_FloatEvent : SO_A_FloatReadWrite
{
    [SerializeField] float m_value;

    public event Action<float> OnValueSet;

    public override float Value
    {
        get
        {
            return m_value;
        }
        set
        {
            m_value = value;
            OnValueSet?.Invoke(value);
        }
    }
}
