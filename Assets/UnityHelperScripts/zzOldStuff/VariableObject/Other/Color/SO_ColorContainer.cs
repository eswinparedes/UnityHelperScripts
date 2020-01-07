using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Other/Color/Color Container")]
public class SO_ColorContainer : SO_A_Color
{
    [Header("GETTER ONLY")]
    [SerializeField] SO_A_Color m_colorReference = default;

    public override Color color { get { return m_colorReference.color; } set { } }
}
