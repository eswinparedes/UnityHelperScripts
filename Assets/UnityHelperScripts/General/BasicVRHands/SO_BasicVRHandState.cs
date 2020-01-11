using UnityEngine;

[CreateAssetMenu(menuName = "SUHS/Experimental/BasicVRHandState")]
public class SO_BasicVRHandState : ScriptableObject
{
    [SerializeField] BasicVRHandState m_state;

    public BasicVRHandState State
    {
        get
        {
            return m_state;
        }
        set
        {
            m_state = value;
        }
    }
}
