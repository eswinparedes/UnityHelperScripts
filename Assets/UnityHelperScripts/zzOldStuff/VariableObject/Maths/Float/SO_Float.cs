using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Maths/Float/Float")]
public class SO_Float : SO_A_FloatReadWrite
{
    [SerializeField] float m_value;

    public override float Value
    {
        get => m_value;
        set => m_value = value;
    }
}