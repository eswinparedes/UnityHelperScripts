using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Maths/Vector2/Vector2")]
public class SO_Vector2 : SO_A_Vector2ReadWrite {

    [SerializeField] Vector2 m_value;

    public override Vector2 Value
    {
        get => m_value;
        set => m_value = value;
    }
}
