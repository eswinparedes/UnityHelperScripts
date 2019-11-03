using SUHScripts.Functional;
using UnityEngine;

public struct RaycastData
{
    public readonly Ray SourceRay;
    public readonly Option<RaycastHit> RaycastHitOption;

    public RaycastData(Ray sourceRay, Option<RaycastHit> raycastHitOption)
    {
        this.SourceRay = sourceRay;
        this.RaycastHitOption = raycastHitOption;
    }
}