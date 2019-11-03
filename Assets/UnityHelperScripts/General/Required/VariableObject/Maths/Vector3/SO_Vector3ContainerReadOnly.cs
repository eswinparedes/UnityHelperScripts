using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Maths/Vector3/Vector3 Container (RO)")]
public class SO_Vector3ContainerReadOnly : SO_A_Vector3ReadOnly
{
    [SerializeField] SO_A_Vector3 m_reference;

    public override Vector3 Value { get => m_reference.Value; set { } }

    public void SetReference(SO_A_Vector3 newValue)
    {
        m_reference = newValue;
    }

}
