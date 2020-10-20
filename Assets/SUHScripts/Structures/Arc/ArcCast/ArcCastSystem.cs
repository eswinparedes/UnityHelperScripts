using UnityEngine;
using System;
using System.Linq;

namespace SUHScripts
{
    using Functional;
    using static ArcDataSystem;
    using static Functional.Functional;
    using static SUHScripts.PhysicsCasting;

    public static class ArcCastSystem
    {
        public static (Option<RaycastHit> hitOption, Vector3[] points) 
            ArcCast(ArcSettings settings, ArcInput input, LayerMask mask)
        {
            Option<RaycastHit> hitOption = None.Default;

            Func<Vector3, Vector3, bool> ArcCast = (p0, p1) =>
            {
                hitOption = PhysicsLinecast(p0, p1, mask).RaycastHitOption;
                return hitOption.IsSome;
            };

            var arcPoints =
                CalculateArcPositions(settings, input, ArcCast)
                .ToArray();

            return (hitOption, arcPoints);
        }

        public static (Option<RaycastHit> hitOption, Vector3[] points)
            ArcCast(this ArcInput input, ArcSettings settings, LayerMask mask) =>
            ArcCast(settings, input, mask);
    }

}
