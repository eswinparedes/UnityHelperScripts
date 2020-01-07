using UnityEngine;

namespace SUHScripts
{
    using Functional;
    public struct SphereCastData 
    {
        public readonly Vector3 Origin;
        public readonly Vector3 Direction;
        public readonly float Radius;
        public Option<RaycastHit> HitData;

        public SphereCastData(Vector3 origin, Vector3 direction,
            float radius, Option<RaycastHit> hitData)
        {
            this.Origin = origin;
            this.Direction = direction;
            this.Radius = radius;
            this.HitData = hitData;
        }
    }
}