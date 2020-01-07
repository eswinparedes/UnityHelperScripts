using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Unity Values/Transform/Transform")]
public class SO_Transform : SO_A_TransformReadWrite {

    [SerializeField] Transform m_transform;

    public override Transform transform
    {
        get { return m_transform; }
        set { m_transform = value; }
    }
}


