using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Raycaster
{
    [SerializeField] UnityEvent_C_RaycastOutcome m_onRaycastHitEvent = new UnityEvent_C_RaycastOutcome();
    [SerializeField] UnityEvent_C_RaycastOutcome m_onRaycastMissEvent = new UnityEvent_C_RaycastOutcome();
    [SerializeField] UnityEvent_C_RaycastOutcome m_onRaycastAnyEvent = new UnityEvent_C_RaycastOutcome();

    public UnityEvent_C_RaycastOutcome OnRaycastMiss => m_onRaycastMissEvent;
    public UnityEvent_C_RaycastOutcome OnRaycastAny => m_onRaycastAnyEvent;
    public UnityEvent_C_RaycastOutcome OnRaycastHitEvent => m_onRaycastHitEvent;

    RaycastOutcome m_outcome = new RaycastOutcome();

    public RaycastOutcome Outcome => m_outcome;

    public void Raycast(Ray ray, float range = float.PositiveInfinity, LayerMask? mask = null)
    {
        LayerMask _mask = mask == null ? (LayerMask) ~0 : mask.Value;
        bool didHit = m_outcome.WorldCastGetOutcome(ray, range, _mask);

        m_onRaycastAnyEvent.Invoke(m_outcome);

        if (m_outcome.WorldCastGetOutcome(ray, range, _mask))
            m_onRaycastHitEvent.Invoke(m_outcome);
        else
            m_onRaycastMissEvent.Invoke(m_outcome);
    }

    public void Raycast(Vector3 origin, Vector3 direction, float range = float.PositiveInfinity, LayerMask? mask = null)
    {
        Raycast(new Ray(origin, direction), range, mask);
    }

    public void Raycast(Transform rayIdentity, float range = float.PositiveInfinity, LayerMask? mask = null)
    {
        Raycast(new Ray(rayIdentity.position, rayIdentity.forward), range, mask);
    }

    public void Raycast(Camera screen, Vector3 screenPoint, float range = float.PositiveInfinity, LayerMask? mask = null)
    {
        Ray ray = screen.ScreenPointToRay(screenPoint);
        Raycast(ray, range, mask);
    }
}
