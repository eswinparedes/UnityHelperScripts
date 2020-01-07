using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Other/Color/Color")]
public class SO_Color : SO_A_Color {

    [SerializeField] Color m_color;

    public override Color color { get { return m_color; } set { m_color = value; } }
}