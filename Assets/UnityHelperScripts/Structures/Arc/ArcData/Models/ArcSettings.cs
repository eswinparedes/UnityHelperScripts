using UnityEngine;

[System.Serializable]
public struct ArcSettings
{
    [SerializeField] int m_maxVertexCount;
    [SerializeField] float m_vertexDelta;
    [SerializeField] float m_angle;
    [SerializeField] float m_strength;

    public ArcSettings(int maxVertexCount, float vertexDelta, float angle, float strength)
    {
        this.m_maxVertexCount = maxVertexCount;
        this.m_vertexDelta = vertexDelta;
        this.m_angle = angle;
        this.m_strength = strength;
    }
    public int MaxVertexCount => m_maxVertexCount;
    public float VertexDelta => m_vertexDelta;
    public float Angle => m_angle;
    public float Strength => m_strength;
}
