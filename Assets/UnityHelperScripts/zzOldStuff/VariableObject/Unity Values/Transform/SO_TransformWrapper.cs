using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Unity Values/Transform/Transform Wrapper")]
public class SO_TransformWrapper : SO_A_Transform
{
    [SerializeField] SO_Transform m_transformReference = default;

    public override Transform transform
    {
        get { return m_transformReference.transform; }
        set { }
    }
}
