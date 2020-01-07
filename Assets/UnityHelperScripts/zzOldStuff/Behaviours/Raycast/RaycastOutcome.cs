using UnityEngine;
using SUHScripts;

public class RaycastOutcome
{
    [HideInInspector] public Ray outcomeRay;
    [HideInInspector] public RaycastHit outcomeRaycastHit;
    [HideInInspector] public bool b_didHitThisCast;

    public void ResetData()
    {
        outcomeRaycastHit = new RaycastHit();
        outcomeRay = new Ray();

        b_didHitThisCast = false;
    }

    public bool HitHasComponent<T>(out T value) 
    {
        if (outcomeRaycastHit.collider != null)
        {
            return outcomeRaycastHit.collider.HasComponent(out value);
        }
        else
        {
            value = default;
            return false;
        }
    }

    public bool WorldCastGetOutcome(Vector3 origin, Vector3 direction, float range, LayerMask mask)
    {
        return WorldCastGetOutcome(new Ray(origin, direction * range), range, mask);
    }

    public bool WorldCastGetOutcome(Ray ray, float range, LayerMask mask)
    {
        ResetData();
        outcomeRay = ray;

        b_didHitThisCast = Physics.Raycast(ray, out outcomeRaycastHit, range, mask);

        return b_didHitThisCast;
    }
}
