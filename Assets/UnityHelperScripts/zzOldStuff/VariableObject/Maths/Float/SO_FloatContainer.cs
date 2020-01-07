using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Maths/Float/Float Container (RO)")]
public class SO_FloatContainer : SO_A_FloatReadOnly {

    [Header("READ ONLY")]
    public  SO_A_Float m_floatObject;

	public override float Value
    {
        get { return m_floatObject.Value; }
        set { }
    }
}
