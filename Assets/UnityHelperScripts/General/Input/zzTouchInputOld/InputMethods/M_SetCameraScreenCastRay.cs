using UnityEngine;

public class M_SetCameraScreenCastRay : A_Component
{
    [Header("Camera Ray")]
    [SerializeField] Camera m_camera = default;
    [SerializeField] SO_A_Vector3 m_screenPoint = default;
    [SerializeField] SO_A_RayReadWrite m_ray = default;

    public override void Execute()
    {
        m_ray.Value = m_camera.ScreenPointToRay(m_screenPoint.Value);
    }
}
