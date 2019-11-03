using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Maths/Vector3/Vector3")]
public class SO_Vector3 : SO_A_Vector3ReadWrite
{
    [SerializeField] public Vector3 m_value;

    public override Vector3 Value
    {
        get => m_value;
        set => m_value = value;
    }
}