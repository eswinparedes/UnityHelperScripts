using UnityEngine;

public class M_OnCollision2DEvent : MonoBehaviour
{
    [SerializeField] UnityEvent_Collision2D m_onCollisionEnterEvent = new UnityEvent_Collision2D();
    [SerializeField] UnityEvent_Collision2D m_onCollisionStayEvent = new UnityEvent_Collision2D();
    [SerializeField] UnityEvent_Collision2D m_onCollisionExitEvent = new UnityEvent_Collision2D();

    public UnityEvent_Collision2D OnCollisionEnterEvent => m_onCollisionEnterEvent;
    public UnityEvent_Collision2D OnCollisionStayEvent => m_onCollisionStayEvent;
    public UnityEvent_Collision2D OnCollisionExitEvent => m_onCollisionExitEvent;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_onCollisionEnterEvent.Invoke(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        m_onCollisionStayEvent.Invoke(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        m_onCollisionExitEvent.Invoke(collision);
    }
}
