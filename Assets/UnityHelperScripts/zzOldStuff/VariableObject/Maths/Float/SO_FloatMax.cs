using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Maths/Float/Float Max (RO)")]
public class SO_A_FloatMax : SO_A_FloatReadOnly {

    [SerializeField] SO_A_Float a = null;
    [SerializeField] SO_A_Float b = null;

    public override float Value {
        get
        {
            return a.Value > b.Value ? a.Value : b.Value;
        }
        set { }
    }
}
