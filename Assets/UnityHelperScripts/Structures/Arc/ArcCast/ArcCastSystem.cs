using UnityEngine;
using SUHScripts.Functional;
using static SUHScripts.Functional.Functional;
using static SUHScripts.Functional.PhysicsCasting;
using static ArcDataSystem;
using System;
using System.Linq;

public static class ArcCastSystem
{
    public static (Option<RaycastHit> hitOption, Vector3[] points) 
        ArcCast(ArcSettings settings, ArcInput input, LayerMask mask)
    {
        Option<RaycastHit> hitOption = NONE;

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
