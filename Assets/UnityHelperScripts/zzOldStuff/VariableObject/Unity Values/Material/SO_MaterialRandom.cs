using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Unity Values/Material/Material Random")]
public class SO_MaterialRandom : SO_A_Material{

    [Header("GETTER ONLY")]
    [SerializeField] SO_MaterialArray m_materials = default;

    int m_idx = 0;

    public int LastSelectedIndex { get { return m_idx; } }
    public SO_MaterialArray MaterialOptions { get { return m_materials; } }

    public override Material Mat
    {
        get
        {
            m_idx = Random.Range(0, m_materials.Length);
            return m_materials[m_idx];
        }

        set { }
    }
}
