using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Unity Values/Ray/Ray Extended")]
public class SO_RayExtended : SO_A_RayReadWrite
{
    [SerializeField] SO_A_Vector3ReadWrite m_rayOriginValue = default;
    [SerializeField] SO_A_Vector3ReadWrite m_rayDirectionValue = default;

    Ray m_ray;
    public override Ray Value
    {
        get => m_ray;
        set
        {
            m_ray = value;
            m_rayOriginValue.Value = m_ray.origin;
            m_rayDirectionValue.Value = m_ray.direction;
        }
    }
}
