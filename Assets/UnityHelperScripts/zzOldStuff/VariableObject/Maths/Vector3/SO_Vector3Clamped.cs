using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Maths/Vector3/Vector3 Clamped")]
public class SO_Vector3Clamped : SO_A_Vector3ReadWrite
{
    [SerializeField] Vector3 m_value = default;
    [SerializeField] Vector2 m_xMinMax = default;
    [SerializeField] Vector2 m_yMinMax = default;
    [SerializeField] Vector2 m_zMinMax = default;

    public override Vector3 Value { get => m_value; set { m_value = ReturnClamped(value); } }

    public Vector3 ReturnClamped(Vector3 value)
    {
        float x = value.x < m_xMinMax.x ? m_xMinMax.x : value.x > m_xMinMax.y ? m_xMinMax.y : value.x;
        float y = value.y < m_yMinMax.x ? m_yMinMax.x : value.y > m_yMinMax.y ? m_yMinMax.y : value.y;
        float z = value.z < m_zMinMax.x ? m_zMinMax.x : value.z > m_zMinMax.y ? m_zMinMax.y : value.z;

        return new Vector3(x, y, z);
    }
}
