using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Maths/Integer/Integer")]
public class SO_Int : SO_A_IntReadWrite
{
    [SerializeField] int m_value;

    public override int Value
    {
        get => m_value;
        set => m_value = Value;
    }
}
