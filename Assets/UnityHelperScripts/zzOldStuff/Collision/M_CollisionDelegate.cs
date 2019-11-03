using UnityEngine;

public class M_CollisionDelegate : MonoBehaviour
{
    [Header("Generic Events")]
    [SerializeField] UnityEvent_GameObject m_onAnyEnterEvent = new UnityEvent_GameObject();
    [SerializeField] UnityEvent_GameObject m_onAnyStayEvent = new UnityEvent_GameObject();
    [SerializeField] UnityEvent_GameObject m_onAnyExitEvent = new UnityEvent_GameObject();

    public UnityEvent_GameObject OnAnyEnterEvent => m_onAnyEnterEvent = new UnityEvent_GameObject();
    public UnityEvent_GameObject OnAnyStayEvent => m_onAnyStayEvent = new UnityEvent_GameObject();
    public UnityEvent_GameObject OnAnyExitEvent => m_onAnyExitEvent = new UnityEvent_GameObject();

    private void OnCollisionEnter(Collision collision)
    {
        m_onAnyEnterEvent.Invoke(collision.gameObject) ;
    }

    private void OnCollisionStay(Collision collision)
    {
        m_onAnyStayEvent.Invoke(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        m_onAnyExitEvent.Invoke(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        m_onAnyEnterEvent.Invoke(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        m_onAnyStayEvent.Invoke(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        m_onAnyExitEvent.Invoke(other.gameObject);
    }
}
