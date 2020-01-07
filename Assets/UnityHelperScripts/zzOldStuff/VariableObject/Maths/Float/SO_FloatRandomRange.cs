using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Maths/Float/Float Random Range (RO)")]
public class SO_FloatRandomRange : SO_A_FloatReadOnly {

    [Header("[READ ONLY] Range for random values")]
    [SerializeField] Vector2 m_floatMinMax = Vector2.zero;
    public override float Value
    {
        get
        {
            return Random.Range(m_floatMinMax.x, m_floatMinMax.y);
        }

        set
        {

        }
    }
}
