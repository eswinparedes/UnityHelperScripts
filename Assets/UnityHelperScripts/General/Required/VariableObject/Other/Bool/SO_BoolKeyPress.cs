using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Other/Bool/Key Pressed State Bool")]
public class SO_BoolKeyPress : SO_A_BoolReadOnly
{
    [SerializeField] KeyCode m_keyCode = KeyCode.Space;
    [SerializeField] E_KeyPressState m_pressState = E_KeyPressState.None;

    public override bool IsTrue
    {
        get
        {
            switch (m_pressState)
            {
                case E_KeyPressState.pressedThisFrame: return Input.GetKeyDown(m_keyCode);
                case E_KeyPressState.releasedThisFrame: return Input.GetKeyUp(m_keyCode);
                case E_KeyPressState.heldThisFrame: return Input.GetKey(m_keyCode);
                default: return Input.GetKey(m_keyCode);
            }
        }
        set { }
    }
}
