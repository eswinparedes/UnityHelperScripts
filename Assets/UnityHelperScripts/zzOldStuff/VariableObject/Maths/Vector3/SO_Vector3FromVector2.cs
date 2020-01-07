using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Maths/Vector3/Vector3 from Vector2 (RO)")]
public class SO_Vector3FromVector2 : SO_A_Vector3ReadOnly
{
    [SerializeField] SO_A_Vector2 m_vector2 = default;
    [SerializeField] float m_zValue = 0;

    Vector3 m_value;

    public float ZValue { get => m_zValue; set => m_zValue = value; }

    public override Vector3 Value
    {
        get
        {
            m_value.x = m_vector2.Value.x;
            m_value.y = m_vector2.Value.y;
            m_value.z = m_zValue;
            return m_value;
        }
        set { }
    }
}
