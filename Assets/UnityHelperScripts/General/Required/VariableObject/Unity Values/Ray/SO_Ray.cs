using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Unity Values/Ray/Ray")]
public class SO_Ray : SO_A_RayReadWrite
{
    Ray m_ray;

    public override Ray Value
    {
        get => m_ray;
        set => m_ray = value;
    }
}