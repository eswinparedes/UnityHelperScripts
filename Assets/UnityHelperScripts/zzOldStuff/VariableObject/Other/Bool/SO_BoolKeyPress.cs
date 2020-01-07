using UnityEngine;
using SUHScripts;

[CreateAssetMenu(menuName = "Variables/Other/Bool/Key Pressed State Bool")]
public class SO_BoolKeyPress : SO_A_BoolReadOnly
{
    [SerializeField] KeyCode m_keyCode = KeyCode.Space;
    [SerializeField] KeyPressState m_pressState = KeyPressState.None;

    public override bool IsTrue
    {
        get
        {
            switch (m_pressState)
            {
                case KeyPressState.pressedThisFrame: return Input.GetKeyDown(m_keyCode);
                case KeyPressState.releasedThisFrame: return Input.GetKeyUp(m_keyCode);
                case KeyPressState.heldThisFrame: return Input.GetKey(m_keyCode);
                default: return Input.GetKey(m_keyCode);
            }
        }
        set { }
    }
}
