using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Maths/Quaternion/Quaternion")]
public class SO_Quaternion : SO_A_QuaternionReadWrite {

    [SerializeField] Quaternion m_value;

    public override Quaternion Value
    {
        get => m_value;
        set => m_value = value;
    }
}
