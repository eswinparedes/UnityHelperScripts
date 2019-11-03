using UnityEngine;
using UnityEngine.Events;

public class M_SelectableEvents : MonoBehaviour, I_Selectable
{
    [SerializeField] UnityEvent m_onSelectStartEvent = new UnityEvent();
    [SerializeField] UnityEvent m_onSelectEndEvent = new UnityEvent();

    public void OnSelectEnd()
    {
        Debug.Log("Select start");
        m_onSelectEndEvent.Invoke();
    }

    public void OnSelectStart()
    {
        Debug.Log("Select End");
        m_onSelectStartEvent.Invoke();
    }
}
