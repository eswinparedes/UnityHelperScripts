using UnityEngine;

public class M_TransformRaycaster : A_Component
{
    [SerializeField] Raycaster m_raycaster = null;
    [SerializeField] Transform m_identity = null;

    public override void Execute() =>
        m_raycaster.Raycast(m_identity);
}
