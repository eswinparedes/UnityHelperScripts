using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Other/Bool/Bool Container (RO)")]
public class SO_BoolContainerReadOnly : SO_A_BoolReadOnly
{
    [SerializeField] SO_A_Bool m_boolObject = default;

    public override bool IsTrue
    {
        get => m_boolObject.IsTrue;
        set { }
    }
}
