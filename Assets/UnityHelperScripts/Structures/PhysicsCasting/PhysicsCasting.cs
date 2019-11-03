using UnityEngine;
using static SUHScripts.Functional.Functional;

namespace SUHScripts.Functional
{
    public static class PhysicsCasting
    {
        //SUHS todo: Create overloads that just get an Option<RaycastHit> for lightweight version
        #region Raycasting
        //TODO make speparate struct?
        public static RaycastData PhysicsLinecast(Vector3 prev, Vector3 next, LayerMask mask)
        {
            Ray sourceRay = new Ray(prev, next - prev);
            bool didHit = Physics.Linecast(prev, next, out RaycastHit hit, mask);
            return new RaycastData(sourceRay, didHit ? hit : (Option<RaycastHit>)NONE);
        }

        public static RaycastData Raycast(Ray ray, float range = float.PositiveInfinity, LayerMask? mask = null)
        {
            LayerMask _mask = mask == null ? (LayerMask)~0 : mask.Value;
            return WorldCastGetOutcome(ray.origin, ray.direction, range, _mask);
        }

        public static RaycastData Raycast(Vector3 origin, Vector3 direction, float range = float.PositiveInfinity, LayerMask? mask = null) =>
            Raycast(new Ray(origin, direction), range, mask);

        public static RaycastData Raycast(Transform rayIdentity, float range = float.PositiveInfinity, LayerMask? mask = null) =>
            Raycast(new Ray(rayIdentity.position, rayIdentity.forward), range, mask);

        public static RaycastData Raycast(Camera screen, Vector3 screenPoint, float range = float.PositiveInfinity, LayerMask? mask = null) =>
            Raycast(screen.ScreenPointToRay(screenPoint), range, mask);

        public static RaycastData WorldCastGetOutcome(Vector3 origin, Vector3 direction, float range, LayerMask mask) =>
            WorldCastGetOutcome(new Ray(origin, direction), range, mask);

        public static RaycastData WorldCastGetOutcome(Ray ray, float range, LayerMask mask) =>
            Physics.Raycast(ray, out RaycastHit outcomeRaycastHit, range, mask) ?
                new RaycastData(ray, outcomeRaycastHit.AsOption()) :
                new RaycastData(ray, (Option<RaycastHit>)NONE);

        #endregion

        #region SphereCasting
        public static SphereCastData SphereCast(Vector3 origin, float radius, Vector3 direction, 
            float maxdistance, int layerMask = ~0, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            RaycastHit hit;
            bool didHit = 
                Physics.SphereCast(origin, radius, direction, out hit, maxdistance, layerMask, queryTriggerInteraction);

            var hitOption = didHit ? hit.AsOption() : NONE;

            return new SphereCastData(origin, direction, radius, hitOption);
        }
        #endregion
    }

}
