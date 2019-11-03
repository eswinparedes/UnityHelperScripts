using UnityEngine;

public class M_OnCollisionEvent : MonoBehaviour
{
    [SerializeField] UnityEvent_Collision m_onCollisionEnterEvent = default;
    [SerializeField] UnityEvent_Collision m_onCollisionStayEvent = default;
    [SerializeField] UnityEvent_Collision m_onCollisionExitEvent = default;

    public UnityEvent_Collision OnCollisionEnterEvent => m_onCollisionEnterEvent;
    public UnityEvent_Collision OnCollisionStayEvent => m_onCollisionStayEvent;
    public UnityEvent_Collision OnCollisionExitEvent => m_onCollisionExitEvent;

    private void OnCollisionEnter(Collision collision)
    {
        m_onCollisionEnterEvent.Invoke(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        m_onCollisionStayEvent.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        m_onCollisionExitEvent.Invoke(collision);
    }
}
