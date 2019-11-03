using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Unity Values/Material/Material Array")]
public class SO_MaterialArray : ScriptableObject {

    [SerializeField] Material[] m_materials = default;

    public Material[] MaterialArray { get { return m_materials; } }

    public Material this[int index] { get { return m_materials[index]; } }
    public int Length { get { return m_materials.Length; } }
}
