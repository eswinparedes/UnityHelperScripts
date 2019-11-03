using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Maths/Vector3/Vector3 Flerp To Target")]
public class SO_Vector3FlerpTo : SO_A_Vector3ReadOnly
{
    [SerializeField] SO_A_Vector3 m_flerpTarget = default;
    [SerializeField] SO_A_Float m_deltaTime = default;
    [SerializeField] SO_A_Float m_flerpAmount = default;
    Vector3 m_currentFlerp;

    public override Vector3 Value
    {
        get
        {
            m_currentFlerp = Vector3.Lerp(m_currentFlerp, m_flerpTarget.Value, m_flerpAmount.Value * m_deltaTime.Value);
            return m_currentFlerp;
        }
        set { }
    }

}
