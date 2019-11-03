using UnityEngine;
using UnityEngine.Events;

public class M_ScriptableEventListener : MonoBehaviour
{
    [SerializeField] ScriptableEventGroup[] m_scriptableEvents = default;

    private void OnEnable()
    {
        for(int i = 0; i < m_scriptableEvents.Length; i++)
        {
            m_scriptableEvents[i].Subscribe();
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < m_scriptableEvents.Length; i++)
        {
            m_scriptableEvents[i].Unsubscribe();
        }
    }
}


