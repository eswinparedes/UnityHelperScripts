using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Unity Values/Ray/Ray Container")]
public class SO_RayContainer : SO_A_Ray
{
    [SerializeField] SO_A_Ray m_ray = default;

    public override Ray Value
    {
        get => m_ray.Value;
        set => m_ray.Value = value;
    }
}
