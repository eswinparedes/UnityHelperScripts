using UnityEngine;

public class M_OnTrigger2DEvent : MonoBehaviour
{
    [SerializeField] UnityEvent_Collider2D m_onTriggerEnterEvent = new UnityEvent_Collider2D();
    [SerializeField] UnityEvent_Collider2D m_onTriggerStayEvent = new UnityEvent_Collider2D();
    [SerializeField] UnityEvent_Collider2D m_onTriggerExitEvent = new UnityEvent_Collider2D();

    public UnityEvent_Collider2D OnTriggerEnterEvent => m_onTriggerEnterEvent;
    public UnityEvent_Collider2D OnTriggerStayEvent => m_onTriggerStayEvent;
    public UnityEvent_Collider2D OnTriggerExitEvent => m_onTriggerExitEvent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        m_onTriggerEnterEvent.Invoke(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        m_onTriggerStayEvent.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        m_onTriggerExitEvent.Invoke(other);
    }

}
