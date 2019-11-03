using UnityEngine;

public class M_OnTriggerEvent : MonoBehaviour
{
    [SerializeField] UnityEvent_Collider m_onTriggerEnterEvent = new UnityEvent_Collider();
    [SerializeField] UnityEvent_Collider m_onTriggerStayEvent = new UnityEvent_Collider();
    [SerializeField] UnityEvent_Collider m_onTriggerExitEvent = new UnityEvent_Collider();

    public UnityEvent_Collider OnTriggerEnterEvent => m_onTriggerEnterEvent;
    public UnityEvent_Collider OnTriggerStayEvent => m_onTriggerStayEvent;
    public UnityEvent_Collider OnTriggerExitEvent => m_onTriggerExitEvent;


    private void OnTriggerEnter(Collider other)
    {
        m_onTriggerEnterEvent.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        m_onTriggerStayEvent.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        m_onTriggerExitEvent.Invoke(other);
    }

}
