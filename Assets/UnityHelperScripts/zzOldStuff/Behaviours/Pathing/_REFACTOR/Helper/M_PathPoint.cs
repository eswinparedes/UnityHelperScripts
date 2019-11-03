using UnityEngine;

public class M_PathPoint : MonoBehaviour, I_PathPoint
{
    [SerializeField] Transform m_transformPoint = default;

    public Transform TransformPoint { get => m_transformPoint; }

    public Vector3 PathPointPosition => m_transformPoint.position;
}
