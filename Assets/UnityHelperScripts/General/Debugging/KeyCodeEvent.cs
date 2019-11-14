using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyCodeEvent : MonoBehaviour
{
    public List<KeyPressEvent> m_events = default;

    private void Update()
    {
        for(int i = 0; i < m_events.Count; i++)
        {
            var conditionMet = false;
            var key = m_events[i].m_key;
            switch (m_events[i].m_pressState)
            {
                case KeyPressState.pressedThisFrame: conditionMet = Input.GetKeyDown(key); break;
                case KeyPressState.heldThisFrame: conditionMet = Input.GetKey(key); break;
                case KeyPressState.releasedThisFrame: conditionMet = Input.GetKeyUp(key); break;
            }

            if (conditionMet)
                m_events[i].m_onStateMet.Invoke();
        }
    }
}

[System.Serializable]
public class KeyPressEvent
{
    public KeyCode m_key = default;
    public KeyPressState m_pressState = default;
    public UnityEvent m_onStateMet = default;
}
